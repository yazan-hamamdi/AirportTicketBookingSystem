using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Utilities;
using FluentAssertions;

namespace AirportTicketBookingSystem.Tests.UtilitiesTests
{
    public class SeatClassExtensionsTests
    {
        [Theory]
        [InlineData("economy", typeof(EconomyClass))]
        [InlineData("business", typeof(BusinessClass))]
        [InlineData("first", typeof(FirstClass))]
        [InlineData("ECONOMY", typeof(EconomyClass))]
        [InlineData("unknown", typeof(EconomyClass))]
        public void ToSeatClass_ShouldReturn_CorrectType(string input, System.Type expectedType)
        {
            // Act
            var result = input.ToSeatClass();

            // Assert
            result.Should().BeOfType(expectedType);
        }
    }
}
