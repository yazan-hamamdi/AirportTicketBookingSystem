using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Repositories
{
    public class ManagerRepository : BaseRepository<Manager>, IManagerRepository
    {
        public ManagerRepository(string filePath) : base(filePath) { }
    }
}
