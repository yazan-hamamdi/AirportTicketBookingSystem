using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepo _flightRepo;

        public FlightService(IFlightRepo flightRepo)
        {
            _flightRepo = flightRepo;
        }

        public Flight GetFlightById(int id)
        {
            try
            {
                return _flightRepo.GetFlightById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Flight with Id {id} does not exist");
            }
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null) throw new ArgumentNullException(nameof(flight));
            _flightRepo.AddFlight(flight);
        }

        public void UpdateFlight(Func<Flight, bool> predicate, Flight newFlight)
        {
            if (newFlight == null) throw new ArgumentNullException(nameof(newFlight));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                _flightRepo.UpdateFlight(predicate, newFlight);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Cannot update. Flight not found");
            }
        }

        public void DeleteFlight(int id)
        {
            try
            {
                _flightRepo.DeleteFlight(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Flight with Id {id} does not exist");
            }
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepo.GetAllFlights();
        }

    }
}
