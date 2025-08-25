using AirportTicketBookingSystem.Enums;
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

        public List<Flight> GetAllFlightsWithBookings()
        {
            var flights = _flightRepository.GetAll();
            var bookings = _bookingRepository.GetAll();
            FlightBookingUtilities.AttachBookings(flights, bookings);
            return flights;
        }

        public Flight GetFlightByIdWithBookings(int id)
        {
            var flight = _flightRepository.GetById(id);
            var bookings = _bookingRepository.GetAll();

            FlightBookingUtilities.AttachBookings(new List<Flight> { flight }, bookings);

            return flight;
        }

        public void DeleteFlightWithBookings(int flightId)
        {
            var flight = _flightRepository.GetById(flightId);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with Id {flightId} does not exist");

            _bookingRepository.DeleteBookings(b => b.FlightId == flightId);
            _flightRepository.Delete(flightId);
        }

        public void AddFlightWithBookings(Flight newFlight, List<Booking> bookings)
        {
            _flightRepository.Add(newFlight);

            if (bookings == null || bookings.Count == 0)
                throw new ArgumentException("Bookings list cannot be null or empty when adding a flight with bookings");

            foreach (var booking in bookings)
            {
                booking.FlightId = newFlight.Id; 
               _bookingRepository.Add(booking); 
            }
        }

        public List<FieldValidationDetail> GetFlightModelValidationDetails()
        {
            var details = new List<FieldValidationDetail>();
            var properties = typeof(Flight).GetProperties();

            foreach (var prop in properties)
            {
                var fieldDetail = new FieldValidationDetail
                {
                    FieldName = prop.Name,
                    FieldType = prop.PropertyType.Name,
                    Constraints = ValidationMetadataHelper.GetPropertyConstraints(prop)
                };
                details.Add(fieldDetail);
            }
            return details;
        }

        public List<Flight> SearchAvailableFlights(string departureCountry = null, string destinationCountry = null,
            string departureAirport = null, string arrivalAirport = null, DateTime? departureDateFrom = null,
            DateTime? departureDateTo = null, TravelClass? seatClass = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allFlights = _flightRepository.GetAll();
            var allBookings = _bookingRepository.GetAll();

            FlightBookingUtilities.AttachBookings(allFlights, allBookings);

            var query =
            from f in allFlights
            where (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase))
            where (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
            where (!departureDateFrom.HasValue || f.DepartureDate >= departureDateFrom.Value)
            where (!departureDateTo.HasValue || f.DepartureDate <= departureDateTo.Value)
            where (!seatClass.HasValue || f.Bookings.Any(b => b.SeatClass.Name == seatClass.Value))
            where (!minPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() >= minPrice.Value))
            where (!maxPrice.HasValue || f.Bookings.Any(b => b.SeatClass.CalculatePrice() <= maxPrice.Value))
            select f;

            return query.ToList();
        }

        public List<string> ImportFlightsFromCsv(string csvFilePath)
        {
            var importedFlights = _flightRepository.GetAllFromFile(csvFilePath);

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
            var errors = new List<string>();

            if (IsDuplicateFlight(flight, existingFlights))
            {
                errors.Add($"Duplicate flight: {flight.DepartureCountry} -> {flight.DestinationCountry} on {flight.DepartureDate}");
            }

            var validationErrors = ValidationHelper.ValidateModel(flight);
            foreach (var err in validationErrors)
            {
                errors.Add($"Flight {flight.DepartureCountry} -> {flight.DestinationCountry} on {flight.DepartureDate}: {err}");
            }
            return errors;
        }

    }
}
