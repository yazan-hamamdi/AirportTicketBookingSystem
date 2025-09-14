using AirportTicketBookingSystem.Adapters;
using AirportTicketBookingSystem.Interfaces;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AirportTicketBookingSystem.Tests.RepositoriesTests
{
    public class FlightRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICsvFileHelperAdapter> _csvMock;
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly FlightRepository _repository;
        private readonly Mock<ISeatClassAdapter> _seatClassAdapterMock;
        private readonly string _fakePath = "fake.csv";

        public FlightRepositoryTests()
        {
            _fixture = new Fixture();
            _csvMock = new Mock<ICsvFileHelperAdapter>();
            _bookingRepoMock = new Mock<IBookingRepository>();
            _repository = new FlightRepository(_fakePath, _bookingRepoMock.Object, _csvMock.Object);
            _seatClassAdapterMock = new Mock<ISeatClassAdapter>();
        }

        [Fact]
        public void SearchAvailableFlights_ShouldReturnAll_WhenNoFilters()
        {
            // Arrange
            var flights = new List<Flight>
               {
                 new Flight { Id = 1, DepartureDate = new DateTime(2025, 10, 10) },
                 new Flight { Id = 2, DepartureDate = new DateTime(2025, 11, 15) },
                 new Flight { Id = 3, DepartureDate = new DateTime(2025, 12, 20) }
               };
            var bookings = flights.Select(f => new Booking(
                id: f.Id,
                flightId: f.Id,
                passengerId: 10,
                bookingDate: f.DepartureDate,
                seatClass: _seatClassAdapterMock.Object.ToSeatClass("business"),
                price: 200
            )).ToList();
            _csvMock.Setup(c => c.ReadFromCsv<Flight>(_fakePath)).Returns(flights);
            _bookingRepoMock.Setup(b => b.GetAll()).Returns(bookings);
            // Act
            var result = _repository.SearchAvailableFlights();
            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDepartureCountry()
        {
            // Arrange
            var matchingFlight = new Flight { Id = 1, DepartureCountry = "France", DepartureDate = new DateTime(2025, 10, 10) };
            var nonMatchingFlight = new Flight { Id = 2, DepartureCountry = "Germany", DepartureDate = new DateTime(2025, 11, 11) };
            var flights = new List<Flight> { matchingFlight, nonMatchingFlight };
            var bookings = _fixture.Build<Booking>()
                .With(b => b.BookingDate, matchingFlight.DepartureDate)
                .With(b => b.SeatClass, _seatClassAdapterMock.Object.ToSeatClass("business"))
                .With(b => b.FlightId, matchingFlight.Id)
                .With(b => b.PassengerId, 10)
                .CreateMany(2)
                .ToList();
            _csvMock.Setup(c => c.ReadFromCsv<Flight>(_fakePath)).Returns(flights);
            _bookingRepoMock.Setup(b => b.GetAll()).Returns(bookings);
            // Act
            var result = _repository.SearchAvailableFlights(departureCountry: "France");
            // Assert
            result.Should().ContainSingle(f => f.DepartureCountry == "France");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDateRange()
        {
            // Arrange
            var matchingFlight = new Flight { Id = 1, DepartureDate = new DateTime(2025, 10, 10) };
            var nonMatchingFlight = new Flight { Id = 2, DepartureDate = new DateTime(2026, 1, 1) };
            var flights = new List<Flight> { matchingFlight, nonMatchingFlight };
            var bookings = _fixture.Build<Booking>()
                .With(b => b.BookingDate, matchingFlight.DepartureDate)
                .With(b => b.SeatClass, _seatClassAdapterMock.Object.ToSeatClass("business"))
                .With(b => b.FlightId, matchingFlight.Id)
                .With(b => b.PassengerId, 11)
                .CreateMany(2)
                .ToList();
            _csvMock.Setup(c => c.ReadFromCsv<Flight>(_fakePath)).Returns(flights);
            _bookingRepoMock.Setup(b => b.GetAll()).Returns(bookings);
            // Act
            var result = _repository.SearchAvailableFlights(
                departureDateFrom: new DateTime(2025, 10, 1),
                departureDateTo: new DateTime(2025, 12, 31));
            // Assert
            result.Should().ContainSingle(f => f.DepartureDate == new DateTime(2025, 10, 10));
        }
    }
}
