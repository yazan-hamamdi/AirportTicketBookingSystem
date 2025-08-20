using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IBookingService
    {
        Booking GetBookingById(int id);
        List<Booking> GetAllBookings();
        void AddBooking(Booking booking);
        void UpdateBooking(int id, Booking updatedBooking);
        void DeleteBooking(int id);
    }

}
