using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IFlightRepo
    {
        List<Flight> GetAll();
        void Add(Flight flight);
        void Update(Func<Flight, bool> predicate, Flight newFlight);
        void DeleteAll(Func<Flight, bool> predicate);
    }
}
