using System.Collections.Generic;
using System.ComponentModel.Design;
using static Program;

static class UserInterface{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps){
        foreach (var item in cheeps)
            {
            var time = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).LocalDateTime;
            Console.WriteLine($"{item.Author} @ {time.ToString("MM/dd/yy HH:mm:ss")}: {item.Message}");
            }
    }
}