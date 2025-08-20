using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IRepositories
{
    public interface IManagerRepo
    {
        public Manager GetById(int id);
        void DeleteById(int id);
        List<Manager> GetAll();
        void Add(Manager manager);
        void Update(Func<Manager, bool> predicate, Manager newManager);
        void DeleteWhere(Func<Manager, bool> predicate);
    }

}
