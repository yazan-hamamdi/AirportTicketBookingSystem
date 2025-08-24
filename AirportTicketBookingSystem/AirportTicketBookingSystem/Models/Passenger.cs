using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

public class Passenger : IEntity
{ 
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public List<Booking> Bookings { get; set; } = new();

    public Passenger() { }

    public Passenger(int id, string fullName, string passportNumber, List<Booking> bookings)
    {
        Id = id;
        FullName = fullName;
        PassportNumber = passportNumber;
        Bookings = bookings;
    }
}

