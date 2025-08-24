using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.DTOs;
using AirportTicketBookingSystem.Interfaces;
using System.Linq;
using AirportTicketBookingSystem.Utilities;
using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _filePath;

        public BookingRepository(string filePath)
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
                r.SeatClass.ToSeatClass(),
                r.Price
            )).ToList();
        }

        public void AddBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            var bookingRecords = GetAllBookings();
            booking.Id = IdGenerator.GenerateNewId(bookingRecords);

            bookingRecords.Add(booking);
            Save(bookingRecords);
        }

        public void UpdateBooking(int bookingId, Booking newBooking)
        {
            if (newBooking is null) throw new ArgumentNullException(nameof(newBooking));

            var bookingRecords = GetAllBookings();
            var index = bookingRecords.FindIndex(b => b.Id == bookingId);

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

        public List<Booking> FilterBookingsForManager(int? flightId = null, int? passengerId = null,
            DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null, TravelClass? seatClass = null,
            decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allBookings = GetAllBookings();

            var query =
                from b in allBookings
                where (!flightId.HasValue || b.FlightId == flightId.Value)
                where (!passengerId.HasValue || b.PassengerId == passengerId.Value)
                where (!bookingDateFrom.HasValue || b.BookingDate >= bookingDateFrom.Value)
                where (!bookingDateTo.HasValue || b.BookingDate <= bookingDateTo.Value)
                where (!seatClass.HasValue || b.SeatClass.Name == seatClass.Value)
                where (!minPrice.HasValue || b.SeatClass.CalculatePrice() >= minPrice.Value)
                where (!maxPrice.HasValue || b.SeatClass.CalculatePrice() <= maxPrice.Value)
                select b;

            return query.ToList();
        }

        public void DeleteBookings(Func<Booking, bool> predicate)
        {
            var allBookings = GetAllBookings();
            var relatedBookings = allBookings.Where(predicate).ToList();

            if (relatedBookings.Count == 0)
                throw new KeyNotFoundException("No bookings found matching the specified criteria");

            foreach (var booking in relatedBookings)
            {
                allBookings.Remove(booking);
            }

            Save(allBookings);
        }

    }
}
