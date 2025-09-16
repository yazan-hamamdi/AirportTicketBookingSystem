using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.DTOs;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Utilities;
using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Adapters;

namespace AirportTicketBookingSystem.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(string filePath, ICsvFileHelperAdapter csvHelper)
            : base(filePath, csvHelper) { }

        public override List<Booking> GetAll()
        {
            var bookingCsvRecords = base.GetAll<BookingCsv>();

            return bookingCsvRecords.Select(r => new Booking(
                r.Id,
                r.FlightId,
                r.PassengerId,
                r.BookingDate,
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

        public List<Booking> GetByPassengerId(int passengerId)
        {
            var allBookings = GetAll();
            return allBookings.Where(b => b.PassengerId == passengerId).ToList();
        }

    }
}
