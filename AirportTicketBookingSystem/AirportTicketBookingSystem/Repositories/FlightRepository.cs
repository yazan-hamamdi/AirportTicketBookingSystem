using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepository : CsvRepositoryBase<Flight>, IFlightRepository
    {
        public FlightRepository(string filePath) : base(filePath) { }

        public List<Flight> GetAllFromFile(string filePath)
        {

            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV file not found", filePath);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var flights = csv.GetRecords<Flight>().ToList();

            return flights;
        }
    }
}
