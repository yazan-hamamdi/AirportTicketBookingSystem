using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IManagerRepository
    {
        public Manager GetManagerById(int id);
        List<Manager> GetAllManagers();
        void AddManager(Manager manager);
        void UpdateManager(Func<Manager, bool> predicate, Manager newManager);
        void DeleteManager(int id);
    }
}
