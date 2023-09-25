using System.Collections.Generic;
using System.ComponentModel.Design;
using static Program;

public static class UserInterface
{

    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var item in cheeps)
        {
            Console.WriteLine($"{item.Author} @ {times(item.Timestamp).ToString("MM/dd/yy HH:mm:ss")}: {item.Message}");
        }
    }

    public static DateTimeOffset times(long unixtime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixtime).LocalDateTime;

    }

}