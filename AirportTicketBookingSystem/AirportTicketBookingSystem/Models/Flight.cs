using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

public class Flight : IEntity
{
    public int Id { get; set; }
    public int ManagerId { get; set; }
    public string DepartureCountry { get; set; }
    public string DestinationCountry { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTime DepartureDate { get; set; }
    public List<Booking> Bookings { get; set; } = new();

    public Flight() { }
   
    public Flight(int id, int managerId, string departureCountry, string destinationCountry, string departureAirport,
        string arrivalAirport, DateTime departureDate, List<Booking> bookings)
    {
        Id = id;
        ManagerId = managerId;
        DepartureCountry = departureCountry;
        DestinationCountry = destinationCountry;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        DepartureDate = departureDate;
        Bookings = bookings;
    }
}

