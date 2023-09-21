namespace SimpleDB.Tests;

public class UnitTest1
{
    record Data(string data1, string data2, long data3);
    CSVDatabase<Data> database;
    

    //test that we can read a CSV-file
    [Fact]
    public void TestReadCSV()
    {
        //arrange
        database = CSVDatabase<Data>.GetInstance("../../../test_data.csv");
        
        //act
        var recordList = database.Read();

        //assert
        Assert.True(recordList.Any()); // check that database contains something 
    }


}

