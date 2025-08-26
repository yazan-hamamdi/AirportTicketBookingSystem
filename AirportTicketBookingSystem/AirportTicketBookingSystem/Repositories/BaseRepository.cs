using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly string _filePath;

        public BaseRepository(string filePath)
        {
            _filePath = filePath;
        }

        public virtual T GetById(int id)
        {
            var records = GetAll();
            var record = records.FirstOrDefault(r => r.Id == id);
            if (record == null)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");
            return record;
        }

        public virtual List<T> GetAll()
        {
            return CsvFileHelper.ReadFromCsv<T>(_filePath).ToList();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var records = GetAll();
            entity.Id = IdGenerator.GenerateNewId(records);

            records.Add(entity);
            Save(records);
        }

        public virtual void Update(int id, T entity)
        {
            var records = GetAll();
            var index = records.FindIndex(r => r.Id == id);
            if (index == -1)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");
            records[index] = entity;
            Save(records);
        }

        public virtual void Delete(int id)
        {
            var records = GetAll();
            var selectedRecord = records.FirstOrDefault(b => b.Id == id);

            if (selectedRecord == null)
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found");

            records.Remove(selectedRecord);
            Save(records);
        }

        public virtual void Save(List<T> records)
        {
            CsvFileHelper.WriteToCsv(_filePath, records);
        }
    }
}
