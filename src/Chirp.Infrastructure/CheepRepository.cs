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
        .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString()))
        .ToListAsync();

    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset)
    {
        return await _context.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.Name == user)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString()))
        .ToListAsync();
    }

    public async void CreateCheep(CheepDTO cheep, AuthorDTO author)
    {
        Author AuthorModel;
        if (!_context.Authors.Any(e => e.Email == author.Email))
        {
             AuthorModel = new Author
            {
                Name = author.Name,
                Email = author.Email,
                Cheeps = new List<Cheep>()
            };

            _context.Authors
            .Add(AuthorModel);
            _context.SaveChanges();
            
        }

        var CheepModel = new Cheep
        {
            Author = await _context.Authors.FirstOrDefaultAsync(c=>c.Email == author.Email),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.Timestamp)
        };

        _context.Cheeps
        .Add(CheepModel);
        _context.SaveChanges();
    }

}