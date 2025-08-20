using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class BusinessClass : ISeatClass
    {
        private const decimal priceFactor = 2;
        public TravelClass Name => TravelClass.Business;
        public decimal BasePrice { get; private set; }

        public BusinessClass(decimal basePrice)
        {
          BasePrice = basePrice;
        }

        public decimal CalculatePrice() => BasePrice * priceFactor;
    }

