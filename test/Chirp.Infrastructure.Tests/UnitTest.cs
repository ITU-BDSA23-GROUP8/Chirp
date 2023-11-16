using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Chirp.Core;
using Chirp.Web;
namespace Chirp.Infrastructure.Tests;

public class UnitTest
{
    [Fact]
    public async void TestCreateAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new AuthorRepository(context);

        //Act
        var author = new AuthorDTO("Aladdin", "abu@gmail.com");
        repository.CreateAuthor(author);
        var createdAuthor = await context.Authors.SingleOrDefaultAsync(c=> c.UserName == "Aladdin");

        //Assert 
        Assert.NotNull(createdAuthor);
    }

     [Fact]
    public async void TestGetAuthorFromName()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new AuthorRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var createdAuthor = await repository.GetAuthorFromName("Helge");

        //Assert 
        Assert.NotNull(createdAuthor);
    }

    [Fact]
    public async void TestGetAuthorFromEmail()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new AuthorRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var createdAuthor = await repository.GetAuthorFromEmail("ropf@itu.dk");

        //Assert 
        Assert.NotNull(createdAuthor);
    }
    [Fact]
     public async void TestDoubleCreatedAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new AuthorRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        var author = new AuthorDTO("Helge", "ropf@itu.dk");

        //Assert 
       var exception = Assert.Throws<ArgumentException>(() => repository.CreateAuthor(author));
       Assert.Equal("Author already exists",exception.Message);
    }

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
        var cheep = new CheepDTO("Gaston", "I <3 Belle", "2023-08-01 13:14:44");
        repository.CreateCheep(cheep, author);
        var authorCount = context.Authors.Where(c=> c.Email == "belle@gmail.com").Count();
        
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
        var cheep = new CheepDTO("Helge", "Honey cookie", "2023-08-01 13:14:45");
        repository.CreateCheep(cheep, author);
        var authorCount = context.Authors.Where(c=> c.Email == "ropf@itu.dk").Count();
        
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
        var list = await repository.GetCheeps(2,32);
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