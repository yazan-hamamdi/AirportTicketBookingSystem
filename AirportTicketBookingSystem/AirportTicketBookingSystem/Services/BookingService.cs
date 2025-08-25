using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class BookingService : BaseService<Booking>, IBookingService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepo, IPassengerRepository passengerRepo) : base(bookingRepo)
        {
            _passengerRepository = passengerRepo;
            _bookingRepository = bookingRepo;
        }

        public List<Booking> GetBookingsForPassenger(int passengerId)
        {
            var passenger = _passengerRepository.GetById(passengerId);
            if (passenger == null)
                throw new KeyNotFoundException($"Passenger with Id {passengerId} does not exist");

            var allBookings = _bookingRepository.GetAll();
            return allBookings.Where(b => b.PassengerId == passengerId).ToList();
        }

        public void CancelBooking(int passengerId, int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null || booking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepository.Delete(bookingId);
        }

        public void UpdateBooking(int passengerId, int bookingId, Booking newBooking)
        {
            var PassengerBooking = _bookingRepository.GetById(bookingId);
            if (PassengerBooking.PassengerId != passengerId)
                throw new KeyNotFoundException("Booking not found for this passenger");

            _bookingRepository.Update(bookingId, newBooking);
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
