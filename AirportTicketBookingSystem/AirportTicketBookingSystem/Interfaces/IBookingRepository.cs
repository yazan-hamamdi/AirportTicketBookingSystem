using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        List<Booking> FilterBookingsForManager(
           int? flightId = null,
           int? passengerId = null,
           DateTime? bookingDateFrom = null,
           DateTime? bookingDateTo = null,
           TravelClass? seatClass = null,
           decimal? minPrice = null,
           decimal? maxPrice = null 
        );
    }
}
