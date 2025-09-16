using AirportTicketBookingSystem.Adapters;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Tests.TestDoubles.MyProject.Tests.TestDoubles;

namespace AirportTicketBookingSystem.Tests.TestDoubles
{
    public class TestEntityRepository : BaseRepository<TestEntity>
    {
        public TestEntityRepository(string filePath, ICsvFileHelperAdapter csvHelper)
            : base(filePath, csvHelper) { }
    }
}