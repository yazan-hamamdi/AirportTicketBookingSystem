
using CsvHelper;
using System.Globalization;

namespace AirportTicketBookingSystem.Repositories
{
    public class CsvRepository<T> where T : class
    {
        private readonly string _filePath;

        public CsvRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }

        public void Delete(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var records = GetAll();
            var removedCount = records.RemoveAll(r => predicate(r));

            if (removedCount == 0)
                throw new InvalidOperationException("No record found to delete");

            Save(records);
        }
        
        public void Update(Func<T, bool> predicate, T newRecord)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newRecord is null) throw new ArgumentNullException(nameof(newRecord));

            var records = GetAll();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new InvalidOperationException("Record to update not found");

            records[index] = newRecord;
            Save(records);

        }

        public void Add(T record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            var records = GetAll();
            records.Add(record);
            Save(records);
        }

        private void Save(List<T> records)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records));

            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

    }
}
