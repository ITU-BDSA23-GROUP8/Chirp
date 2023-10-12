using Chirp.Core;
using Microsoft
namespace EFDemo;

public class CheepRepository : ICheepRepository
{
    private radonly ChirpContext _context; 

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }



    public async IEnumerable<CheepDTO> GetCheeps(int pageSize = 32, int page = 0) =>
     {
       await _context.Cheeps
       .Skip(page* pageSize)
       .Take(pagesize)
       .Select(c => new CheepDTO(c.Message, c.Author.Name, c.PubDate ))
       .toListAsync()

        public async Task Add(CheepDTO cheep)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Name ==

            if (author is null)
            {
            author = new Author(Name = cheep.Author)
            })
          
            var cheep = new Cheep{
            Author = cheep.Author
            Message = cheep.Message
            PubDate = cheep.PubDate
            };

        }

    }

}