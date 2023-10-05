using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;

public class DBFacade
{
    private void CreateConnection(){
        var sqlDBFilePath = Path.GetTempPath() + "./chirp.db";
        
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
        
        var sqlQuery = @"SELECT username, text, pub_date FROM message JOIN user ON author_id = user_id ORDER by message.pub_date desc";

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
}