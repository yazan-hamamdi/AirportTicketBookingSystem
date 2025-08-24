using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly IBookingRepository _bookingRepo;

        public FlightService(IFlightRepository flightRepo, IBookingRepository bookingRepo)
        {
            _flightRepo = flightRepo;
            _bookingRepo = bookingRepo;
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

        public List<Flight> GetAllFlightsWithBookings()
        {
            var flights = _flightRepo.GetAllFlights();
            var bookings = _bookingRepo.GetAllBookings();
            FlightBookingUtilities.AttachBookings(flights, bookings);
            return flights;
        }

        public Flight GetFlightByIdWithBookings(int id)
        {
            var flight = _flightRepo.GetFlightById(id);
            var bookings = _bookingRepo.GetAllBookings();

            FlightBookingUtilities.AttachBookings(new List<Flight> { flight }, bookings);

            return flight;
        }

        public void DeleteFlightWithBookings(int flightId)
        {
            var flight = _flightRepo.GetFlightById(flightId);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with Id {flightId} does not exist");

            var allBookings = _bookingRepo.GetAllBookings();
            FlightBookingUtilities.CascadeDeleteBookings(flightId, allBookings, _bookingRepo.DeleteBooking);

            _flightRepo.DeleteFlight(flightId);
        }

        public void AddFlightWithBookings(Flight flight)
        {
            if (flight == null) throw new ArgumentNullException(nameof(flight));

            _flightRepo.AddFlight(flight);

            FlightBookingUtilities.CascadeAddBookings(flight, _bookingRepo.AddBooking);
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

        public void ImportFlights(List<Flight> flights)
        {
            if (flights == null || !flights.Any())
                throw new ArgumentException("No flights provided for import");

            foreach (var flight in flights)
            {
                if (flight == null)
                    throw new ArgumentNullException(nameof(flight), "One of the flights is null");

                var existing = _flightRepo.GetAllFlights().Any(f => f.Id == flight.Id);
                if (existing)
                    throw new InvalidOperationException($"Flight with Id {flight.Id} already exists");

                _flightRepo.AddFlight(flight);
            }
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepo.GetAllFlights();
        }

    }
}
