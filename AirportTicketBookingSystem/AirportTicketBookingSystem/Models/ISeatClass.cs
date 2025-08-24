using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Models;

public interface ISeatClass
{
    TravelClass Name { get; }
    decimal BasePrice { get; }

    decimal CalculatePrice();
}

