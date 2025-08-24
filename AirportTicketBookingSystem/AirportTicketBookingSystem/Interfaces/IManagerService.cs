using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IManagerService
    {
        Manager GetManagerById(int id);
        List<Manager> GetAllManagers();
        void AddManager(Manager manager);
        void UpdateManager(int id, Manager updatedManager);
        void DeleteManager(int id);
    }

}
