using System.Data;
using Microsoft.Data.Sqlite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<CheepViewModel> GetCheeps(int page)
    {
        List<CheepViewModel> finalList;
        int amountOfCheeps = (page == 1 ? 0 : 32 * (page-1));
        var sqlDBFilePath = "/tmp/chirp.db";
        var sqlQuery = $@"SELECT username, text, pub_date 
                        FROM message 
                        JOIN user ON author_id = user_id 
                        ORDER by message.pub_date desc
                        LIMIT 32
                        OFFSET {amountOfCheeps}";

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();
            

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            finalList = new List<CheepViewModel>();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                string author = (string)dataRecord[0];
                string message = (string)dataRecord[1];
                string timestamp = UnixTimeStampToDateTimeString(Convert.ToDouble(dataRecord[2]));
                finalList.Add(new CheepViewModel(author,message,timestamp));

            }
        }

        return finalList;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string user, int page)
    {

        List<CheepViewModel> finalList;
        int amountOfCheeps = (page == 1 ? 0 : 32 * (page-1));
        var sqlDBFilePath = "/tmp/chirp.db";
        var sqlQuery = $@"SELECT username, text, pub_date 
                        FROM message 
                        JOIN user ON author_id = user_id 
                        WHERE username = @user 
                        ORDER by message.pub_date desc
                        LIMIT 32
                        OFFSET {amountOfCheeps}";

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();
            

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@user", user);

            using var reader = command.ExecuteReader();
            finalList = new List<CheepViewModel>();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                string author = (string)dataRecord[0];
                string message = (string)dataRecord[1];
                string timestamp = UnixTimeStampToDateTimeString(Convert.ToDouble(dataRecord[2]));
                finalList.Add(new CheepViewModel(author,message,timestamp));

            }
        }

        return finalList;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
