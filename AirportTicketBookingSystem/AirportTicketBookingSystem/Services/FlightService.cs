using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Utilities;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace AirportTicketBookingSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingRepository _bookingRepository;

        public FlightService(IFlightRepository flightRepo, IBookingRepository bookingRepo)
        {
            _flightRepository = flightRepo;
            _bookingRepository = bookingRepo;
        }

        public Flight GetFlightById(int id)
        {
            try
            {
                return _flightRepository.GetFlightById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Flight with Id {id} does not exist");
            }
        }

        public List<Flight> GetAllFlightsWithBookings()
        {
            var flights = _flightRepository.GetAllFlights();
            var bookings = _bookingRepository.GetAllBookings();
            FlightBookingUtilities.AttachBookings(flights, bookings);
            return flights;
        }

        public Flight GetFlightByIdWithBookings(int id)
        {
            var flight = _flightRepository.GetFlightById(id);
            var bookings = _bookingRepository.GetAllBookings();

            FlightBookingUtilities.AttachBookings(new List<Flight> { flight }, bookings);

            return flight;
        }

        public void DeleteFlightWithBookings(int flightId)
        {
            var flight = _flightRepository.GetFlightById(flightId);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with Id {flightId} does not exist");

            _bookingRepository.DeleteBookings(b => b.FlightId == flightId);
            _flightRepository.DeleteFlight(flightId);
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null) throw new ArgumentNullException(nameof(flight));
            _flightRepository.AddFlight(flight);
        }

        public void UpdateFlight(int flightId, Flight newFlight)
        {
            if (newFlight == null) throw new ArgumentNullException(nameof(newFlight));

            try
            {
                _flightRepository.UpdateFlight(flightId, newFlight);
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
                _flightRepository.DeleteFlight(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Flight with Id {id} does not exist");
            }
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepository.GetAllFlights();
        }

        public void AddFlightWithBookings(Flight newFlight, List<Booking> bookings)
        {
            _flightRepository.AddFlight(newFlight);

            if (bookings == null || bookings.Count == 0)
                throw new ArgumentException("Bookings list cannot be null or empty when adding a flight with bookings");

            foreach (var booking in bookings)
            {
                booking.FlightId = newFlight.Id; 
               _bookingRepository.AddBooking(booking); 
            }
        }

    }
}
