namespace PlaywrightTests;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Threading.Tasks;


[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{

    [Test]
    public async Task LoginWriteCheepLogout()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup8chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("JuliaKlompmaker");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").FillAsync("Gfa82mqs");

        await page.GetByLabel("Password").PressAsync("Enter");

        await page.GetByRole(AriaRole.Textbox).ClickAsync();

        await page.GetByRole(AriaRole.Textbox).FillAsync("Hej, dette er en test");

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "logout" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();

    }

    [Test]
     public async Task FollowAndUnfollow()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup8chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("JuliaKlompmaker");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").FillAsync("Gfa82mqs");

        await page.GetByLabel("Password").PressAsync("Enter");

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 2023-08-01 13:17:" }).GetByRole(AriaRole.Button).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Unfollow Starbuck now is what we hear the worst. — 2023-08-01 13:17:" }).GetByRole(AriaRole.Button).ClickAsync();

    }

    [Test]
    public async Task LoginAndForgetMe()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup8chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("JuliaKlompmaker");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").FillAsync("Gfa82mqs");

        await page.GetByLabel("Password").PressAsync("Enter");

        await page.GotoAsync("https://bdsagroup8chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "JuliaKlompmaker's profile" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 2023-08-01 13:17:" }).GetByRole(AriaRole.Button).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "JuliaKlompmaker's profile" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Forget Me!" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "JuliaKlompmaker's profile" }).ClickAsync();

    }

}



