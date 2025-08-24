using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _filePath;
        private readonly IBookingRepository _bookingRepository;

        public FlightRepository(string filePath)
        {
            _filePath = filePath;
        }

        public Flight GetFlightById(int id)
        {
            var flightRecords = GetAllFlights();
            var selectedFlight = flightRecords.FirstOrDefault(b => b.Id == id);

            return selectedFlight ?? throw new KeyNotFoundException($"Flight with Id {id} was not found");
        }

        public void DeleteFlight(int id)
        {
            var flightRecords = GetAllFlights();
            var selectedFlight = flightRecords.FirstOrDefault(b => b.Id == id);

            if (selectedFlight == null)
                throw new KeyNotFoundException($"Flight with Id {id} was not found");

            flightRecords.Remove(selectedFlight);

            Save(flightRecords);
        }

        public List<Flight> GetAllFlights()
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

            return flights;
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null)
                throw new ArgumentNullException(nameof(flight));

            var records = GetAllFlights();
            records.Add(flight);

            Save(records);
        }

        public void UpdateFlight(Func<Flight, bool> predicate, Flight newFlight)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newFlight is null) throw new ArgumentNullException(nameof(newFlight));

            var records = GetAllFlights();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new KeyNotFoundException("Flight not found to update");

            records[index] = newFlight;
            Save(records);
        }

        private void Save(List<Flight> flights)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(flights);
        }

        public List<Flight> SearchAvailableFlights(string departureCountry = null, string destinationCountry = null,
            string departureAirport = null, string arrivalAirport = null, DateTime? departureDateFrom = null,
            DateTime? departureDateTo = null, TravelClass? seatClass = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allFlights = GetAllFlights();
            var allBookings = _bookingRepository.GetAllBookings();

            FlightBookingUtilities.AttachBookings(allFlights, allBookings);

            var query =
            from f in allFlights
            where (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
            where (!departureDateFrom.HasValue || f.DepartureDate >= departureDateFrom.Value)
            where (!departureDateTo.HasValue || f.DepartureDate <= departureDateTo.Value)
            where (!seatClass.HasValue || f.Bookings.Any(b => b.SeatClass.Name == seatClass.Value))
            where (!minPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() >= minPrice.Value))
            where (!maxPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() <= maxPrice.Value))
            select f;

            return query.ToList();
        }
    }
}
