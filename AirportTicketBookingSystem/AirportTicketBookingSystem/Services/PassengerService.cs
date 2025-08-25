using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public PassengerService(IPassengerRepository passengerRepo, IBookingRepository bookRepo, IFlightRepository flightRepo)
        {
            _passengerRepository = passengerRepo;
            _bookingRepository = bookRepo;
            _flightRepository = flightRepo;
        }

        public Passenger GetPassengerById(int id)
        {
            try
            {
                return _passengerRepository.GetById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Passenger with Id {id} does not exist");
            }
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

        public void AddPassenger(Passenger passenger)
        {
            if (passenger == null) throw new ArgumentNullException(nameof(passenger));
            _passengerRepository.Add(passenger);
        }

        public void UpdatePassenger(int id, Passenger newPassenger)
        {
            if (newPassenger == null) throw new ArgumentNullException(nameof(newPassenger));

            try
            {
                _passengerRepository.Update(id, newPassenger);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. Passenger with Id {id} does not exist");
            }
        }

        public void DeletePassenger(int id)
        {
            try
            {
                _passengerRepository.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Passenger with Id {id} does not exist");
            }
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengerRepository.GetAll();
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
