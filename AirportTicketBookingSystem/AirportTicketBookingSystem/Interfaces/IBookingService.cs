using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IBookingService : IService<Booking>
    {
        List<Booking> GetBookingsForPassenger(int passengerId);
        void CancelBooking(int passengerId, int bookingId);
        void UpdateBooking(int passengerId, int bookingId, Booking newBooking);
        List<Booking> FilterBookingsForManager(int? flightId = null, int? passengerId = null,
            DateTime? bookingDateFrom = null,
            DateTime? bookingDateTo = null,
            TravelClass? seatClass = null,
            decimal? minPrice = null,
            decimal? maxPrice = null
        );
    }
}
