using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace AirportTicketBookingSystem.Utilities
{
    public static class CsvFileHelper
    {
        public static List<T> ReadFromCsv<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV file not found", filePath);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            return csv.GetRecords<T>().ToList();
        }

        public static void WriteToCsv<T>(string filePath, List<T> records)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }
    }
}
