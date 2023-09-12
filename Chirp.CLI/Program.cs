using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.CommandLine;

using SimpleDB;



class Program {
     
    
    public static int Main(string[] args)
    {
        var readOption = new Option<bool>(
                    name: "--read",
                    description: "Prints the cheeps stored in the database currently"
                );
        var cheepOption = new Option<string>(
                    name: "--cheep",
                    description: "Stores the string in the database, along with the author name and timestamp for cheep"
                );
        var rootCommand = new RootCommand("Chirp is a social media program"){
            readOption,
            cheepOption
        };
            rootCommand.SetHandler((cheep)=> 
                {
                    handleCommand(cheep);
                },
                readOption
            ); return rootCommand.Invoke(args);
        
        /*
        CSVDatabase<Cheep> database = new("chirp_cli_db.csv");
        if(args[0].Equals("read")){
            
            try
            {
                // Open the text file using a stream reader.
                var cheeps = database.Read();
                UserInterface.PrintCheeps(cheeps);
            
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

        }*/
      
    }

    public static void handleCommand(string cheep){
        Console.WriteLine("hello");
    }

    public record Cheep(string Author, string Message, long Timestamp);
}