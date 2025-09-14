using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Adapters;

namespace AirportTicketBookingSystem.Repositories
{
    public class ManagerRepository : BaseRepository<Manager>, IManagerRepository
    {
        public ManagerRepository(string filePath, ICsvFileHelperAdapter csvHelper) 
            : base(filePath, csvHelper) { }
    }
}
