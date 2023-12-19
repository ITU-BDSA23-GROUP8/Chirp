using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;

#nullable disable

/// <summary>
/// The class 'CheepRepository' inherits from the interface 'ICheepRepository'. 
/// This repository is responsible for interacting with the database, to retrieve data about the Cheeps. 
/// 
/// This class has methods such as 'GetCheeps', 'GetCheepsFromAuthor', 'CreateCheep' and other methods,
/// which provides functionality for a Cheep. 
/// </summary>

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    // The method 'GetCheeps' loads the cheeps from a specific page.  
    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page, int offset)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), c.Id, c.Likes.Count()))
        .ToListAsync();

    }

    // The method 'GetCheepsFromAuthor' loads all cheeps from a certain Author, from a certain page.  
    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.UserName == user)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), c.Id, c.Likes.Count()))
        .ToListAsync();
    }

    // The method 'GetAllCheepsFromAuthor' loads all cheeps from a certain Author.
    public async Task<IEnumerable<CheepDTO>> GetAllCheepsFromAuthor(string user)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.UserName == user)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), c.Id, c.Likes.Count()))
        .ToListAsync();
    }

    // The method 'GetCheepsFromFollowing' loads all cheeps from from oneself, and all the Authors that you follow. 
    // These cheeps is then ordered by descending on 'Timestamp', so the user sees the newest cheeps. 
    public async Task<IEnumerable<CheepDTO>> GetCheepsFromFollowing(string user, int page, int offset)
    {

        var ownCheeps = await GetAllCheepsFromAuthor(user);
        var finalList = ownCheeps.ToList();

        var followingList = await _context.Authors
        .Where(x => x.Followers.Any(y => y.UserName == user))
        .ToListAsync();

        foreach (var author in followingList)
        {
            var followingCheeps = await GetAllCheepsFromAuthor(author.UserName!);
            var cheeplist = followingCheeps.ToList();
            finalList.AddRange(cheeplist);
        }

        return finalList
        .OrderByDescending(d => d.Timestamp)
        .Skip(offset)
        .Take(32);
    }


    public async Task CreateCheep(CheepDTO cheep, AuthorDTO author)
    {
        Author AuthorModel;
        if (!_context.Authors.Any(e => e.UserName == author.Name))
        {
            AuthorModel = new Author
            {
                UserName = author.Name,
                Email = author.Email,
                Cheeps = new List<Cheep>()
            };

            _context.Authors
            .Add(AuthorModel);
            await _context.SaveChangesAsync();

        }

        var CheepModel = new Cheep
        {
            Author = await _context.Authors.FirstAsync(c => c.UserName == author.Name),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.Timestamp)
        };

        _context.Cheeps
        .Add(CheepModel);
        await _context.SaveChangesAsync();
    }

}