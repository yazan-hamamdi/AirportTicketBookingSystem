using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly string _filePath;

        public ManagerRepo(string filePath)
        {
            _filePath = filePath;
        }

        public Manager GetManagerById(int id)
        {
            var managerRecords = GetAllManagers();
            var selectedManager = managerRecords.FirstOrDefault(b => b.Id == id);

            return selectedManager ?? throw new KeyNotFoundException($"Manager with Id {id} was not found");  
        }

        public void DeleteManager(int id)
        {
            var managerRecords = GetAllManagers();
            var selectedManager = managerRecords.FirstOrDefault(b => b.Id == id);

            if (selectedManager == null)
                throw new KeyNotFoundException($"Manager with Id {id} was not found");

            managerRecords.Remove(selectedManager);

            Save(managerRecords);
        }

        public List<Manager> GetAllManagers()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var managers = csv.GetRecords<Manager>().ToList();

            return managers;
        }

        public void AddManager(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            var records = GetAllManagers();
            records.Add(manager);
            Save(records);
        }

        public void UpdateManager(Func<Manager, bool> predicate, Manager newManager)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newManager is null) throw new ArgumentNullException(nameof(newManager));

            var records = GetAllManagers();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new KeyNotFoundException("Manager not found to update");

            records[index] = newManager;
            Save(records);
        }

        private void Save(List<Manager> managers)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(managers);
        }

    }
}
