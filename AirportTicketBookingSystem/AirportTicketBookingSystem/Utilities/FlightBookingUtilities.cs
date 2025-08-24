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

        public static void CascadeDeleteBookings(int flightId, List<Booking> bookings, Action<int> deleteBooking)
        {
            var relatedBookings = bookings
                .Where(b => b.FlightId == flightId)
                .Select(b => b.Id)
                .ToList();

            foreach (var bookingId in relatedBookings)
            {
                deleteBooking(bookingId);
            }
        }
        public static void CascadeAddBookings(Flight flight, Action<Booking> addBooking)
        {
            if (flight.Bookings == null || !flight.Bookings.Any())
                return;

            foreach (var booking in flight.Bookings)
            {
                booking.FlightId = flight.Id;
                addBooking(booking);
            }
        }
    }
}
