using Microsoft.AspNetCore.Mvc;
using Fields.Domain.Interfaces;
using Fields.Domain.Models;
using Fields.Domain.Models.Dto;

namespace Fields.API.Controllers;

[ApiController]
[Route("api/fields")]
public class FieldController : ControllerBase
{
    private readonly IFieldService _fieldService;

    public FieldController(IFieldService fieldService)
    {
        _fieldService = fieldService;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<FieldDto>> GetAllFields()
    {
        return Ok(_fieldService.GetAllFields());
    }
    
    [HttpGet("{id:int}/size")]
    public ActionResult<double> GetFieldSize(int id)
    {
        var size = _fieldService.GetFieldSize(id);
        return size.HasValue ? Ok(size.Value) : NotFound("Field not found");
    }

    [HttpGet("{id:int}/distance")]
    public ActionResult<double> GetDistanceToPoint(int id, [FromQuery] double lat, [FromQuery] double lng)
    {
        var distance = _fieldService.GetDistanceToPoint(id, lat, lng);
        return distance.HasValue ? Ok(distance.Value) : NotFound("Field or centroid not found");
    }
    
    [HttpGet("contains")]
    public ActionResult<object> CheckPointInField([FromQuery] double lat, [FromQuery] double lng)
    {
        var result = _fieldService.GetFieldContainingPoint(lat, lng);
        return result is not null ? result : NotFound();
    }
}