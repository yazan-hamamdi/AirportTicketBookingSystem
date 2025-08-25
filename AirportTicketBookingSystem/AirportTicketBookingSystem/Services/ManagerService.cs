using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class ManagerService : BaseService<Manager>, IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
            : base(managerRepository)
        {
            _managerRepository = managerRepository;
        }
    }
}
