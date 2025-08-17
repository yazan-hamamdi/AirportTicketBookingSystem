using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models
{
    public class BusinessClass : ISeatClass
    {
        private const decimal multiplier = 2;
        public Class Name => Class.Business;
        public decimal CalculatePrice(decimal basePrice) => basePrice * multiplier;
    }
}
