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
        void AddFlight(Flight flight);
        void UpdateFlight(int flightId, Flight newFlight);
        void DeleteFlight(int id);
        //void ImportFlights(List<Flight> flights);
        List<Flight> GetAllFlights();
        void AddFlightWithBookings(Flight newFlight, List<Booking> bookings);

    }
}
