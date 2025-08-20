using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepo _managerRepo;

        public ManagerService(IManagerRepo managerRepo)
        {
            _managerRepo = managerRepo;
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
                _managerRepo.UpdateManager(m => m.Id == id, updatedManager);
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
