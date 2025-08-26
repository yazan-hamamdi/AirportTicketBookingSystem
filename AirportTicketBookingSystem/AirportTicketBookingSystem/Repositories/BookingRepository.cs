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
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(string filePath) : base(filePath) { }

        public override List<Booking> GetAll()
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

        public List<Booking> FilterBookingsForManager(int? flightId = null, int? passengerId = null,
            DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null, TravelClass? seatClass = null,
            decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allBookings = GetAll();

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
    }
}
