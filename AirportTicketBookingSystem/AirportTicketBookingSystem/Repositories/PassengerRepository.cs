using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Adapters;


namespace AirportTicketBookingSystem.Repositories
{
    public class PassengerRepository : BaseRepository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(string filePath, ICsvFileHelperAdapter csvHelper)
            : base(filePath, csvHelper) { }
    }
}