using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Adapters
{
    public interface ISeatClassAdapter
    {
        ISeatClass ToSeatClass(string seatClass);
    }
}
