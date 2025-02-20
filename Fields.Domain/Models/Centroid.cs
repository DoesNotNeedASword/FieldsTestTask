namespace Fields.Domain.Models;

public class Centroid
{
    public int Id { get; set; }
    public double[] Coordinates { get; set; } = new double[2];
}