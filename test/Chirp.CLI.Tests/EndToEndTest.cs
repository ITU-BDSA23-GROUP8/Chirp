using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace SimpleDB.Tests;






//Additionally, you want to test that calling chirp cheep "Hello!!!" stores the respective values in the database.
public class EndToEndWrite
{
  record Data(string data1, string data2, long data3);
  CSVDatabase<Data> database;

  [Fact]

  public void testEndtoEnd()
  {

    //Act 
    database = CSVDatabase<Data>.GetInstance("../../../test_data.csv");

    //Assembly
    Data data = new("user", "Hello endTest", 123456789);
    database.Store(data);
    var msg = database.Read().Last().data2;

    //Asses
    Assert.Equal("Hello endTest", msg);

  }




}