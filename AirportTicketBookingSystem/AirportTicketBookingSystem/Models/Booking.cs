using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models;

    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public DateTime BookingDate { get; set; }
        public ISeatClass SeatClass { get; set; }
        public decimal Price => SeatClass.CalculatePrice();
        
        public Booking() { }
        
        public Booking(int id,int flightId, int passengerId , DateTime bookingDate, 
           string seatNumber, ISeatClass seatClass)
        {
           Id = id;
           FlightId = flightId;
           PassengerId = passengerId;
           BookingDate = bookingDate;
           SeatClass = seatClass;
        }

    }

