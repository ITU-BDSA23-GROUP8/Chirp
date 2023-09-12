using System;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;


class Program
{
    
    public static void Main(string[] args)
    {
        if(args[0].Equals("read")){
            
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader("chirp_cli_db.csv"))
                using(var csv = new CsvReader(sr, CultureInfo.InvariantCulture)) {
                    var records = csv.GetRecords<Cheep>(); 
                    foreach (var item in records)
                {
                    var time = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).LocalDateTime;
                    Console.WriteLine($"{item.Author} @ {time.ToString("MM/dd/yy HH:mm:ss")}: {item.Message}");
                }
                
                }
                
            }
            
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        } else if(args[0].Equals("cheep")){
            DateTimeOffset localTime = DateTimeOffset.Now;
            
            Cheep cheep = new(Environment.UserName, args[1], localTime.ToUnixTimeSeconds());
            
           
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, 
            }; 

            using (var stream = File.Open("chirp_cli_db.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                
                csv.WriteRecord(cheep); 
                csv.NextRecord();
            }

        }
      
    }

    public record Cheep(string Author, string Message, long Timestamp);
}