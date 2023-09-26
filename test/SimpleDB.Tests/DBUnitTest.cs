using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;


namespace SimpleDB.Tests;

public class UnitTest
{
    [Fact]
    public async Task TestGetCheeps()
    {
        //arrange
        var baseURL = "http://localhost:5079";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
       
        

        //act
        var cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
        var httpRespons = await client.GetAsync("/cheeps");

        //assert
        Assert.True(cheeps.Any());
        Assert.Equal(HttpStatusCode.OK, httpRespons.StatusCode);
        
    }


}

