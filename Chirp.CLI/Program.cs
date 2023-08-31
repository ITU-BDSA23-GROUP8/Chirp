using System;
using System.IO;
using System.Net.Security;
using System.Text.RegularExpressions;

class Program
{
    
    public static void Main()
    {
      List<Cheep> cheeps = new List<Cheep>();  
        try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader("chirp_cli_db.csv"))
            {
                string cheep; 
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    cheep = sr.ReadLine();
                    var values = Regex.Split(cheep, ",(?=(?:[^']*'[^']*')*[^']*$)");
                    cheeps.Add(new Cheep(values[0], values[1], DateTimeOffset.Parse(values[2])));
                }

                // Read the stream as a string, and write the string to the console.
                foreach (var item in cheeps)
                {
                   Console.WriteLine(item);
                }
               
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}