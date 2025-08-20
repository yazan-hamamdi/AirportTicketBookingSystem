
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Services
{
    public class BookingService
    {
        private readonly IBookingRepo _bookingRepo;

        public BookingService(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

    }
}
