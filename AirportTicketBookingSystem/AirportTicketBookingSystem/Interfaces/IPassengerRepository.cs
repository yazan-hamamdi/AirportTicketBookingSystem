using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IPassengerRepository
    {
        public Passenger GetPassengerById(int id);
        List<Passenger> GetAllPassengers();
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(Func<Passenger, bool> predicate, Passenger newPassenger);
        void DeletePassenger(int id);
    }
}
