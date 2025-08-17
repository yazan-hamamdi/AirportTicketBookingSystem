using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Models
{
    public class Booking
    {
        public Flight Flight { get; set; }
        public Passenger Passenger { get; set; }
        public DateTime BookingDate { get; set; }
        public ISeatClass SeatClass { get; set; }
        public decimal Price { get; }

        public Booking(Flight flight, Passenger passenger, DateTime bookingDate, 
            string seatNumber, ISeatClass seatClass, decimal price)
        {
            Flight = flight;
            Passenger = passenger;
            BookingDate = bookingDate;
            SeatClass = seatClass;
            Price = price;
        }

    }
}
