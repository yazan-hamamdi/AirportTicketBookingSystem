using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IFlightRepository : IRepository<Flight>
    {
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
