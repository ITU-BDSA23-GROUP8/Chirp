using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
namespace SimpleDB;


class Program {
    
    
    public static void Main(string[] args)
    {
        CSVDatabase<Cheep> database = new("chirp_cli_db.csv");
        if(args[0].Equals("read")){
            
            try
            {
                
                var cheeps = database.Read();
               
                foreach (var item in cheeps)
                {
                    var time = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).LocalDateTime;
                    Console.WriteLine($"{item.Author} @ {time.ToString("MM/dd/yy HH:mm:ss")}: {item.Message}");
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
            
            database.Store(cheep);

        }
      
    }

    public record Cheep(string Author, string Message, long Timestamp);
}