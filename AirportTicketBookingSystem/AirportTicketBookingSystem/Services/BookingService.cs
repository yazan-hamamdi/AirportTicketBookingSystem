using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;

        public BookingService(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
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
                _bookingRepo.UpdateBooking(b => b.Id == id, newBooking);
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

    }
}
