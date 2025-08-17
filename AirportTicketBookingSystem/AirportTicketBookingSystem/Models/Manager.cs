
namespace AirportTicketBookingSystem.Models
{
    public class Manager
    {
        public string FullName { get; set; }
        public List<Flight> Flights { get; private set; } = new();

        public Manager(string fullName, List<Flight> flights)
        {
            FullName = fullName;
            Flights = flights;
        }

    }
}
