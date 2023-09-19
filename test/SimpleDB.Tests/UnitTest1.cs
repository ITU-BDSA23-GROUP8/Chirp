namespace SimpleDB.Tests;

public class UnitTest1
{
    record Data(string data1, string data2, long data3);
    

    //test that we can read a CSV-file and get the correct amount of records
    [Fact]
    public void TestReadCSV1()
    {
        //arrange
        CSVDatabase<Data> database = CSVDatabase<Data>.GetInstance("../../../test_data.csv");

        //act
        var recordList = database.Read();

        //assert
        Assert.Equal<int>(2, recordList.Count());
    }


}

