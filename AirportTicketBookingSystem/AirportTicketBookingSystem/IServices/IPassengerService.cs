using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IPassengerService
    {
        Passenger GetPassengerById(int id);
        List<Passenger> GetAllPassengers();
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(int id, Passenger updatedPassenger);
        void DeletePassenger(int id);
    }

}
