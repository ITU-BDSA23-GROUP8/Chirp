using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;

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
        .Select(c => new CheepDTO(c.Author.UserName, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
        .ToListAsync();

    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.UserName == user)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.UserName, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
        .ToListAsync();
    }

    public async Task CreateCheep(CheepDTO cheep, AuthorDTO author)
    {
        Author AuthorModel;
        if (!_context.Authors.Any(e => e.Email == author.Email))
        {
             AuthorModel = new Author
            {
                UserName = author.Name,
                Email = author.Email,
                Cheeps = new List<Cheep>(),
                Followers = new List<Author>(),
                Following = new List<Author>()
            };

            _context.Authors
            .Add(AuthorModel);
            await _context.SaveChangesAsync();
            
        }

        var CheepModel = new Cheep
        {
            Author = await _context.Authors.FirstAsync(c=>c.Email == author.Email),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.Timestamp)
        };

        _context.Cheeps
        .Add(CheepModel);
        await _context.SaveChangesAsync();
    }

}