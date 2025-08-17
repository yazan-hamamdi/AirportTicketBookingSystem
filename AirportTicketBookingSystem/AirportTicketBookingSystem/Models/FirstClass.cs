using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models
{
    public class FirstClass : ISeatClass
    {
        private const decimal multiplier = 4;
        public Class Name => Class.FirstClass;
        public decimal CalculatePrice(decimal basePrice) => basePrice * multiplier;
    }
}
