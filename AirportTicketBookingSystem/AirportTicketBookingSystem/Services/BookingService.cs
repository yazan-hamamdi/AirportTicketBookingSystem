using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;

namespace AirportTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPassengerRepository _passengerRepository;

        public BookingService(IBookingRepository bookingRepo, IPassengerRepository passengerRepo)
        {
            _bookingRepository = bookingRepo;
            _passengerRepository = passengerRepo;
        }

        public Booking GetBookingById(int id)
        {
            try
            {
                return _bookingRepository.GetBookingById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Booking with Id {id} does not exist");
            }
        }

        public void AddBooking(Booking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));
            _bookingRepository.AddBooking(booking);
        }

        public void UpdateBooking(int id, Booking newBooking)
        {
            if (newBooking == null) throw new ArgumentNullException(nameof(newBooking));

            try
            {
                _bookingRepository.UpdateBooking(id, newBooking);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. Booking with Id {id} does not exist");
            }
        }

        public void DeleteBooking(int id)
        {
            try
            {
                _bookingRepository.DeleteBooking(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Booking with Id {id} does not exist");
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public List<Booking> GetBookingsForPassenger(int passengerId)
        {
            var passenger = _passengerRepository.GetPassengerById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            var allBookings = _bookingRepository.GetAllBookings();
            return allBookings.Where(b => b.PassengerId == passengerId).ToList();
        }

        public void CancelBooking(int passengerId, int bookingId)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null || booking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepository.DeleteBooking(bookingId);
        }

        public void UpdateBooking(int passengerId, int bookingId, Booking newBooking)
        {
            var PassengerBooking = _bookingRepository.GetBookingById(bookingId);
            if (PassengerBooking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepository.UpdateBooking(bookingId, newBooking);
        }

        public List<Booking> FilterBookingsForManager(int? flightId = null, int? passengerId = null,
            DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null,
            TravelClass? seatClass = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            return _bookingRepository.FilterBookingsForManager(
                flightId,
                passengerId, 
                bookingDateFrom, 
                bookingDateTo, 
                seatClass, 
                minPrice, 
                maxPrice
            );
        }

    }
}
