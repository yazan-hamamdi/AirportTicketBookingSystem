using AirportTicketBookingSystem.Enums;
using AirportTicketBookingSystem.IRepositories;
using AirportTicketBookingSystem.IServices;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;

namespace AirportTicketBookingSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepo _managerRepo;
        private readonly IBookingRepo _bookingRepo;

        public ManagerService(IManagerRepo managerRepo, IBookingRepo bookRepo)
        {
            _managerRepo = managerRepo;
            _bookingRepo = bookRepo;
        }

        public Manager GetManagerById(int id)
        {
            try
            {
                return _managerRepo.GetManagerById(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Manager with Id {id} does not exist");
            }
        }

        public List<Manager> GetAllManagers()
        {
            return _managerRepo.GetAllManagers();
        }

        public void AddManager(Manager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            _managerRepo.AddManager(manager);
        }

        public void UpdateManager(int id, Manager updatedManager)
        {
            if (updatedManager == null) throw new ArgumentNullException(nameof(updatedManager));

            try
            {
                _managerRepo.UpdateManager(m => m.Id == id, updatedManager);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot update. Manager with Id {id} does not exist");
            }
        }

        public void DeleteManager(int id)
        {
            try
            {
                _managerRepo.DeleteManager(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Cannot delete. Manager with Id {id} does not exist");
            }
        }
        public List<Booking> FilterBookingsForManager( int? flightId = null, int? passengerId = null,
        DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null, TravelClass? seatClass = null,
        decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allBookings = _bookingRepo.GetAllBookings();

            var query =
                from b in allBookings
                where (!flightId.HasValue || b.FlightId == flightId.Value)
                where (!passengerId.HasValue || b.PassengerId == passengerId.Value)
                where (!bookingDateFrom.HasValue || b.BookingDate >= bookingDateFrom.Value)
                where (!bookingDateTo.HasValue || b.BookingDate <= bookingDateTo.Value)
                where (!seatClass.HasValue || b.SeatClass.Name == seatClass.Value)
                where (!minPrice.HasValue || b.SeatClass.CalculatePrice() >= minPrice.Value)
                where (!maxPrice.HasValue || b.SeatClass.CalculatePrice() <= maxPrice.Value)
                select b;

            return query.ToList();
        }

    }
}
