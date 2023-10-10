global using Xunit;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Chirp.Razor.Tests;



public class UnitTest : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> factory;


    public UnitTest(WebApplicationFactory<Program> _factory)
    {
        factory = _factory;
    }

    [Fact]
    public async Task GetAPIResponseCode()
    {
        // Arrange
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });


        // Act 
        var response = await client.GetAsync("/");


        // Assert 

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]

    public async void PrivateGETApiTest()
    {
        // Arrange
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // Act 
        var response = await client.GetAsync("/Helge");
        var content = await response.Content.ReadAsStringAsync();

        // Assert 
        Assert.Contains("Hello, BDSA students!", content);


    }
    public void PublicGETApiTestString()
    {
        // Arrange
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // Act 
        var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert 
        Assert.Contains("Starbuck now is what we hear the worst", content);
    }




}