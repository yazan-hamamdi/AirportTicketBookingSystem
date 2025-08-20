using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IBookingRepo
    {
        Booking GetById(int id);
        List<Booking> GetAll();
        void Add(Booking booking);
        void Update(Func<Booking, bool> predicate, Booking newBooking);
        void DeleteAll(Func<Booking, bool> predicate);
    }
}
