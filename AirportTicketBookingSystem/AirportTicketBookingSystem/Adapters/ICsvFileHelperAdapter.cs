namespace AirportTicketBookingSystem.Adapters
{
    public interface ICsvFileHelperAdapter
    {
        List<T> ReadFromCsv<T>(string filePath);
        void WriteToCsv<T>(string filePath, List<T> records);
    }
}