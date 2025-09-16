using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Services
{
    public class FlightService : BaseService<Flight>, IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingRepository _bookingRepository;

        public FlightService(IFlightRepository flightRepository, IBookingRepository bookingRepository)
            : base(flightRepository)
        {
            _flightRepository = flightRepository; 
            _bookingRepository = bookingRepository;
        }

        public List<FieldValidationDetail> GetFlightModelValidationDetails()
        {
            return typeof(Flight)
                .GetProperties()
                .Select(prop => new FieldValidationDetail
                {
                    FieldName = prop.Name,
                    FieldType = prop.PropertyType.Name,
                    Constraints = ValidationMetadataHelper.GetPropertyConstraints(prop)
                })
                .ToList();
        }

        public List<string> ImportFlightsFromCsv(string csvFilePath)
        {
            var importedFlights = CsvFileHelper.ReadFromCsv<Flight>(csvFilePath);

            if (!importedFlights.Any())
                throw new ArgumentException("No flights found in the CSV file");

            var existingFlights = GetAll();
            var errors = new List<string>();

            foreach (var flight in importedFlights)
            {
                var flightErrors = ValidateFlightForImport(flight, existingFlights);

                if (flightErrors.Any())
                {
                    errors.AddRange(flightErrors);
                    continue;
                }
                flight.Id = IdGenerator.GenerateNewId(existingFlights);
                existingFlights.Add(flight);
            }
            _flightRepository.Save(existingFlights);

            return errors;
        }

        public bool IsDuplicateFlight(Flight flight, List<Flight> existingFlights)
        {
            return existingFlights.Any(f =>
                f.DepartureCountry == flight.DepartureCountry &&
                f.DestinationCountry == flight.DestinationCountry &&
                f.DepartureAirport == flight.DepartureAirport &&
                f.ArrivalAirport == flight.ArrivalAirport &&
                f.DepartureDate == flight.DepartureDate
            );
        }

        private List<string> ValidateFlightForImport(Flight flight, List<Flight> existingFlights)
        {
            var flightIdentifier = $"{flight.DepartureCountry} -> {flight.DestinationCountry} on {flight.DepartureDate}";

            var duplicateError = IsDuplicateFlight(flight, existingFlights)
                ? new[] { $"Duplicate flight: {flightIdentifier}" }
                : Enumerable.Empty<string>();

            var validationErrors = ValidationHelper.ValidateModel(flight)
                .Select(err => $"Flight {flightIdentifier}: {err}");

            return duplicateError.Concat(validationErrors).ToList();
        }
    }
}
