using System.CommandLine;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using SimpleDB;

public class Program
{

    static async Task<int> Main(string[] args)
    {
        var readOption = new Option<bool>(
            name: "read",
            description: "Read cheeps.");
        readOption.AddAlias("-r");

        var cheepOption = new Option<string>(
            name: "cheep",
            description: "Write a cheep.");
        cheepOption.AddAlias("-c");

        var rootCommand = new RootCommand("Chirp: write and read cheeps!");

        rootCommand.AddOption(readOption);
        rootCommand.AddOption(cheepOption);

        rootCommand.SetHandler(async (read, cheepMsg) =>
        {
            if (read)
            {
                await HandleReadAsync();
            }
            else if (cheepMsg != null)
            {
                await HandleCheep(cheepMsg);
            }
        },
        readOption, cheepOption);

        return await rootCommand.InvokeAsync(args);

    }

    static async Task HandleReadAsync()
    {
        var baseURL = "http://localhost:5079";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
       
        var cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");

        UserInterface.PrintCheeps(cheeps);

    }

    static async Task HandleCheep(string message)
    {
        var baseURL = "http://localhost:5079";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);


        DateTimeOffset localTime = DateTimeOffset.Now;
        Cheep cheep = new(Environment.UserName, message, localTime.ToUnixTimeSeconds());

        var content = JsonContent.Create(cheep);
        var response = await client.PostAsync("/cheep", content);
    }

    public record Cheep(string Author, string Message, long Timestamp);
}