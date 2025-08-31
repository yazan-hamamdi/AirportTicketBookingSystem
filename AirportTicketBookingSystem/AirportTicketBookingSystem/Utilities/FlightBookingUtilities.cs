using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Utilities
{
    public static class FlightBookingUtilities
    {
        public static void AttachBookings(List<Flight> flights, List<Booking> bookings)
        {
            var bookingsByFlight = bookings
                .GroupBy(b => b.FlightId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var flight in flights)
            {
                flight.Bookings = bookingsByFlight.TryGetValue(flight.Id, out var list)
                    ? list
                    : new List<Booking>();
            }
        }
    }
}
