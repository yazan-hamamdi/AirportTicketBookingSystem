using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Services;
using FluentAssertions;
using Moq;

namespace AirportTicketBookingSystem.Tests.ServicesTests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepo;
        private readonly Mock<IPassengerRepository> _mockPassengerRepo;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _mockBookingRepo = new Mock<IBookingRepository>();
            _mockPassengerRepo = new Mock<IPassengerRepository>();
            _service = new BookingService(_mockBookingRepo.Object, _mockPassengerRepo.Object);
        }

        [Fact]
        public void GetBookingsForPassenger_Found()
        {
            // Arrange
            var passenger = new Passenger { Id = 1, FullName = "yazan" };
            _mockPassengerRepo.Setup(r => r.GetById(1)).Returns(passenger);
            var bookings = new List<Booking>
              {
                  new Booking { Id = 10, PassengerId = 1 },
                  new Booking { Id = 11, PassengerId = 2 }
              };
            _mockBookingRepo.Setup(r => r.GetAll()).Returns(bookings);

            // Act
            var result = _service.GetBookingsForPassenger(1);

            // Assert
            result.Should().HaveCount(1);
            result[0].PassengerId.Should().Be(1);
        }

        [Fact]
        public void GetBookingsForPassenger_NotFound()
        {
            // Arrange
            _mockPassengerRepo.Setup(r => r.GetById(1)).Returns((Passenger)null);

            // Act
            Action act = () => _service.GetBookingsForPassenger(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("Passenger with Id 1 does not exist");
        }

        [Fact]
        public void CancelBooking_Found()
        {
            // Arrange
            var booking = new Booking { Id = 10, PassengerId = 1 };
            _mockBookingRepo.Setup(r => r.GetById(10)).Returns(booking);

            // Act
            _service.CancelBooking(1, 10);

            // Assert
            _mockBookingRepo.Verify(r => r.Delete(10), Times.Once);
        }

        [Fact]
        public void CancelBooking_NotFound()
        {
            // Arrange
            _mockBookingRepo.Setup(r => r.GetById(10)).Returns((Booking)null);

            // Act
            Action act = () => _service.CancelBooking(1, 10);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("Booking not found for this passenger");
        }

        [Fact]
        public void UpdateBooking_Found()
        {
            // Arrange
            var oldBooking = new Booking { Id = 10, PassengerId = 1 };
            var newBooking = new Booking { Id = 10, PassengerId = 1 };
            _mockBookingRepo.Setup(r => r.GetById(10)).Returns(oldBooking);

            // Act
            _service.UpdateBooking(1, 10, newBooking);

            // Assert
            _mockBookingRepo.Verify(r => r.Update(10, newBooking), Times.Once);
        }

        [Fact]
        public void UpdateBooking_NotFound()
        {
            // Arrange
            var oldBooking = new Booking { Id = 10, PassengerId = 2 };
            var newBooking = new Booking { Id = 10, PassengerId = 1 };
            _mockBookingRepo.Setup(r => r.GetById(10)).Returns(oldBooking);

            // Act
            Action act = () => _service.UpdateBooking(1, 10, newBooking);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("Booking not found for this passenger");
        }

        [Fact]
        public void GetBookingsByPassenger_Found()
        {
            // Arrange
            var bookings = new List<Booking>
              {
                 new Booking { Id = 1, PassengerId = 1 },
                 new Booking { Id = 2, PassengerId = 1 }
              };
            _mockBookingRepo.Setup(r => r.GetByPassengerId(1)).Returns(bookings);

            // Act
            var result = _service.GetBookingsByPassengerId(1);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetBookingsByPassenger_NotFound()
        {
            // Arrange
            _mockBookingRepo.Setup(r => r.GetByPassengerId(1)).Returns(new List<Booking>());

            // Act
            Action act = () => _service.GetBookingsByPassengerId(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
               .WithMessage("No bookings found for passenger with Id 1");
        }
    }
}
