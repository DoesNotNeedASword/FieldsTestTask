namespace Fields.Domain.Models.Dto;

public class FieldDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Size { get; set; }
    public LocationsDto Locations { get; set; } = new();
}