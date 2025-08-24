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
        void ModifyBooking(int passengerId, int bookingId, Booking newBooking);
    }

}
