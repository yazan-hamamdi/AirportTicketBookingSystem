using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class PassengerRepo : IPassengerRepo
    {
        private readonly string _filePath;

        public PassengerRepo(string filePath)
        {
            _filePath = filePath;
        }

        public Passenger GetById(int id)
        {
            var passengerRecords = GetAll();
            var selectedPassenger = passengerRecords.FirstOrDefault(b => b.Id == id);

           return selectedPassenger ?? throw new KeyNotFoundException($"Passenger with Id {id} was not found");      
        }

        public void DeleteById(int id)
        {
            var passengerRecords = GetAll();
            var selectedPassenger = passengerRecords.FirstOrDefault(b => b.Id == id);

            if (selectedPassenger == null)
                throw new KeyNotFoundException($"Passenger with Id {id} was not found");

            passengerRecords.Remove(selectedPassenger);

            Save(passengerRecords);
        }

        public List<Passenger> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var passengers = csv.GetRecords<Passenger>().ToList();

            return passengers;
        }

        public void Add(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException(nameof(passenger));

            var records = GetAll();
            records.Add(passenger);

            Save(records);
        }

        public void Update(Func<Passenger, bool> predicate, Passenger newPassenger)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newPassenger is null) throw new ArgumentNullException(nameof(newPassenger));

            var records = GetAll();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new KeyNotFoundException("Passenger not found to update");

            records[index] = newPassenger;
            Save(records);
        }

        private void Save(List<Passenger> passengers)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(passengers);
        }

    }
}
