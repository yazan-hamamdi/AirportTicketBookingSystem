using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IPassengerRepo
    {
        public Passenger GetById(int id);
        void DeleteById(int id);
        List<Passenger> GetAll();
        void Add(Passenger passenger);
        void Update(Func<Passenger, bool> predicate, Passenger newPassenger);
        void DeleteWhere(Func<Passenger, bool> predicate);
    }
}
