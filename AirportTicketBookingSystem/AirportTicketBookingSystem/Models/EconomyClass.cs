using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models
{
    public class EconomyClass : ISeatClass
    {
        public Class Name => Class.Economy;
        public decimal CalculatePrice(decimal basePrice) => basePrice;
    }
}
