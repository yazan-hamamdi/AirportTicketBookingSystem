using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Repositories
{
    public class ManagerRepository : CsvRepositoryBase<Manager>, IManagerRepository
    {
        public ManagerRepository(string filePath) : base(filePath) { }
    }
}
