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

    public async Task<AuthorDTO?> GetAuthorFromName(string user)
    {
        return await _context.Authors
        .Where(x => x.Name == user)
        .Select(y => new AuthorDTO(y.Name, y.Email))
        .FirstOrDefaultAsync();
    }


    public async Task<AuthorDTO?> GetAuthorFromEmail(string email)
    {
        return await _context.Authors
        .Where(x => x.Email == email)
        .Select(y => new AuthorDTO(y.Name, y.Email))
        .FirstOrDefaultAsync();
    }

    public async void CreateAuthor(AuthorDTO author)
    {
        if (await GetAuthorFromEmail(author.Email) == null)
        {
            var AuthorModel = new Author
            {
                Name = author.Name,
                Email = author.Email,
                Cheeps = new List<Cheep>()
            };

            _context.Authors
            .Add(AuthorModel);
            _context.SaveChanges();
        } else {
            throw new ArgumentException("Author already exists");
        }
    }

    public void CreateCheep(CheepDTO cheep, AuthorDTO author)
    {
        if (GetAuthorFromName(author.Name) == null)
        {
            CreateAuthor(new AuthorDTO(author.Name, author.Email));
        }
        var AuthorModel = new Author
        {
            Name = author.Name,
            Email = author.Email,
            Cheeps = new List<Cheep>()
        };

        var CheepModel = new Cheep
        {
            Author = AuthorModel,
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.Timestamp)
        };

        _context.Cheeps
        .Add(CheepModel);
        _context.SaveChanges();
    }

}