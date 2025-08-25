using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IBookingService
    {
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(int id, Booking newBooking);
        void DeleteBooking(int id);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsForPassenger(int passengerId);
        void CancelBooking(int passengerId, int bookingId);
        void UpdateBooking(int passengerId, int bookingId, Booking newBooking);
        List<Booking> FilterBookingsForManager(int? flightId = null, int? passengerId = null,
       DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null,
       TravelClass? seatClass = null, decimal? minPrice = null, decimal? maxPrice = null);
    }
}
