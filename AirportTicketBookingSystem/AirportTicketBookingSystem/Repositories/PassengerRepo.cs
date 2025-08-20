using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class PassengerRepo : IPassengerRepo
    {
        private readonly string _filePath;
        private readonly BookingRepo _bookingRepo;

        public PassengerRepo(string filePath, BookingRepo bookingRepo)
        {
            _filePath = filePath;
            _bookingRepo = bookingRepo;
        }

        public Passenger GetPassengerById(int id)
        {
            var passengerRecords = GetAll();
            var selectedPassenger = passengerRecords.FirstOrDefault(b => b.Id == id);

            if (selectedPassenger != null)
            {
                return selectedPassenger;
            }
            throw new KeyNotFoundException($"Passenger with Id {id} was not found");
        }

        public List<Passenger> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var passengers = csv.GetRecords<Passenger>().ToList();
            var allBookings = _bookingRepo.GetAll();

            var bookingsByPassenger = allBookings
                .GroupBy(b => b.PassengerId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var passenger in passengers)
            {
                if (bookingsByPassenger.TryGetValue(passenger.Id, out var bookings))
                {
                    passenger.Bookings = bookings;
                }
            }

            return passengers;
        }

        public void Add(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException(nameof(passenger));

            var records = GetAll();
            records.Add(passenger);

            Save(records);

            if (passenger.Bookings != null && passenger.Bookings.Count > 0)
            {
                passenger.Bookings.ForEach(_bookingRepo.Add);
            }
        }

        public void DeleteAll(Func<Passenger, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var records = GetAll();
            var removedCount = records.RemoveAll(r => predicate(r));

            if (removedCount == 0)
                throw new InvalidOperationException("No passenger found to delete");

            Save(records);

            foreach (var passenger in records.Where(predicate))
            {
                _bookingRepo.DeleteAll(b => b.PassengerId == passenger.Id);
            }
        }

        public void Update(Func<Passenger, bool> predicate, Passenger newPassenger)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newPassenger is null) throw new ArgumentNullException(nameof(newPassenger));

            var records = GetAll();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new InvalidOperationException("Passenger not found to update");

            records[index] = newPassenger;
            Save(records);

            foreach (var booking in newPassenger.Bookings)
            {
                _bookingRepo.Update(b => b.Id == booking.Id, booking);
            }
        }

        private void Save(List<Passenger> passengers)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(passengers);
        }

    }
}
