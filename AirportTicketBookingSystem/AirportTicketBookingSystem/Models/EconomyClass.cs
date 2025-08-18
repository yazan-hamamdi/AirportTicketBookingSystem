using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class EconomyClass : ISeatClass
    {
        public TravelClass Name => TravelClass.Economy;

        public decimal CalculatePrice(decimal basePrice) => basePrice;
    }

