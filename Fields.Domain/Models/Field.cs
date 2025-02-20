using NetTopologySuite.Geometries;

namespace Fields.Domain.Models;

public class Field
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Size { get; set; }
    public List<double[]> Polygon { get; set; } = new();
    public Geometry Geometry { get; set; } = default!;
}