using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IFlightService
    {
        Flight GetFlightById(int id);
        List<Flight> GetAllFlightsWithBookings();
        Flight GetFlightByIdWithBookings(int id);
        void DeleteFlightWithBookings(int flightId);
        void AddFlightWithBookings(Flight flight);
        void AddFlight(Flight flight);
        void UpdateFlight(Func<Flight, bool> predicate, Flight newFlight);
        void DeleteFlight(int id);
        void ImportFlights(List<Flight> flights);
        List<Flight> GetAllFlights();
    }
}
