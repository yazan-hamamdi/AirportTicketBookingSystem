using AirportTicketBookingSystem.Adapters;
using AirportTicketBookingSystem.DTOs;
using AirportTicketBookingSystem.Repositories;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AirportTicketBookingSystem.Tests.RepositoriesTests
{
    public class BookingRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICsvFileHelperAdapter> _csvMock;
        private readonly BookingRepository _repository;
        private readonly Mock<ISeatClassAdapter> _seatClassAdapterMock;
        private const string _fakePath = "fake.csv";

        public BookingRepositoryTests()
        {
            _fixture = new Fixture();
            _csvMock = new Mock<ICsvFileHelperAdapter>();
            _repository = new BookingRepository(_fakePath, _csvMock.Object);
            _seatClassAdapterMock = new Mock<ISeatClassAdapter>();
        }

        [Fact]
        public void GetAll_ShouldMapBookingCsv_ToBooking()
        {
            // Arrange
            var csvRecords = _fixture.CreateMany<BookingCsv>(3).ToList();
            _csvMock.Setup(c => c.ReadFromCsv<BookingCsv>(_fakePath)).Returns(csvRecords);
            // Act
            var result = _repository.GetAll();
            // Assert
            result.Should().HaveCount(3);
            result.Select(r => r.Id).Should().BeEquivalentTo(csvRecords.Select(c => c.Id));
        }

        [Fact]
        public void GetByPassengerId_ShouldReturnCorrectBookings()
        {
            // Arrange
            var passengerId = 123;
            var csvRecords = _fixture.Build<BookingCsv>()
                                     .With(b => b.PassengerId, passengerId)
                                     .CreateMany(2)
                                     .ToList();
            csvRecords.AddRange(_fixture.Build<BookingCsv>()
                                        .With(b => b.PassengerId, 999)
                                        .CreateMany(2));
            _csvMock.Setup(c => c.ReadFromCsv<BookingCsv>(_fakePath)).Returns(csvRecords);
            // Act
            var result = _repository.GetByPassengerId(passengerId);
            // Assert
            result.Should().OnlyContain(b => b.PassengerId == passengerId);
        }

        [Theory]
        [InlineData(10, null, null, null)]               
        [InlineData(null, 20, null, null)]              
        [InlineData(null, null, "2023-01-01", "2023-12-31")]
        public void FilterBookingsForManager_ShouldApplyFilters(
            int? flightId,
            int? passengerId,
            string bookingDateFromStr,
            string bookingDateToStr)
        {
            // Arrange
            var bookingDateFrom = bookingDateFromStr != null ? DateTime.Parse(bookingDateFromStr) : (DateTime?)null;
            var bookingDateTo = bookingDateToStr != null ? DateTime.Parse(bookingDateToStr) : (DateTime?)null;
            var matching = new BookingCsv
            {
                Id = 1,
                FlightId = 10,
                PassengerId = 20,
                BookingDate = new DateTime(2023, 06, 15),
                SeatClass = "Economy",
                Price = 100
            };
            var notMatching = new BookingCsv
            {
                Id = 2,
                FlightId = 99,
                PassengerId = 88,
                BookingDate = new DateTime(2022, 12, 30),
                SeatClass = "Business",
                Price = 500
            };
            var records = new List<BookingCsv> { matching, notMatching };
            _csvMock.Setup(c => c.ReadFromCsv<BookingCsv>(_fakePath)).Returns(records);
            // Act
            var result = _repository.FilterBookingsForManager(
                flightId: flightId,
                passengerId: passengerId,
                bookingDateFrom: bookingDateFrom,
                bookingDateTo: bookingDateTo
            );
            // Assert
            result.Should().NotBeNull();
            result.Should().OnlyContain(b =>
                (!flightId.HasValue || b.FlightId == flightId.Value) &&
                (!passengerId.HasValue || b.PassengerId == passengerId.Value) &&
                (!bookingDateFrom.HasValue || b.BookingDate >= bookingDateFrom.Value) &&
                (!bookingDateTo.HasValue || b.BookingDate <= bookingDateTo.Value)
            );
        }
    }
}
