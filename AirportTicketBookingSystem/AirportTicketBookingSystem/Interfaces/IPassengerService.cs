using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IPassengerService : IService<Passenger>
    {
        void AddPassengerWithBookings(Passenger newPassenger, List<Booking> bookings);
        List<Passenger> GetAllPassengersWithBookings();
        Passenger GetPassengerByIdWithBookings(int id);
        void DeletePassengerWithBookings(int passengerId);
    }
}
