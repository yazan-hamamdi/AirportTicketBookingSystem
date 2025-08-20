using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IFlightRepo
    {
        public Flight GetById(int id);
        void DeleteById(int id);
        List<Flight> GetAll();
        void Add(Flight flight);
        void Update(Func<Flight, bool> predicate, Flight newFlight);
        void DeleteWhere(Func<Flight, bool> predicate);
    }
}
