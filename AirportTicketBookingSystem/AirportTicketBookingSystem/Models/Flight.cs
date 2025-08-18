
namespace AirportTicketBookingSystem.Models;

    public class Flight
    {
        public int Id { get; set; }
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<Booking> Bookings { get; private set; } = new();

        public Flight(int id, string departureCountry, string destinationCountry, string departureAirport,
            string arrivalAirport, DateTime departureDate, List<Booking> bookings)
        {
            Id = id;
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            DepartureDate = departureDate;
            Bookings = bookings;
        }

    }

