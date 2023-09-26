using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;


namespace SimpleDB.Tests;

public class DBUnitTest
{
    public record Cheep(string Author, string Message, long Timestamp);

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

    [Fact]
    public async Task TestPostCheeps()
    {
        //arrange
        var baseURL = "http://localhost:5079";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        //act
        Cheep cheep = new("test", "test msg", 123456789);
        var content = JsonContent.Create(cheep);
        var response = await client.PostAsync("/cheep", content);

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

}

