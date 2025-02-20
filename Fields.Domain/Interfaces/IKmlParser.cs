using Fields.Domain.Models;

namespace Fields.Domain.Interfaces;

public interface IKmlParser
{
    public List<Field> ParseFields(string filePath);
    public Dictionary<int, Centroid> ParseCentroids(string filePath);

}