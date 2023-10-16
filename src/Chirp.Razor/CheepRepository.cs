using Microsoft.EntityFrameworkCore;

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

}