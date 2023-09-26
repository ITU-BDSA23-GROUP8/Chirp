using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic;

namespace SimpleDB;


 public sealed class CSVDatabase<T> : IDatabaseRepository<T>{
    string path;
    private IEnumerable<T> records;

    // singleton static object
    private static CSVDatabase<T> instance = null;

    // an object to use in a singleton class
    private List<CSVDatabase<T>> database = null;

    // Restrict to create object of singleton class
        
    private CSVDatabase(string path){
        if (database == null) {
            this.path=path; 
        }
    }

    // The static method to provide global access to the singleton object
        // Get singleton object of SingletonEmployeeService class
    public static CSVDatabase<T> GetInstance(string path)
        {
            if (instance == null)
            {
                // Thread safe singleton
                lock (typeof(CSVDatabase<T>))
                {
                    instance = new CSVDatabase<T>(path);
                }
            }
            return instance;
        }


    public IEnumerable<T> Read(int? limit = null){
        
        using (var sr = new StreamReader(path))
        using(var csv = new CsvReader(sr, CultureInfo.InvariantCulture)) {
            records = csv.GetRecords<T>().ToList(); 
                
        }
        return records;
    }

    public void Store(T record){
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, 
            }; 

            using (var stream = File.Open(path, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                
            csv.WriteRecord(record); 
            csv.NextRecord();
            }

    }

}