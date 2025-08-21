using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IPassengerService
    {
        Passenger GetPassengerById(int id);
        List<Passenger> GetAllPassengersWithBookings();
        Passenger GetPassengerByIdWithBookings(int id);
        void DeletePassengerWithBookings(int passengerId);
        void AddPassengerWithBookings(Passenger passenger);
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(int id, Passenger newPassenger);
        void DeletePassenger(int id);
        List<Passenger> GetAllPassengers();
        List<Flight> SearchAvailableFlights(
            string departureCountry = null,
            string destinationCountry = null,
            string departureAirport = null,
            string arrivalAirport = null,
            DateTime? departureDateFrom = null,
            DateTime? departureDateTo = null,
            TravelClass? seatClass = null,
            decimal? minPrice = null,
            decimal? maxPrice = null
        );
        List<Booking> GetBookingsForPassenger(int passengerId);
        void CancelBooking(int passengerId, int bookingId);
        void ModifyBooking(int passengerId, int bookingId, Booking newBooking);
    }

}
