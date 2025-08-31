using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(int id, T entity);
        void Delete(int id);
        void Save(List<T> records);
    }
}
