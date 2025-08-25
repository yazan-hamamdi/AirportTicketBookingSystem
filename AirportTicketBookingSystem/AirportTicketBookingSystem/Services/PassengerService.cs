using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : BaseService<Passenger>, IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public PassengerService(
            IPassengerRepository passengerRepository,
            IBookingRepository bookingRepository,
            IFlightRepository flightRepository)
            : base(passengerRepository)
        {
            _passengerRepository = passengerRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public List<Passenger> GetAllPassengersWithBookings()
        {
            var passengers = _passengerRepository.GetAll();
            var bookings = _bookingRepository.GetAll();
            PassengerBookingUtilities.AttachBookings(passengers, bookings);
            return passengers;
        }

        public Passenger GetPassengerByIdWithBookings(int id)
        {
            var passenger = _passengerRepository.GetById(id);
            var bookings = _bookingRepository.GetAll();

            PassengerBookingUtilities.AttachBookings(new List<Passenger> { passenger }, bookings);

            return passenger;
        }

        public void DeletePassengerWithBookings(int passengerId)
        {
            var passenger = _passengerRepository.GetById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            _bookingRepository.DeleteBookings(b => b.PassengerId == passengerId);
            _passengerRepository.Delete(passengerId);
        }

        public void AddPassengerWithBookings(Passenger newPassenger, List<Booking> bookings)
        {
            _passengerRepository.Add(newPassenger);
            if (bookings == null || bookings.Count == 0)
                throw new ArgumentException("Bookings list cannot be null or empty when adding a passenger with bookings");

            foreach (var booking in bookings)
            {
                booking.PassengerId = newPassenger.Id;
                _bookingRepository.Add(booking);
            }
        }

    }
}
