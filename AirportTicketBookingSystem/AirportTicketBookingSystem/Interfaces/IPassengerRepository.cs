using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IPassengerRepository
    {
        public Passenger GetPassengerById(int id);
        List<Passenger> GetAllPassengers();
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(int passengerId, Passenger newPassenger);
        void DeletePassenger(int id);
    }
}
