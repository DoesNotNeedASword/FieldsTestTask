using Fields.Domain.Models;
using Fields.Domain.Models.Dto;

namespace Fields.Domain.Interfaces;

public interface IFieldService
{
    IEnumerable<FieldDto> GetAllFields();
    double? GetFieldSize(int id);
    double? GetDistanceToPoint(int id, double lat, double lng);
    FieldInfoDto? GetFieldContainingPoint(double lat, double lng);
}