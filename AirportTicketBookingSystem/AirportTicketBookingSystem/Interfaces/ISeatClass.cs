
using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface ISeatClass
    {
        Class Name { get; }
        decimal CalculatePrice(decimal basePrice);
    }
}
