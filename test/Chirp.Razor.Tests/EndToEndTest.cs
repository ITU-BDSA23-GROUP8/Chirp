using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace SimpleDB.Tests;



/*
//Additionally, you want to test that calling chirp cheep "Hello!!!" stores the respective values in the database.
public class EndToEndWrite
{
  record Cheep(string Author, string Message, long Timestamp);

  [Fact]

  public async Task testEndtoEnd()
  {

    //Act 
    var baseURL = "http://localhost:5079";
    using HttpClient client = new();
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.BaseAddress = new Uri(baseURL);

    //Assembly
    Cheep cheep = new("user", "Hello endTest", 123456789);
    var content = JsonContent.Create(cheep);
    var response = await client.PostAsync("/cheep", content);
    
    var list = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
    var msg = list.Last().Message;

    //Asses
    Assert.Equal("Hello endTest", msg);

  }




}
*/