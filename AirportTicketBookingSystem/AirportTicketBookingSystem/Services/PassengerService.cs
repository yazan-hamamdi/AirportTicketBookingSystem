using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IFlightRepository _flightRepo;

        public PassengerService(IPassengerRepository passengerRepo, IBookingRepository bookRepo, IFlightRepository flightRepo)
        {
            _passengerRepo = passengerRepo;
            _bookingRepo = bookRepo;
            _flightRepo = flightRepo;
        }

        public Passenger GetPassengerById(int id)
        {
            try
            {
                return _passengerRepo.GetPassengerById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Passenger with Id {id} does not exist");
            }
        }

        public List<Passenger> GetAllPassengersWithBookings()
        {
            var passengers = _passengerRepo.GetAllPassengers();
            var bookings = _bookingRepo.GetAllBookings();
            PassengerBookingUtilities.AttachBookings(passengers, bookings);
            return passengers;
        }

        public Passenger GetPassengerByIdWithBookings(int id)
        {
            var passenger = _passengerRepo.GetPassengerById(id);
            var bookings = _bookingRepo.GetAllBookings();

            PassengerBookingUtilities.AttachBookings(new List<Passenger> { passenger }, bookings);

            return passenger;
        }

        public void DeletePassengerWithBookings(int passengerId)
        {
            var passenger = _passengerRepo.GetPassengerById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            var allBookings = _bookingRepo.GetAllBookings();
            PassengerBookingUtilities.CascadeDeleteBookings(passengerId, allBookings, _bookingRepo.DeleteBooking);

            _passengerRepo.DeletePassenger(passengerId);
        }

        public void AddPassengerWithBookings(Passenger passenger)
        {
            if (passenger == null) throw new ArgumentNullException(nameof(passenger));
            _passengerRepo.AddPassenger(passenger);

            PassengerBookingUtilities.CascadeAddBookings(passenger, _bookingRepo.AddBooking);
        }

        public void AddPassenger(Passenger passenger)
        {
            if (passenger == null) throw new ArgumentNullException(nameof(passenger));
            _passengerRepo.AddPassenger(passenger);
        }

        public void UpdatePassenger(int id, Passenger newPassenger)
        {
            if (newPassenger == null) throw new ArgumentNullException(nameof(newPassenger));

            try
            {
                _passengerRepo.UpdatePassenger(p => p.Id == id, newPassenger);
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
                _passengerRepo.DeletePassenger(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Passenger with Id {id} does not exist");
            }
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengerRepo.GetAllPassengers();
        }

    }
}
