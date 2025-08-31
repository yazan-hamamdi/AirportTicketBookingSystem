using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    public class PassengerService : BaseService<Passenger>, IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        public PassengerService( IPassengerRepository passengerRepository) 
            : base(passengerRepository) 
        {
            _passengerRepository = passengerRepository;
        }
    }
}
