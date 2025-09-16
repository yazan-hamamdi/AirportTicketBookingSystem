using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public abstract class BaseService<T> : IService<T> where T : class, IEntity
    {
        protected readonly IRepository<T> _repository;

        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T GetById(int id)
        {
            try
            {
                return _repository.GetById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} does not exist");
            }
            Console.WriteLine("sssss");
        }

        public virtual List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _repository.Add(entity);
        }

        public virtual void Update(int id, T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _repository.Update(id, entity);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. {typeof(T).Name} with Id {id} does not exist");
            }
            Console.WriteLine("sssss");
        }

        public virtual void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. {typeof(T).Name} with Id {id} does not exist");
            }
        }
    }

}
