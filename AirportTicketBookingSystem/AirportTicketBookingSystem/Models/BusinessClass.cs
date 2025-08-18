using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class BusinessClass : ISeatClass
    {
        private const decimal priceFactor = 2;
        public TravelClass Name => TravelClass.Business;

        public decimal CalculatePrice(decimal basePrice) => basePrice * priceFactor;
    }

