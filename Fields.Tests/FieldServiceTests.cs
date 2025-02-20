using System.Collections.Generic;
using Fields.API.Services;
using Fields.Domain.Interfaces;
using Fields.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Fields.Tests;

public class FieldServiceTests
{
    private readonly IFieldService _fieldService;

    public FieldServiceTests()
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["KmlPaths:Fields"]).Returns("Data/fields.kml");
        configurationMock.Setup(c => c["KmlPaths:Centroids"]).Returns("Data/centroids.kml");
        
        var kmlParserMock = new Mock<IKmlParser>();
        
        var polygon = new List<double[]>
        {
            new [] { 45.7074047669366, 41.3346809239899 },
            new [] { 45.707543073278, 41.3414148034017 },
            new [] { 45.6850638023809, 41.3414148034017 },
            new [] { 45.7074047669366, 41.3346809239899 } 
        };

        kmlParserMock.Setup(k => k.ParseFields(It.IsAny<string>())).Returns(new List<Field>
        {
            new()
            {
                Id = 1,
                Name = "м01",
                Size = 100.0,
                Polygon = polygon
            }
        });

        kmlParserMock.Setup(k => k.ParseCentroids(It.IsAny<string>())).Returns(new Dictionary<int, Centroid>
        {
            { 1, new Centroid { Id = 1, Coordinates = new [] { 45.6962567581079, 41.3380610642585 } } }
        });
        
        _fieldService = new FieldService(kmlParserMock.Object, configurationMock.Object);
    }

    [Fact]
    public void GetAllFields_ShouldReturnNonEmptyList()
    {
        var fields = _fieldService.GetAllFields();
        Assert.NotEmpty(fields);
    }

    [Fact]
    public void GetFieldSize_ValidId_ShouldReturnSize()
    {
        var size = _fieldService.GetFieldSize(1);
        Assert.NotNull(size);
        Assert.Equal(100.0, size);
    }

    [Fact]
    public void GetDistanceToPoint_ShouldReturnCorrectDistance()
    {
        double lat = 45.70;
        double lng = 41.34;
        var distance = _fieldService.GetDistanceToPoint(1, lat, lng);
        Assert.NotNull(distance);
        Assert.True(distance > 0);
    }

    [Fact]
    public void GetFieldSize_InvalidId_ShouldReturnNull()
    {
        var size = _fieldService.GetFieldSize(999);
        Assert.Null(size);
    }
}
