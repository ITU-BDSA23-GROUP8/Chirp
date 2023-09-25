using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;




var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.MapGet("/cheeps", () =>
{
    using (var sr = new StreamReader("../..//data/chirp_cli_db.csv"))
    using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
    {
        return csv.GetRecords<Cheep>().ToList();
    }

});

app.MapPost("/cheep", (Cheep cheep) =>
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, 
            }; 

            using (var stream = File.Open("../..//data/chirp_cli_db.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                
            csv.WriteRecord(cheep); 
            csv.NextRecord();
            }

});



app.Run();

public record Cheep(string Author, string Message, long Timestamp);



