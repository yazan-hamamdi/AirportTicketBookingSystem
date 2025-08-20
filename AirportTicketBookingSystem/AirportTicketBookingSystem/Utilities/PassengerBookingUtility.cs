using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Utilities
{
    public static class PassengerBookingUtility
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

        public static void CascadeDeleteBookings(int passengerId, List<Booking> bookings, Action<int> deleteBooking)
        {
            var relatedBookings = bookings
                .Where(b => b.PassengerId == passengerId)
                .Select(b => b.Id)
                .ToList();

            foreach (var bookingId in relatedBookings)
            {
                deleteBooking(bookingId);
            }
        }

        public static void CascadeAddBookings(Passenger passenger, Action<Booking> addBooking)
        {
            if (passenger.Bookings == null || !passenger.Bookings.Any())
                return;

            foreach (var booking in passenger.Bookings)
            {
                booking.PassengerId = passenger.Id;
                addBooking(booking);
            }
        }

    }
}
