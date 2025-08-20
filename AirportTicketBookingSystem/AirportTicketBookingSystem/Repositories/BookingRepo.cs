using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.DTOs;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class BookingRepo : IBookingRepo
    {
        private readonly string _filePath;

        public BookingRepo(string filePath)
        {
            _filePath = filePath;
        }

        public Booking GetBookingById(int id)
        {
            var bookingRecords = GetAllBookings();
            var selectedBooking = bookingRecords.FirstOrDefault(b => b.Id == id);

            return selectedBooking ?? throw new KeyNotFoundException($"Booking with Id {id} was not found.");
        }

        public void DeleteBooking(int id)
        {
            var bookingRecords = GetAllBookings();
            var selectedBooking = bookingRecords.FirstOrDefault(b => b.Id == id);

            if (selectedBooking == null)
                throw new KeyNotFoundException($"Booking with Id {id} was not found");

            bookingRecords.Remove(selectedBooking);

            Save(bookingRecords);
        }

        public List<Booking> GetAllBookings()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            return csv.GetRecords<BookingCsv>().Select(r => new Booking(
                r.Id,
                r.FlightId,
                r.PassengerId,
                r.BookingDate,
                "",
                SeatClassFactory.FromString(r.SeatClass),
                r.Price
            )).ToList();
        }

        public void AddBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            var bookingRecords = GetAllBookings();
            bookingRecords.Add(booking);
            Save(bookingRecords);
        }

        public void UpdateBooking(Func<Booking, bool> predicate, Booking newBooking)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newBooking is null) throw new ArgumentNullException(nameof(newBooking));

            var bookingRecords = GetAllBookings();
            var index = bookingRecords.FindIndex(r => predicate(r));

            if (index < 0)
               throw new KeyNotFoundException("Booking not found to update");

            bookingRecords[index] = newBooking;
            Save(bookingRecords);
        }

        private void Save(List<Booking> bookings)
        {
            if (bookings == null)
                throw new ArgumentNullException(nameof(bookings));

            var bookingRecords = bookings.Select(b => new BookingCsv
            {
                Id = b.Id,
                FlightId = b.FlightId,
                PassengerId = b.PassengerId,
                BookingDate = b.BookingDate,
                SeatClass = b.SeatClass.GetType().Name.Replace("Class", ""),
                Price = b.Price
            });

            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(bookingRecords);
        }

    }
}
