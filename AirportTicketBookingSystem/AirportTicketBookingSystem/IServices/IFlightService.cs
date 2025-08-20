using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IFlightService
    {
        public Flight GetFlightById(int id);
        void DeleteFlight(int id);
        List<Flight> GetAllFlights();
        void AddFlight(Flight flight);
        void UpdateFlight(Func<Flight, bool> predicate, Flight newFlight);
    }
}
