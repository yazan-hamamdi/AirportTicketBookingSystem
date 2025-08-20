using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepo _passengerRepo;

        public PassengerService(IPassengerRepo passengerRepo)
        {
            _passengerRepo = passengerRepo;
        }

        public Passenger GetPassengerById(int id)
        {
            try
            {
                return _passengerRepo.GetPassengerById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Passenger with Id {id} does not exist");
            }
        }

        public void AddPassenger(Passenger passenger)
        {
            if (passenger == null) throw new ArgumentNullException(nameof(passenger));
            _passengerRepo.AddPassenger(passenger);
        }

        public void UpdatePassenger(int id, Passenger newPassenger)
        {
            if (newPassenger == null) throw new ArgumentNullException(nameof(newPassenger));

            try
            {
                _passengerRepo.UpdatePassenger(p => p.Id == id, newPassenger);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. Passenger with Id {id} does not exist");
            }
        }

        public void DeletePassenger(int id)
        {
            try
            {
                _passengerRepo.DeletePassenger(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Passenger with Id {id} does not exist");
            }
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengerRepo.GetAllPassengers();
        }

    }
}
