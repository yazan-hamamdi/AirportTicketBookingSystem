using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepo _passengerRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly IFlightRepo _flightRepo;
        public PassengerService(IPassengerRepo passengerRepo, IBookingRepo bookRepo, IFlightRepo flightRepo)
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
            PassengerBookingUtility.AttachBookings(passengers, bookings);
            return passengers;
        }

        public Passenger GetPassengerByIdWithBookings(int id)
        {
            var passenger = _passengerRepo.GetPassengerById(id);
            var bookings = _bookingRepo.GetAllBookings();

            PassengerBookingUtility.AttachBookings(new List<Passenger> { passenger }, bookings);

            return passenger;
        }

        public void DeletePassengerWithBookings(int passengerId)
        {
            var passenger = _passengerRepo.GetPassengerById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            var allBookings = _bookingRepo.GetAllBookings();
            PassengerBookingUtility.CascadeDeleteBookings(passengerId, allBookings, _bookingRepo.DeleteBooking);

            _passengerRepo.DeletePassenger(passengerId);
        }

        public void AddPassengerWithBookings(Passenger passenger)
        {
            if (passenger == null) throw new ArgumentNullException(nameof(passenger));
            _passengerRepo.AddPassenger(passenger);

            PassengerBookingUtility.CascadeAddBookings(passenger, _bookingRepo.AddBooking);
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

        public List<Flight> SearchAvailableFlights( string departureCountry = null, string destinationCountry = null,
         string departureAirport = null, string arrivalAirport = null, DateTime? departureDateFrom = null,
         DateTime? departureDateTo = null, TravelClass? seatClass = null, decimal? minPrice = null,decimal? maxPrice = null)
        {
            var allFlights = _flightRepo.GetAllFlights();
            var allBookings = _bookingRepo.GetAllBookings();

            FlightBookingUtility.AttachBookings(allFlights, allBookings);

            var query =
        from f in allFlights
        where (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
        where (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase))
        where (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase))
        where (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
        where (!departureDateFrom.HasValue || f.DepartureDate >= departureDateFrom.Value)
        where (!departureDateTo.HasValue || f.DepartureDate <= departureDateTo.Value)
        where (!seatClass.HasValue || f.Bookings.Any(b => b.SeatClass.Name == seatClass.Value))
        where (!minPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() >= minPrice.Value))
        where (!maxPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() <= maxPrice.Value))
        select f;

            return query.ToList();
        }

        public List<Booking> GetBookingsForPassenger(int passengerId)
        {
            var passenger = _passengerRepo.GetPassengerById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            var allBookings = _bookingRepo.GetAllBookings();
            return allBookings.Where(b => b.PassengerId == passengerId).ToList();
        }

        public void CancelBooking(int passengerId, int bookingId)
        {
            var booking = _bookingRepo.GetBookingById(bookingId);
            if (booking == null || booking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepo.DeleteBooking(bookingId);
        }

        public void ModifyBooking(int passengerId, int bookingId, Booking newBooking)
        {
            var PassengerBooking = _bookingRepo.GetBookingById(bookingId);
            if (PassengerBooking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepo.UpdateBooking(b => b.Id == bookingId, newBooking);
        }

    }
}
