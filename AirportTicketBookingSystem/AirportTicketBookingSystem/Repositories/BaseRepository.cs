using AirportTicketBookingSystem.Adapters;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly string _filePath;
        protected readonly ICsvFileHelperAdapter _csvHelper;

        public BaseRepository(string filePath, ICsvFileHelperAdapter csvHelper)
        {
            _filePath = filePath;
            _csvHelper = csvHelper;
        }

        public virtual T GetById(int id)
        {
            var records = _csvHelper.ReadFromCsv<T>(_filePath).ToList();
            var record = records.FirstOrDefault(r => r.Id == id);

            if (record == null)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");

            return record;
        }

        public virtual List<T> GetAll()
        {
            return _csvHelper.ReadFromCsv<T>(_filePath).ToList();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var records = _csvHelper.ReadFromCsv<T>(_filePath).ToList();
            entity.Id = IdGenerator.GenerateNewId(records);

            records.Add(entity);
            Save(records);
        }

        public virtual void Update(int id, T entity)
        {
            var records = _csvHelper.ReadFromCsv<T>(_filePath).ToList();
            var index = records.FindIndex(r => r.Id == id);

            if (index == -1)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");

            records[index] = entity;
            Save(records);
        }

        public virtual void Delete(int id)
        {
            var records = _csvHelper.ReadFromCsv<T>(_filePath).ToList();
            var selectedRecord = records.FirstOrDefault(b => b.Id == id);

            if (selectedRecord == null)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");

            records.Remove(selectedRecord);
            Save(records);
        }

        public virtual void Save(List<T> records)
        {
            _csvHelper.WriteToCsv(_filePath, records);
        }

        public virtual List<TModel> GetAll<TModel>() where TModel : class, new()
        {
            return _csvHelper.ReadFromCsv<TModel>(_filePath).ToList();
        }
    }
}
