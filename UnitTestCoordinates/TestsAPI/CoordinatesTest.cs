using System.Net;
using System.Net.Http.Headers;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace UnitTestCoordinates.TestsAPI;

internal class CoordinatesApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }
}
public class CoordinatesTest
{
    [Fact]
    public async Task GetCoordinates_WithCountGreaterThanOne()
    {
        await using var app = new CoordinatesApiApplication();

        using var httpClient = app.CreateClient();
        using var response = await httpClient.GetAsync("/coordinates?count=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCoordinates_WithCountLessThanOne()
    {
        await using var app = new CoordinatesApiApplication();

        using var httpClient = app.CreateClient();
        using var response = await httpClient.GetAsync("/coordinates?count=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostDistance_WithLengthGreaterThanOne()
    {
        await using var app = new CoordinatesApiApplication();

        Random latitudeRandom = new Random();
        Random longitudeRandom = new Random();

        List<double> latitude = new List<double>() { 60.021158, 60.024157 };
        List<double> longitude = new List<double>() { 30.321135, 30.323133 };

        List<Coordinates> coordinates = new List<Coordinates>();
        coordinates.Add(new Coordinates { Latitude = latitude[0], Longitude = longitude[0] });
        coordinates.Add(new Coordinates { Latitude = latitude[1], Longitude = longitude[1] });

        using var httpClient = app.CreateClient();
        HttpResponseMessage response = await httpClient.PostAsJsonAsync<List<Coordinates>> ("/coordinates", coordinates);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"metres\":351.465,\"miles\":0.218}", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task PostDistance_WithLengthLessOne()
    {
        await using var app = new CoordinatesApiApplication();

        List<Coordinates> coordinates = new List<Coordinates>();

        using var httpClient = app.CreateClient();
        HttpResponseMessage response = await httpClient.PostAsJsonAsync<List<Coordinates>>("/coordinates", coordinates);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"metres\":0,\"miles\":0}", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task PostDistance_WithLengthOne()
    {
        await using var app = new CoordinatesApiApplication();

        Random latitudeRandom = new Random();
        Random longitudeRandom = new Random();

        double latitude = Math.Round(latitudeRandom.NextDouble(0, 90.0), 6);
        double longitude = Math.Round(longitudeRandom.NextDouble(0, 180.0), 6);

        List<Coordinates> coordinates = new List<Coordinates>();
        coordinates.Add(new Coordinates { Latitude = latitude, Longitude = longitude });

        using var httpClient = app.CreateClient();
        HttpResponseMessage response = await httpClient.PostAsJsonAsync<List<Coordinates>>("/coordinates", coordinates);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"metres\":0,\"miles\":0}", await response.Content.ReadAsStringAsync());
    }
}