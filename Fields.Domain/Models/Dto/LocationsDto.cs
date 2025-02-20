namespace Fields.Domain.Models.Dto;

public class LocationsDto
{
    public double[] Center { get; set; } = new double[2];
    public List<double[]> Polygon { get; set; } = [];
}