using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepo : IFlightRepo
    {
        private readonly string _filePath;
        private readonly BookingRepo _bookingRepo;

        public FlightRepo(string filePath, BookingRepo bookingRepo)
        {
            _filePath = filePath;
            _bookingRepo = bookingRepo;
        }

        public List<Flight> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var flights = csv.GetRecords<Flight>().ToList();
            var allBookings = _bookingRepo.GetAll();

            var bookingsByFlight = allBookings
                .GroupBy(b => b.FlightId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var flight in flights)
            {
                if (bookingsByFlight.TryGetValue(flight.Id, out var bookings))
                {
                    flight.Bookings = bookings;
                }
            }

            return flights;
        }

        public void Add(Flight flight)
        {
            if (flight == null)
                throw new ArgumentNullException(nameof(flight));

            var records = GetAll();
            records.Add(flight);

            Save(records);

            if (flight.Bookings.Count > 0)
            {
                flight.Bookings.ForEach(_bookingRepo.Add);
            }
        }

        public void DeleteAll(Func<Flight, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var records = GetAll();
            var flightsToDelete = records.Where(predicate).ToList();

            if (!flightsToDelete.Any())
                throw new InvalidOperationException("No flight found to delete");

            records.RemoveAll(f => predicate(f));
            Save(records);

            foreach (var flight in flightsToDelete)
            {
                _bookingRepo.DeleteAll(b => b.FlightId == flight.Id);
            }
        }

        public void Update(Func<Flight, bool> predicate, Flight newFlight)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newFlight is null) throw new ArgumentNullException(nameof(newFlight));

            var records = GetAll();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new InvalidOperationException("Flight not found to update");

            records[index] = newFlight;
            Save(records);

            foreach (var booking in newFlight.Bookings)
            {
                _bookingRepo.Update(b => b.Id == booking.Id, booking);
            }
        }

        private void Save(List<Flight> flights)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(flights);
        }

    }
}
