using AirportTicketBookingSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingSystem.Models;

public class Flight : IEntity
{
    public int Id { get; set; }
    public int ManagerId { get; set; }

    [Required(ErrorMessage = "Departure country is required")]
    public string DepartureCountry { get; set; }

    [Required(ErrorMessage = "Destination country is required")]
    public string DestinationCountry { get; set; }

    [Required(ErrorMessage = "Departure airport is required")]
    public string DepartureAirport { get; set; }

    [Required(ErrorMessage = "Arrival airport is required")]
    public string ArrivalAirport { get; set; }

    [Range(typeof(DateTime), "1/1/2023", "12/31/2030", ErrorMessage = "Departure date outside the allowed range")]
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

