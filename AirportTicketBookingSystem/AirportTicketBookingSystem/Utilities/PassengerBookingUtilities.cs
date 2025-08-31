using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Utilities
{
    public static class PassengerBookingUtilities
    {
        public static void AttachBookings(List<Passenger> passengers, List<Booking> bookings)
        {
            var bookingsByPassenger = bookings
                .GroupBy(b => b.PassengerId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var passenger in passengers)
            {
                passenger.Bookings = bookingsByPassenger.TryGetValue(passenger.Id, out var list)
                    ? list
                    : new List<Booking>();
            }
        }
    }
}
