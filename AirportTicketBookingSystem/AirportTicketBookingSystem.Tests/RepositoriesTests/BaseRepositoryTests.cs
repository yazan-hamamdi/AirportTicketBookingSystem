using FluentAssertions;
using AutoFixture;
using Moq;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Tests.TestDoubles.MyProject.Tests.TestDoubles;
using AirportTicketBookingSystem.Tests.TestDoubles;
using AirportTicketBookingSystem.Adapters;

namespace AirportTicketBookingSystem.Tests.RepositoriesTests
{
    public class BaseRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICsvFileHelperAdapter> _csvMock;
        private readonly BaseRepository<TestEntity> _repository;
        private const string _fakePath = "fake.csv";

        public BaseRepositoryTests()
        {
            _fixture = new Fixture();
            _csvMock = new Mock<ICsvFileHelperAdapter>();
            _repository = new TestEntityRepository(_fakePath, _csvMock.Object);
        }

        [Fact]
        public void Add_ShouldAssignNewId_AndPersistEntity()
        {
            // Arrange
            var entity = _fixture.Build<TestEntity>().Without(i => i.Id).Create();
            var entities = new List<TestEntity>();
            _csvMock.Setup(c => c.ReadFromCsv<TestEntity>(_fakePath)).Returns(entities);

            // Act
            _repository.Add(entity);

            // Assert
            entity.Id.Should().BeGreaterThan(0);
            _csvMock.Verify(c => c.WriteToCsv(_fakePath, It.Is<List<TestEntity>>(l => l.Contains(entity))), Times.Once);
        }

        [Fact]
        public void Update_ShouldReplaceEntity_WhenExists()
        {
            // Arrange
            var entity = _fixture.Build<TestEntity>().With(e => e.Id, 1).Create();
            var entities = new List<TestEntity> { entity };
            _csvMock.Setup(c => c.ReadFromCsv<TestEntity>(_fakePath)).Returns(entities);
            var updated = _fixture.Build<TestEntity>().With(e => e.Id, 1).Create();

            // Act
            _repository.Update(1, updated);

            // Assert
            _csvMock.Verify(c => c.WriteToCsv(_fakePath,
                It.Is<List<TestEntity>>(l => l.Any(e => e.Id == 1 && e.Name == updated.Name))),
                Times.Once);
        }

        [Fact]
        public void Delete_ShouldRemoveEntity_WhenExists()
        {
            // Arrange
            var entity = _fixture.Build<TestEntity>().With(e => e.Id, 1).Create();
            var entities = new List<TestEntity> { entity };
            _csvMock.Setup(c => c.ReadFromCsv<TestEntity>(_fakePath)).Returns(entities);

            // Act
            _repository.Delete(1);

            // Assert
            _csvMock.Verify(c => c.WriteToCsv(_fakePath, It.Is<List<TestEntity>>(l => !l.Any())), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = _fixture.CreateMany<TestEntity>(3).ToList();
            _csvMock.Setup(c => c.ReadFromCsv<TestEntity>(_fakePath)).Returns(entities);

            // Act
            var result = _repository.GetAll();

            // Assert
            result.Should().HaveCount(3);
        }
    }
}

