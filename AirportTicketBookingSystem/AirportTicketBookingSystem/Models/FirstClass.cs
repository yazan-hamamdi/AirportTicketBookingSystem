using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class FirstClass : ISeatClass
    {
        private const decimal priceFactor = 4;
        public TravelClass Name => TravelClass.FirstClass;
        public decimal BasePrice { get; private set; }

        public FirstClass() { }

        public FirstClass(decimal basePrice)
        {
          BasePrice = basePrice;
        }

        public decimal CalculatePrice() => BasePrice * priceFactor;
    }

