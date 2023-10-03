using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.GetInstance("chirp_cli_db.csv");

app.MapGet("/cheeps", () =>
{
    return database.Read();

});

app.MapPost("/cheep", (Cheep cheep) =>
{
    database.Store(cheep);

});

app.Run();

public record Cheep(string Author, string Message, long Timestamp);

public partial class Program { }



