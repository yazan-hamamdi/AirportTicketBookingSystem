using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        private readonly IBookingRepository _bookingRepository;

        public FlightRepository(string filePath, IBookingRepository bookingRepository)
            : base(filePath)
        {
            _bookingRepository = bookingRepository;
        }

        public List<Flight> SearchAvailableFlights(
            string departureCountry = null,
            string destinationCountry = null,
            string departureAirport = null,
            string arrivalAirport = null,
            DateTime? departureDateFrom = null,
            DateTime? departureDateTo = null,
            TravelClass? seatClass = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            var allFlights = GetAll();
            var allBookings = _bookingRepository.GetAll();

            FlightBookingUtilities.AttachBookings(allFlights, allBookings);

            var query =
                from f in allFlights
                join b in allBookings on f.Id equals b.FlightId into flightBookings
                let bookings = flightBookings.ToList()
                where (string.IsNullOrEmpty(departureCountry) ||
                       f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
                where (string.IsNullOrEmpty(destinationCountry) ||
                       f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase))
                where (string.IsNullOrEmpty(departureAirport) ||
                       f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase))
                where (string.IsNullOrEmpty(arrivalAirport) ||
                       f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
                where (!departureDateFrom.HasValue || f.DepartureDate >= departureDateFrom.Value)
                where (!departureDateTo.HasValue || f.DepartureDate <= departureDateTo.Value)
                where (!seatClass.HasValue || bookings.Any(b => b.SeatClass.Name == seatClass.Value))
                where (!minPrice.HasValue || bookings.Any(b => b.SeatClass.CalculatePrice() >= minPrice.Value))
                where (!maxPrice.HasValue || bookings.Any(b => b.SeatClass.CalculatePrice() <= maxPrice.Value))
                select f;

            return query.ToList();
        }
    }

}
