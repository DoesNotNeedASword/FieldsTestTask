using NetTopologySuite.Geometries;

namespace Fields.API.Utils;

public static class GeoUtils
{
    private const double EarthRadiusKm = 6371.0;

    /// <summary>
    /// Рассчитывает расстояние между двумя точками (Haversine formula).
    /// </summary>
    /// <param name="lat1">Широта первой точки</param>
    /// <param name="lng1">Долгота первой точки</param>
    /// <param name="lat2">Широта второй точки</param>
    /// <param name="lng2">Долгота второй точки</param>
    /// <returns>Расстояние в метрах</returns>
    public static double CalculateDistance(double lat1, double lng1, double lat2, double lng2)
    {
        var lat1Rad = DegreesToRadians(lat1);
        var lng1Rad = DegreesToRadians(lng1);
        var lat2Rad = DegreesToRadians(lat2);
        var lng2Rad = DegreesToRadians(lng2);

        var deltaLat = lat2Rad - lat1Rad;
        var deltaLng = lng2Rad - lng1Rad;

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distanceKm = EarthRadiusKm * c;
        return distanceKm * 1000; 
    }

    /// <summary>
    /// Проверяет, принадлежит ли точка полигону.
    /// </summary>
    /// <param name="lat">Широта точки</param>
    /// <param name="lng">Долгота точки</param>
    /// <param name="polygon">Полигон, представленный массивом координат</param>
    /// <returns>True, если точка внутри полигона, иначе False</returns>
    public static bool IsPointInPolygon(double lat, double lng, List<double[]> polygon)
    {
        var geometryFactory = new GeometryFactory();
        var point = geometryFactory.CreatePoint(new Coordinate(lng, lat));

        var coordinates = polygon.Select(coord => new Coordinate(coord[0], coord[1])).ToArray();
        var linearRing = geometryFactory.CreateLinearRing(coordinates);
        var poly = geometryFactory.CreatePolygon(linearRing);

        return poly.Contains(point);
    }
    
    private static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180.0);

    /// <summary>
    /// Форматирует координаты в удобный массив [lat, lng].
    /// </summary>
    public static double[] FormatCoordinates(double lat, double lng) => new double[] { lat, lng };
}
