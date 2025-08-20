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

        public Passenger GetPassengerById(int id)
        {
            var passengerRecords = GetAllPassengers();
            var selectedPassenger = passengerRecords.FirstOrDefault(b => b.Id == id);

           return selectedPassenger ?? throw new KeyNotFoundException($"Passenger with Id {id} was not found");      
        }

        public void DeletePassenger(int id)
        {
            var passengerRecords = GetAllPassengers();
            var selectedPassenger = passengerRecords.FirstOrDefault(b => b.Id == id);

            if (selectedPassenger == null)
                throw new KeyNotFoundException($"Passenger with Id {id} was not found");

            passengerRecords.Remove(selectedPassenger);

            Save(passengerRecords);
        }

        public List<Passenger> GetAllPassengers()
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

        public void AddPassenger(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException(nameof(passenger));

            var records = GetAllPassengers();
            records.Add(passenger);

            Save(records);
        }

        public void UpdatePassenger(Func<Passenger, bool> predicate, Passenger newPassenger)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newPassenger is null) throw new ArgumentNullException(nameof(newPassenger));

            var records = GetAllPassengers();
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
