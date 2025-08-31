using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IFlightService : IService<Flight>
    {
        List<FieldValidationDetail> GetFlightModelValidationDetails();
        List<string> ImportFlightsFromCsv(string csvFilePath);
        bool IsDuplicateFlight(Flight flight, List<Flight> existingFlights);
    }
}
