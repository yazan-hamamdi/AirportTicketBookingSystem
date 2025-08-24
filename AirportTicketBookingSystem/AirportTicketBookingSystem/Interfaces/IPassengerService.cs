using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IPassengerService
    {
        Passenger GetPassengerById(int id);
        List<Passenger> GetAllPassengersWithBookings();
        Passenger GetPassengerByIdWithBookings(int id);
        void DeletePassengerWithBookings(int passengerId);
        void AddPassengerWithBookings(Passenger passenger);
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(int id, Passenger newPassenger);
        void DeletePassenger(int id);
        List<Passenger> GetAllPassengers();
    }

}
