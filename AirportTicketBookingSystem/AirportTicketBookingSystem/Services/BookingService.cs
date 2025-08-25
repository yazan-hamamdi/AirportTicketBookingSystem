using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IPassengerRepository _passengerRepo;

        public BookingService(IBookingRepository bookingRepo, IPassengerRepository passengerRepo)
        {
            _bookingRepo = bookingRepo;
            _passengerRepo = passengerRepo;
        }

        public Booking GetBookingById(int id)
        {
            try
            {
                return _bookingRepo.GetBookingById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Booking with Id {id} does not exist");
            }
        }

        public void AddBooking(Booking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));
            _bookingRepo.AddBooking(booking);
        }

        public void UpdateBooking(int id, Booking newBooking)
        {
            if (newBooking == null) throw new ArgumentNullException(nameof(newBooking));

            try
            {
                _bookingRepo.UpdateBooking(id, newBooking);
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
                _bookingRepo.DeleteBooking(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Booking with Id {id} does not exist");
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepo.GetAllBookings();
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

            _bookingRepo.UpdateBooking(bookingId, newBooking);
        }

    }
}
