using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Adapters
{
    public class CsvFileHelperAdapter : ICsvFileHelperAdapter
    {
        public List<T> ReadFromCsv<T>(string filePath)
        {
            return CsvFileHelper.ReadFromCsv<T>(filePath);
        }

        public void WriteToCsv<T>(string filePath, List<T> records)
        {
            CsvFileHelper.WriteToCsv(filePath, records);
        }
    }
}
