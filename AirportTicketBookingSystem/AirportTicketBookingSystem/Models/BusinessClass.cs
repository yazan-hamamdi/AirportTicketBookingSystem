using AirportTicketBookingSystem.Enums;

namespace AirportTicketBookingSystem.Models;

public class BusinessClass : ISeatClass
{
    private const decimal PriceFactor = 2;
    public TravelClass Name => TravelClass.Business;
    public decimal BasePrice { get; private set; }
    
    public BusinessClass () { }

    public BusinessClass(decimal basePrice)
    {
      BasePrice = basePrice;
    }

    public decimal CalculatePrice() => BasePrice * PriceFactor;
}
