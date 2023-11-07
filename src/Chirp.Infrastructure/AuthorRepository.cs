using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<AuthorDTO?> GetAuthorFromName(string user)
    {
        return await _context.Authors
        .Where(x => x.UserName == user)
        .Select(y => new AuthorDTO(y.UserName, y.Email))
        .FirstOrDefaultAsync();
    }


    public async Task<AuthorDTO?> GetAuthorFromEmail(string email)
    {
        return await _context.Authors
        .Where(x => x.Email == email)
        .Select(y => new AuthorDTO(y.UserName, y.Email))
        .FirstOrDefaultAsync();
    }

    public void CreateAuthor(AuthorDTO author)
    {
        if (!_context.Authors.Any(e => e.Email == author.Email))
        {
            var AuthorModel = new Author
            {
                UserName = author.Name,
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


}