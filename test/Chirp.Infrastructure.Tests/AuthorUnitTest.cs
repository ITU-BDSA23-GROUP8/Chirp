using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Chirp.Core;
using Chirp.Web;

#nullable disable

namespace Chirp.Infrastructure.Tests;

public class AuthorUnitTest
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
        var createdAuthor = await context.Authors.SingleOrDefaultAsync(c => c.UserName == "Aladdin");

        //Assert 
        Assert.NotNull(createdAuthor);
    }
    
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
        Assert.Equal("Author already exists", exception.Message);
    }


    [Fact]
    public async void TestFollow()
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
        var follower = new AuthorDTO("Rasmus", "rnie@itu.dk");

        await repository.Follow(author, follower);

        var authorModel = context.Authors.FirstOrDefault(a => a.Email == author.Email);

        //Assert 
        Assert.Contains(authorModel.Followers, f => f.Email == follower.Email);

    }

    [Fact]
    public async void TestUnFollow()
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
        var follower = new AuthorDTO("Rasmus", "rnie@itu.dk");

        await repository.Follow(author, follower);
        await repository.UnFollow(author, follower);

        var authorModel = context.Authors.FirstOrDefault(a => a.Email == author.Email);

        //Assert 
        Assert.DoesNotContain(authorModel.Followers, f => f.Email == follower.Email);

    }

    [Fact]
    public async void TestGetFollowing()
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
        var per = new Author
        {
            UserName = "Per",
            Email = "postmand@mail.com",
            Cheeps = new List<Cheep>()
        };

        var jens = new Author
        {
            UserName = "Jens",
            Email = "vejmand@mail.com",
            Cheeps = new List<Cheep>()
        };

        context.Authors.Add(per);
        context.Authors.Add(jens);

        per.Followers.Add(jens);
        jens.Following.Add(per);

        context.SaveChanges();

        var list = await repository.GetFollowing(new AuthorDTO(jens.UserName, jens.Email));

        //Assert 
        Assert.Contains(list, p => p.Email == per.Email);
    }



}