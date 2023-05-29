using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/coordinates", ([FromQuery(Name = "count")] int count) => 
    count > 0 ? Results.Ok(CoordinatesRepository.GetRandomCoordinates(count)) 
    : Results.BadRequest())
    .WithName("GetCoordinates")
    .Produces<Coordinates>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .WithTags("Get");

app.MapPost("/coordinates", ([FromBody] List<Coordinates> coordinates) => 
    coordinates.Count() > 1 ? Results.Ok(CoordinatesRepository.GetTotalDistance(coordinates)) 
    : Results.Ok(new TotalDistance() { Metres = 0, Miles = 0 }))
    .WithName("PostTotalDistance")
    .Produces<TotalDistance>(StatusCodes.Status200OK)
    .WithTags("Post");

app.Run();

public partial class Program { }