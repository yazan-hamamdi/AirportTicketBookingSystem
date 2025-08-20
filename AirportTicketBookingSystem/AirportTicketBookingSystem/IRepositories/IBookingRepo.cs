using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IBookingRepo
    {
        Booking GetBookingById(int id);
        void DeleteBooking(int id);
        List<Booking> GetAllBookings();
        void AddBooking(Booking booking);
        void UpdateBooking(Func<Booking, bool> predicate, Booking newBooking);
    }
}
