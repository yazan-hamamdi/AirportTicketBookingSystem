
using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Interfaces;

    public interface ISeatClass
    {
        TravelClass Name { get; }
        decimal CalculatePrice(decimal basePrice);
    }

