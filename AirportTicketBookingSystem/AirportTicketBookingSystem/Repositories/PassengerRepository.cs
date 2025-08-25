using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Interfaces;


namespace AirportTicketBookingSystem.Repositories
{
    public class PassengerRepository : CsvRepositoryBase<Passenger>, IPassengerRepository
    {
        public PassengerRepository(string filePath) : base(filePath) { }
    }
}