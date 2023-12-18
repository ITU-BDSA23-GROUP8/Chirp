
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// Taken from lecture 5
public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public IntegrationTest(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        // Arrange
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        // Act
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void CanSeePrivateTimeline(string author)
    {
        // Arrange
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();

        // Act
        var content = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }

    [Fact]
    public async void DifferentPages()
    {
        // Arrange
        var page1 = await _client.GetAsync("/?page=1");
        var page2 = await _client.GetAsync("/?page=2");

        page1.EnsureSuccessStatusCode();
        page2.EnsureSuccessStatusCode();

        // Act
        var contentFromPage1 = await page1.Content.ReadAsStringAsync();
        var contentFromPage2 = await page2.Content.ReadAsStringAsync();

        // Assert
        Assert.DoesNotContain(contentFromPage1, contentFromPage2);
    }

    

    [Theory]
    [InlineData("Jacqualine Gilcoine", "Starbuck now is what we hear the worst.")]
    [InlineData("Mellie Yost", "But what was behind the barricade.")]
    protected async void CheckOwnCheepOnPrivateTimeline(string author, string cheep)
    {
        // Arrange
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();

        // Act
        var content = await response.Content.ReadAsStringAsync();

        // Arrange
        Assert.Contains(cheep, content);
    }
}

