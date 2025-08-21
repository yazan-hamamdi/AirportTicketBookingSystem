using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.IServices
{
    public interface IManagerService
    {
        Manager GetManagerById(int id);
        List<Manager> GetAllManagers();
        void AddManager(Manager manager);
        void UpdateManager(int id, Manager updatedManager);
        void DeleteManager(int id);
        List<Booking> FilterBookingsForManager(
            int? flightId = null,
            int? passengerId = null,
            DateTime? bookingDateFrom = null,
            DateTime? bookingDateTo = null,
            TravelClass? seatClass = null,
            decimal? minPrice = null,
            decimal? maxPrice = null
        );
    }

}
