using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IManagerRepo
    {
        public Manager GetManagerById(int id);
        List<Manager> GetAll();
        void Add(Manager manager);
        void Update(Func<Manager, bool> predicate, Manager newManager);
        void DeleteAll(Func<Manager, bool> predicate);
    }

}
