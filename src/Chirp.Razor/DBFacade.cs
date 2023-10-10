using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;

public class DBFacade
{
    string sqlDBFilePath = Path.GetTempPath() + "./chirp.db";

    public DBFacade(){
        CreateConnection();
    }

    private void CreateConnection(){
        string schemaScript;
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        using (Stream s = thisAssembly.GetManifestResourceStream("Chirp.Razor.data.schema.sql"))
        {
            using (StreamReader sr = new StreamReader(s))
            {
                schemaScript = sr.ReadToEnd();
            }
        }
        string dumpScript;
        using (Stream s = thisAssembly.GetManifestResourceStream("Chirp.Razor.data.dump.sql"))
        {
            using (StreamReader sr = new StreamReader(s))
            {
                dumpScript = sr.ReadToEnd();
            }
        }
        
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();
            
            var schemaCommand = connection.CreateCommand();
            schemaCommand.CommandText = schemaScript;
            schemaCommand.ExecuteNonQuery();

            var dumpCommand = connection.CreateCommand();
            dumpCommand.CommandText = dumpScript;
            dumpCommand.ExecuteNonQuery();
        }
    }

    private List<CheepViewModel> ReadQueryResult(SqliteCommand queryCommand){
        var finalList = new List<CheepViewModel>();
        using var reader = queryCommand.ExecuteReader();
            
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                string author = (string)dataRecord[0];
                string message = (string)dataRecord[1];
                string timestamp = UnixTimeStampToDateTimeString(Convert.ToDouble(dataRecord[2]));
                finalList.Add(new CheepViewModel(author, message, timestamp));

            }
        
        return finalList;
    }

    public List<CheepViewModel> GetCheeps(string query)
    {   
        
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var queryCommand = connection.CreateCommand();
            queryCommand.CommandText = query;

            return ReadQueryResult(queryCommand);
        }
    }

  

    public List<CheepViewModel> GetCheepsFromAuthor(string query, string user)
    {
        List<CheepViewModel> finalList;
                        
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();


            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@user", user);


            return ReadQueryResult(command);

        }
    }
     private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


}