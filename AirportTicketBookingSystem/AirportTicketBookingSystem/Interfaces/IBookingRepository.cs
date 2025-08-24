using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IBookingRepository
    {
        Booking GetBookingById(int id);
        void DeleteBooking(int id);
        List<Booking> GetAllBookings();
        void AddBooking(Booking booking);
        void UpdateBooking(Func<Booking, bool> predicate, Booking newBooking);
        List<Booking> FilterBookingsForManager(
        int? flightId = null,
        int? passengerId = null,
        DateTime? bookingDateFrom = null,
        DateTime? bookingDateTo = null,
        TravelClass? seatClass = null,
        decimal? minPrice = null,
        decimal? maxPrice = null );
        void DeleteBookings(Func<Booking, bool> predicate);
    }
}
