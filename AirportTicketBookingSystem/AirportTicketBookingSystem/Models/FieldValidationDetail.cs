namespace AirportTicketBookingSystem.Models
{
    public class FieldValidationDetail
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public List<string> Constraints { get; set; } = new List<string>();
    }
}
