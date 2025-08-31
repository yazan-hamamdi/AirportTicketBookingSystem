using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Interfaces
{
    public interface IService<T> where T : class, IEntity
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(int id, T entity);
        void Delete(int id);
    }
}
