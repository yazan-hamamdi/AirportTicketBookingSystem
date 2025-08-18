
namespace AirportTicketBookingSystem.Models;

    public class Passenger
    {
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public List<Booking> Bookings { get; private set; } = new();

        public Passenger(string fullName, string passportNumber, List<Booking> booking)
        {
            FullName = fullName;
            PassportNumber = passportNumber;
            Bookings = booking;
        }

    }


