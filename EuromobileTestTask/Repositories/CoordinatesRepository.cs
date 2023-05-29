namespace EuromobileTestTask.Repositories;

public class CoordinatesRepository
{
    public static List<Coordinates> GetRandomCoordinates(int count)
    {
        List<Coordinates> coordinates = new List<Coordinates>();
        for (int i = 0; i < count; i++)
        {
            Random latitudeRandom = new Random();
            Random longitudeRandom = new Random();

            double latitude = Math.Round(latitudeRandom.NextDouble(0, 90.0), 6);
            double longitude = Math.Round(longitudeRandom.NextDouble(0, 180.0), 6);

            coordinates.Add(new Coordinates() { Latitude = latitude, Longitude = longitude });
        }
        return coordinates;
    }
    public static TotalDistance GetTotalDistance(List<Coordinates> coordinates)
    {

        double metres = 0;
        double miles = 0;

        for (int i = 0; i < coordinates.Count - 1; i++)
        {
            double haversine = CalculateHaversine(coordinates[i], coordinates[i + 1]);
            metres += Math.Round(haversine, 3);
            miles += MetresToMiles(metres);
        }
        return new TotalDistance() { Metres = metres, Miles = Math.Round(miles, 3) };
    }
    private static double CalculateHaversine(Coordinates coordinates1, Coordinates coordinates2)
    {
        const double EARTH_RADIUS = 6371000d;

        double latitude1 = coordinates1.Latitude;
        double longitude1 = coordinates1.Longitude;
        double latitude2 = coordinates2.Latitude;
        double longitude2 = coordinates2.Longitude;

        double dLat = DegreesToRadians(latitude2 - latitude1);
        double dLon = DegreesToRadians(longitude2 - longitude1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2);
        double b = Math.Cos(DegreesToRadians(latitude1)) * Math.Cos(DegreesToRadians(latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = Math.Asin(Math.Sqrt(a + b));
        double d = 2 * EARTH_RADIUS * c;

        return d;
    }
    private static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
    private static double MetresToMiles(double metres)
    {
        return metres * 0.000621;
    }
}
public static class RandomExtensions
{
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
        return random.NextDouble() * (maxValue - minValue) + minValue;
    }
}