
namespace AirportTicketBookingSystem.Models;

    public class Manager
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Flight> Flights { get; private set; } = new();

        public Manager(int id, string fullName, List<Flight> flights)
        {
            Id = id;
            FullName = fullName;
            Flights = flights;
        }

    }

