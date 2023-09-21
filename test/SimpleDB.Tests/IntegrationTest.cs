namespace SimpleDB.Tests;

public class InegrationTest
{
    record Data(string data1, string data2, long data3);
    CSVDatabase<Data> database;

    // test that database is incremented by 1 record, when storing a record
    [Fact]
    public void StoreAndRead(){
        //arrange
        database = CSVDatabase<Data>.GetInstance("../../../test_data.csv");
        
        //act
        var dataList_old = database.Read();

        Data data = new("user", "message", 123456789);
        database.Store(data);

        var dataList_new = database.Read();
        
        //assert
        Assert.Equal<int>(dataList_old.Count() + 1,dataList_new.Count());
    }
}