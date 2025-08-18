
namespace AirportTicketBookingSystem.Models;

    public class Passenger
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public List<Booking> Bookings { get; private set; } = new();

        public Passenger(int id, string fullName, string passportNumber, List<Booking> booking)
        {
            Id = id;
            FullName = fullName;
            PassportNumber = passportNumber;
            Bookings = booking;
        }

    }


