using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Chirp.Core;
using Chirp.Web;
namespace Chirp.Infrastructure.Tests;

public class LikeUnitTest
{
    [Fact]
    public async void TestLikeCheep()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new LikeRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        await repository.Like(new AuthorDTO("Helge", "ropf@itu.dk"), 1);
        await repository.Like(new AuthorDTO("Helge", "ropf@itu.dk"), 2);
        await repository.Like(new AuthorDTO("Rasmus", "rnie@itu.dk"), 1);

        var likecountCheep = await repository.LikeCount(1);
        var likecountAuthor = await repository.GetLikedCheeps(new AuthorDTO("Helge", "ropf@itu.dk"));

        //Assert 
        Assert.Equal(2, likecountCheep);
        Assert.Equal(2, likecountAuthor.Count);
    }

    [Fact]
    public async void TestGetLikedCheeps()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new LikeRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        await repository.Like(new AuthorDTO("Helge", "ropf@itu.dk"), 1);
        await repository.Like(new AuthorDTO("Helge", "ropf@itu.dk"), 2);
        await repository.Like(new AuthorDTO("Rasmus", "rnie@itu.dk"), 1);
        await repository.Like(new AuthorDTO("Rasmus", "rnie@itu.dk"), 2);

        var likedHelge = await repository.GetLikedCheeps(new AuthorDTO("Helge", "ropf@itu.dk"));
        var likedRasmus = await repository.GetLikedCheeps(new AuthorDTO("Rasmus", "rnie@itu.dk"));

        //Assert 
        Assert.Equal(likedHelge, likedRasmus);
    }

    [Fact]
    public async void TestUnlikeCheep()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using var context = new ChirpContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        var repository = new LikeRepository(context);

        DBInitializer.SeedDatabase(context);

        //Act
        await repository.Like(new AuthorDTO("Helge", "ropf@itu.dk"), 1);
        await repository.UnLike(new AuthorDTO("Helge", "ropf@itu.dk"), 1);

        var count = await repository.LikeCount(1);

        //Assert 
        Assert.Equal(0, count);
    }
}