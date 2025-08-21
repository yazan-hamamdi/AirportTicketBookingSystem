using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IBookingService
    {
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(int id, Booking newBooking);
        void DeleteBooking(int id);
        List<Booking> GetAllBookings();
    }

}
