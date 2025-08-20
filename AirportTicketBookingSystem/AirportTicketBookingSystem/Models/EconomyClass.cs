using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class EconomyClass : ISeatClass
    {
        public TravelClass Name => TravelClass.Economy;
        public decimal BasePrice { get; private set; }
    
        public EconomyClass() { }

        public EconomyClass(decimal basePrice)
        {
          BasePrice = basePrice;
        }

        public decimal CalculatePrice() => BasePrice;

    }

