using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IPassengerRepo
    {
        public Passenger GetPassengerById(int id);
        List<Passenger> GetAll();
        void Add(Passenger passenger);
        void Update(Func<Passenger, bool> predicate, Passenger newPassenger);
        void DeleteAll(Func<Passenger, bool> predicate);
    }
}
