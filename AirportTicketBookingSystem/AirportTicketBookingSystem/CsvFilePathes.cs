namespace AirportTicketBookingSystem
{
    public struct CsvFilePaths
    {
        private static readonly string BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSVFiles");

        public static string Manager => Path.Combine(BasePath, "managers.csv");
        public static string Flight => Path.Combine(BasePath, "flights.csv");
        public static string Passenger => Path.Combine(BasePath, "passengers.csv");
        public static string Booking => Path.Combine(BasePath, "bookings.csv");
    }

}
