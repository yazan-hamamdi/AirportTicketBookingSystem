using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.IRepositories;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepo : IFlightRepo
    {
        private readonly string _filePath;

        public FlightRepo(string filePath)
        {
            _filePath = filePath;
        }

        public Flight GetFlightById(int id)
        {
            var flightRecords = GetAllFlights();
            var selectedFlight = flightRecords.FirstOrDefault(b => b.Id == id);

            return selectedFlight ?? throw new KeyNotFoundException($"Flight with Id {id} was not found");
            
        }

        public void DeleteFlight(int id)
        {
            var flightRecords = GetAllFlights();
            var selectedFlight = flightRecords.FirstOrDefault(b => b.Id == id);

            if (selectedFlight == null)
                throw new KeyNotFoundException($"Flight with Id {id} was not found");

            flightRecords.Remove(selectedFlight);

            Save(flightRecords);
        }

        public List<Flight> GetAllFlights()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("CSV file not found", _filePath);

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var flights = csv.GetRecords<Flight>().ToList();

            return flights;
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null)
                throw new ArgumentNullException(nameof(flight));

            var records = GetAllFlights();
            records.Add(flight);

            Save(records);
        }

        public void UpdateFlight(Func<Flight, bool> predicate, Flight newFlight)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (newFlight is null) throw new ArgumentNullException(nameof(newFlight));

            var records = GetAllFlights();
            var index = records.FindIndex(r => predicate(r));

            if (index < 0)
                throw new KeyNotFoundException("Flight not found to update");

            records[index] = newFlight;
            Save(records);
        }

        private void Save(List<Flight> flights)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(flights);
        }

    }
}
