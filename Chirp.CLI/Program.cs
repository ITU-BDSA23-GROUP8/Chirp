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
     
    
    static async Task<int> Main(string[] args)
    {
        var readOption = new Option<bool>(
            name: "--read",
            description: "Read cheeps.");
        readOption.AddAlias("-r");

        var cheepOption = new Option<string>(
            name: "--cheep",
            description: "Write a cheep.");
        readOption.AddAlias("-c");

        var rootCommand = new RootCommand("Chirp: write and read cheeps!");
        
        rootCommand.AddOption(readOption);
        rootCommand.AddOption(cheepOption);

        rootCommand.SetHandler((read, cheepMsg) =>
        {
            if(read){
                 HandleRead();
            }
            else if(cheepMsg != null){
                HandleCheep(cheepMsg);
            }
        },
        readOption, cheepOption);

        return await rootCommand.InvokeAsync(args);
      
    }

    static void HandleRead(){
        CSVDatabase<Cheep> database = new("chirp_cli_db.csv");
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
    }

    static void HandleCheep(string message){
        CSVDatabase<Cheep> database = new("chirp_cli_db.csv");
        DateTimeOffset localTime = DateTimeOffset.Now;
            
            Cheep cheep = new(Environment.UserName, message, localTime.ToUnixTimeSeconds());
            
            database.Store(cheep);
    }

public record Cheep(string Author, string Message, long Timestamp);
}