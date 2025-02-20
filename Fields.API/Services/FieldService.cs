using Fields.API.Utils;
using Fields.Domain.Interfaces;
using Fields.Domain.Models;
using Fields.Domain.Models.Dto;

namespace Fields.API.Services;

public class FieldService : IFieldService
{
    private readonly List<Field> _fields;
    private readonly Dictionary<int, Centroid> _centroids;

    public FieldService(IKmlParser kmlParser, IConfiguration configuration)
    {
        var fieldsPath = configuration["KmlPaths:Fields"] ?? "Data/fields.kml";
        var centroidsPath = configuration["KmlPaths:Centroids"] ?? "Data/centroids.kml";

        _fields = kmlParser.ParseFields(fieldsPath);
        _centroids = kmlParser.ParseCentroids(centroidsPath);
    }

    public IEnumerable<FieldDto> GetAllFields() =>
        _fields.Select(f => new FieldDto
        {
            Id = f.Id,
            Name = f.Name,
            Size = f.Size,
            Locations = new LocationsDto
            {
                Center = _centroids[f.Id].Coordinates,
                Polygon = f.Polygon
            }
        });

    public double? GetFieldSize(int id) =>
        _fields.FirstOrDefault(f => f.Id == id)?.Size;

    public double? GetDistanceToPoint(int id, double lat, double lng)
    {
        if (_centroids.TryGetValue(id, out var centroid))
        {
            return GeoUtils.CalculateDistance(centroid.Coordinates[0], centroid.Coordinates[1], lat, lng);
        }
        return null;
    }

    public FieldInfoDto? GetFieldContainingPoint(double lat, double lng)
    {
        foreach (var field in _fields)
        {
            if (GeoUtils.IsPointInPolygon(lat, lng, field.Polygon))
            {
                return new FieldInfoDto { Id = field.Id, Name = field.Name };
            }
        }
        return null;
    }
}