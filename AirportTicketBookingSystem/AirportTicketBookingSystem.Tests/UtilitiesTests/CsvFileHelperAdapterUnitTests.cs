using AirportTicketBookingSystem.Adapters;
using FluentAssertions;
using Moq;

namespace AirportTicketBookingSystem.Tests.UtilitiesTests
{
    public class CsvFileHelperAdapterUnitTests
    {
        [Fact]
        public void ReadFromCsv_Should_Call_Helper_And_Return_Data()
        {
            // Arrange
            var mockAdapter = new Mock<ICsvFileHelperAdapter>();
            var expectedData = new List<string> { "yazan", "Bob" };
            mockAdapter
                .Setup(x => x.ReadFromCsv<string>("test.csv"))
                .Returns(expectedData);

            // Act
            var result = mockAdapter.Object.ReadFromCsv<string>("test.csv");

            // Assert
            result.Should().BeEquivalentTo(expectedData);
            mockAdapter.Verify(x => x.ReadFromCsv<string>("test.csv"), Times.Once);
        }

        [Fact]
        public void WriteToCsv_Should_Call_Helper_With_Correct_Data()
        {
            // Arrange
            var mockAdapter = new Mock<ICsvFileHelperAdapter>();
            var dataToWrite = new List<string> { "yazan", "Bob" };
            mockAdapter
                .Setup(x => x.WriteToCsv("test.csv", dataToWrite));

            // Act
            mockAdapter.Object.WriteToCsv("test.csv", dataToWrite);

            // Assert
            mockAdapter.Verify(x => x.WriteToCsv("test.csv", dataToWrite), Times.Once);
        }
    }
}
