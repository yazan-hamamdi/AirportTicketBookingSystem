using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IBookingRepo
    {
        Booking GetById(int id);
        void DeleteById(int id);
        List<Booking> GetAll();
        void Add(Booking booking);
        void Update(Func<Booking, bool> predicate, Booking newBooking);
        void DeleteWhere(Func<Booking, bool> predicate);   
    }
}
