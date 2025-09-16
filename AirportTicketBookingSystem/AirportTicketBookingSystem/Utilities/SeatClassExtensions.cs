using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Utilities
{
    public static class SeatClassExtensions
    {
        public static ISeatClass ToSeatClass(this string seatClass)
        {
            return seatClass.ToLower() switch
            {
                "economy" => new EconomyClass(),
                "business" => new BusinessClass(),
                "first" => new FirstClass(),
                _ => new EconomyClass()
            };
        }
    }
}