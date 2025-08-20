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
        private readonly FlightRepo _flightRepo;

        public ManagerRepo(string filePath, FlightRepo flightRepo)
        {
            _filePath = filePath;
            _flightRepo = flightRepo;
        }

        public Manager GetManagerById(int id)
        {
            var managerRecords = GetAll();
            var selectedManager = managerRecords.FirstOrDefault(b => b.Id == id);

            if (selectedManager != null)
            {
                return selectedManager;
            }
            throw new KeyNotFoundException($"Manager with Id {id} was not found");
        }

        public List<Manager> GetAll()
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
            var allFlights = _flightRepo.GetAll();

            var flightsByManager = allFlights
                .GroupBy(f => f.ManagerId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var manager in managers)
            {
                if (flightsByManager.TryGetValue(manager.Id, out var flights))
                {
                    manager.Flights = flights;
                }
            }

            return managers;
        }

        public void Add(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            var records = GetAll();
            records.Add(manager);
            Save(records);

            if (manager.Flights.Count > 0)
            {
                manager.Flights.ForEach(_flightRepo.Add);
            }
        }

        public void DeleteAll(Func<Manager, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var allManagers = GetAll();
            var managersToDelete = allManagers.Where(predicate).ToList();

            if (!managersToDelete.Any())
                throw new InvalidOperationException("No manager found to delete");

            allManagers.RemoveAll(m => predicate(m));
            Save(allManagers);

            var flightsToDelete = managersToDelete
                .SelectMany(m => m.Flights)
                .ToList();

            foreach (var flight in flightsToDelete)
            {
                _flightRepo.DeleteAll(f => f.Id == flight.Id);
            }
        }

        public void Update(Func<Manager, bool> predicate, Manager newManager)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newManager is null) throw new ArgumentNullException(nameof(newManager));

            var records = GetAll();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new InvalidOperationException("Manager not found to update");

            records[index] = newManager;
            Save(records);

            foreach (var flight in newManager.Flights)
            {
                _flightRepo.Update(f => f.Id == flight.Id, flight);
            }
        }

        private void Save(List<Manager> managers)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(managers);
        }

    }
}
