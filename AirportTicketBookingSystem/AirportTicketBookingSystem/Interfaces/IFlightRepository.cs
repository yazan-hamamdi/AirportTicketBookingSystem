using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IFlightRepository
    {
        public Flight GetFlightById(int id);
        void DeleteFlight(int id);
        List<Flight> GetAllFlights(string? filePath = null);
        void AddFlight(Flight flight);
        void UpdateFlight(int flightId, Flight newFlight);
        List<Flight> SearchAvailableFlights(
         string departureCountry = null,
         string destinationCountry = null,
         string departureAirport = null,
         string arrivalAirport = null,
         DateTime? departureDateFrom = null,
         DateTime? departureDateTo = null,
         TravelClass? seatClass = null,
         decimal? minPrice = null,
         decimal? maxPrice = null
       );
    }
}
