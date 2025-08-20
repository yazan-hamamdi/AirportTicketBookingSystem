
using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Interfaces;

    public interface ISeatClass
    {
        TravelClass Name { get; }
        decimal BasePrice { get; }

        decimal CalculatePrice();
    }

