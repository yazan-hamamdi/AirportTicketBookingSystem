using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Adapters
{
    public class SeatClassExtensionsAdapter : ISeatClassAdapter
    {
        public ISeatClass ToSeatClass( string seatClass)
        {
            return SeatClassExtensions.ToSeatClass(seatClass);
        }
    }
}