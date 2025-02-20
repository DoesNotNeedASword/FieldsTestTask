using System.Globalization;
using System.Xml.Linq;
using Fields.Domain.Interfaces;
using Fields.Domain.Models;
using NetTopologySuite.Geometries;

namespace Fields.API.Utils;

public class KmlParser : IKmlParser
{
    private static readonly XNamespace kmlNs = "http://www.opengis.net/kml/2.2";

    public List<Field> ParseFields(string filePath)
    {
        var doc = XDocument.Load(filePath);

        return (from placemark in doc.Descendants(kmlNs + "Placemark")
            let id = int.Parse(placemark.Descendants(kmlNs + "SimpleData")
                .First(sd => sd.Attribute("name")?.Value == "fid").Value, CultureInfo.InvariantCulture)
            let name = placemark.Element(kmlNs + "name")?.Value ?? $"Field {id}"
            let size = double.Parse(placemark.Descendants(kmlNs + "SimpleData")
                .First(sd => sd.Attribute("name")?.Value == "size").Value, CultureInfo.InvariantCulture)
            let coordinates = placemark.Descendants(kmlNs + "coordinates")
                .First()
                .Value
                .Trim()
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(coord => coord.Split(',')
                    .Take(2)
                    .Select(s => double.Parse(s, CultureInfo.InvariantCulture))
                    .ToArray())
                .ToList()
            select new Field
            {
                Id = id,
                Name = name,
                Size = size,
                Polygon = coordinates,
                Geometry = new Polygon(new LinearRing(coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray()))
            }).ToList();
    }

    public Dictionary<int, Centroid> ParseCentroids(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var centroids = new Dictionary<int, Centroid>();

        foreach (var placemark in doc.Descendants(kmlNs + "Placemark"))
        {
            var id = int.Parse(placemark.Descendants(kmlNs + "SimpleData")
                .First(sd => sd.Attribute("name")?.Value == "fid").Value, CultureInfo.InvariantCulture);
            var coordinates = placemark.Descendants(kmlNs + "coordinates").First().Value
                .Trim()
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .First()
                .Split(',')
                .Take(2)
                .Select(s => double.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();

            centroids[id] = new Centroid { Id = id, Coordinates = coordinates };
        }
        return centroids;
    }
}
