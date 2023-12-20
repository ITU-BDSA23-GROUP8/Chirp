using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Chirp.Core;
using Chirp.Web;

#nullable disable

namespace Chirp.Infrastructure.Tests;

public class CheepUnitTest{
    
    [Fact]
    public async void TestCreateCheepWithoutAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new CheepRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var author = new AuthorDTO("Gaston", "belle@gmail.com");
        var cheep = new CheepDTO("Gaston", "belle@gmail.com", "I <3 Belle", "2023-08-01 13:14:44", -1, 0);
        await repository.CreateCheep(cheep, author);
        var authorCount = context.Authors.Where(c => c.Email == "belle@gmail.com").Count();

        //Assert 
        Assert.Contains(context.Cheeps, ch => ch.Text == "I <3 Belle");
        Assert.Equal(1, authorCount);
    }

    [Fact]
    public async void TestCreateCheepWithAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new CheepRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var author = new AuthorDTO("Helge", "ropf@itu.dk");
        var cheep = new CheepDTO("Helge","ropf@itu.dk", "Honey cookie", "2023-08-01 13:14:45", -1, 0);
        await repository.CreateCheep(cheep, author);
        var authorCount = context.Authors.Where(c => c.Email == "ropf@itu.dk").Count();

        //Assert 
        Assert.Contains(context.Cheeps, ch => ch.Text == "Honey cookie");
        Assert.Equal(1, authorCount);
    }

    [Fact]
    public async void TestGetCheepsFromCertainPage()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new CheepRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var list = await repository.GetCheeps(2, 32);
        var cheep = list.Any(c => c.Message == "It is asking much of it in the world.");

        //Assert 
        Assert.True(cheep);
    }

    [Fact]
    public async void TestGetCheepsFromCertainPageAndAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new CheepRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var list = await repository.GetCheepsFromAuthor("Jacqualine Gilcoine", 2, 32);
        var cheep = list.Any(c => c.Message == "What a relief it was the place examined.");

        //Assert 
        Assert.True(cheep);
    }

}