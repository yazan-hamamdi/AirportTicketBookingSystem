using AirportTicketBookingSystem.Interfaces;

namespace AirportTicketBookingSystem.Utilities
{
    public static class IdGenerator
    {
        public static int GenerateNewId<T>(List<T> items) where T : IEntity
        {
            return items.Count == 0 ? 1 : items.Max(e => e.Id) + 1;
        }
    }
}
