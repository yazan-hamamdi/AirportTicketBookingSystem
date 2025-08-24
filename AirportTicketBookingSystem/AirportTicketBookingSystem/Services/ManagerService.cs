using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepo;
        private readonly IBookingRepository _bookingRepo;

        public ManagerService(IManagerRepository managerRepo, IBookingRepository bookRepo)
        {
            _managerRepo = managerRepo;
            _bookingRepo = bookRepo;
        }

        public Manager GetManagerById(int id)
        {
            try
            {
                return _managerRepo.GetManagerById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Manager with Id {id} does not exist");
            }
        }

        public List<Manager> GetAllManagers()
        {
            return _managerRepo.GetAllManagers();
        }

        public void AddManager(Manager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            _managerRepo.AddManager(manager);
        }

        public void UpdateManager(int id, Manager updatedManager)
        {
            if (updatedManager == null) throw new ArgumentNullException(nameof(updatedManager));

            try
            {
                _managerRepo.UpdateManager(id, updatedManager);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. Manager with Id {id} does not exist");
            }
        }

        public void DeleteManager(int id)
        {
            try
            {
                _managerRepo.DeleteManager(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Manager with Id {id} does not exist");
            }
        }

    }
}
