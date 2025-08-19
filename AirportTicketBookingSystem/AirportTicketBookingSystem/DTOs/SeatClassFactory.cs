using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.DTOs
{
    public static class SeatClassFactory
    {
        public static ISeatClass FromString(string seatClass)
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
