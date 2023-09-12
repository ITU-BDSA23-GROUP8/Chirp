using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic;

namespace SimpleDB;


 public sealed class CSVDatabase<T> : IDatabaseRepository<T>{
    string path;
    private IEnumerable<T> records;

    public CSVDatabase(string path){
        this.path=path;
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