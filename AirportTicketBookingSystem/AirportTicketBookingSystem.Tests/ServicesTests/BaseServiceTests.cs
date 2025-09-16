using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.Tests.TestDoubles.MyProject.Tests.TestDoubles;
using FluentAssertions;
using Moq;

namespace AirportTicketBookingSystem.Tests.ServicesTests
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepository<TestEntity>> _mockRepo;
        private readonly BaseService<TestEntity> _service;

        public BaseServiceTests()
        {
            _mockRepo = new Mock<IRepository<TestEntity>>();
            _service = new TestService(_mockRepo.Object);
        }

        private class TestService : BaseService<TestEntity>
        {
            public TestService(IRepository<TestEntity> repo) : base(repo) { }
        }

        [Fact]
        public void GetById_EntityExists_ReturnsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "yazan" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entity);

            // Act
            var result = _service.GetById(1);

            // Assert
            result.Should().Be(entity);
            _mockRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_EntityNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(1)).Throws<KeyNotFoundException>();

            // Act
            Action act = () => _service.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("TestEntity with Id 1 does not exist");
        }

        [Fact]
        public void Add_EntityIsNull_ThrowsArgumentNullException()
        {
            // Act
            Action act = () => _service.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_EntityIsValid_AddsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Bob" };

            // Act
            _service.Add(entity);

            // Assert
            _mockRepo.Verify(r => r.Add(entity), Times.Once);
        }

        [Fact]
        public void Update_EntityIsNull_ThrowsArgumentNullException()
        {
            // Act
            Action act = () => _service.Update(1, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_EntityNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "yazan" };
            _mockRepo.Setup(r => r.Update(1, entity)).Throws<KeyNotFoundException>();

            // Act
            Action act = () => _service.Update(1, entity);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("Cannot update. TestEntity with Id 1 does not exist");
        }

        [Fact]
        public void Delete_EntityNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Throws<KeyNotFoundException>();

            // Act
            Action act = () => _service.Delete(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("Cannot delete. TestEntity with Id 1 does not exist");
        }
    }
}
