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
        var repository = new CheepRepository(context);

        //Act
        var author = new AuthorDTO("Aladdin", "abu@gmail.com");
        repository.CreateAuthor(author);
        var createdAuthor = await context.Authors.SingleOrDefaultAsync(c=> c.Name == "Aladdin");

        //Assert 
        // Console.WriteLine(createdAuthor.Name);
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
        var repository = new CheepRepository(context);

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
        var repository = new CheepRepository(context);

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
        var repository = new CheepRepository(context);

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
        var createdAuthor = await context.Authors.FirstOrDefaultAsync(c=> c.Name == "Gaston");

        //Assert 
        Assert.NotNull(createdAuthor);
       Assert.Contains(context.Cheeps, ch => ch.Text == "I <3 Belle");
    }

}