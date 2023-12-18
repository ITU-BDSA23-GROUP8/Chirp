using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.VisualBasic;

#nullable disable

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page, int offset)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
        .ToListAsync();

    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset)
    {


        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.UserName == user)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
        .ToListAsync();
    }

    public async Task<IEnumerable<CheepDTO>> GetAllCheepsFromAuthor(string user)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.UserName == user)
        .Select(c => new CheepDTO(c.Author.UserName, c.Author.Email, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
        .ToListAsync();
    }


    public async Task<IEnumerable<CheepDTO>> GetCheepsFromFollowing(string user, int page, int offset)
    {

        var ownCheeps = await GetAllCheepsFromAuthor(user);
        var finalList = ownCheeps.ToList();

        var followingList = await _context.Authors
        .Where(x => x.Followers.Any(y => y.UserName == user))
        .ToListAsync();

        foreach (var author in followingList)
        {
            //var cheeplist = await GetCheepsFromAuthor(user, page, offset);
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