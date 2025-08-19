
namespace AirportTicketBookingSystem.DTOs
{
    public class BookingCsv
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public DateTime BookingDate { get; set; }
        public string SeatClass { get; set; }
        public decimal Price { get; set; }
    }

}
