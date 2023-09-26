using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;

namespace SimpleDB.Tests;

public class InegrationTest
{
    public record Cheep(string Author, string Message, long Timestamp);

    // test that database is incremented by 1 record, when storing a record
    [Fact]
    public async Task GetAndPost_IncrementDB(){
        //arrange
        var baseURL = "http://localhost:5079";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        
        //act
        var dataList_old = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");

        Cheep cheep = new("user", "message", 123456789);
        var content = JsonContent.Create(cheep);
        var response = await client.PostAsync("/cheep", content);

        var dataList_new = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
        
        //assert
        Assert.Equal(dataList_old.Count() + 1,dataList_new.Count());
    }
}