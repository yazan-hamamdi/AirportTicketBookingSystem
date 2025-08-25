namespace AirportTicketBookingSystem.Models;

public class Booking : IEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int PassengerId { get; set; }
    public DateTime BookingDate { get; set; }
    public ISeatClass SeatClass { get; set; }
    public decimal Price { get; }
    
    public Booking() { }
    
    public Booking(int id,int flightId, int passengerId , DateTime bookingDate, 
       string seatNumber, ISeatClass seatClass, decimal price)
    {
       Id = id;
       FlightId = flightId;
       PassengerId = passengerId;
       BookingDate = bookingDate;
       SeatClass = seatClass;
       Price = price;
    }

}

