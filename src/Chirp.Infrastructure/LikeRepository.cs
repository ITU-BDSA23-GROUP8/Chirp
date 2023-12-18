using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;

public class LikeRepository : ILikeRepository
{
    private readonly ChirpContext _context;

    public LikeRepository(ChirpContext context)
    {
        _context = context;
    }


    public async Task<List<CheepDTO>> GetLikedCheeps(AuthorDTO author)
    {
        
        var authorModel = await _context.Authors.Include(a => a.Likes).FirstOrDefaultAsync(a => a.UserName == author.Name);

        var list = await _context.Cheeps.Include(x => x.Author).Where(x => x.Likes.Any(y => y.AuthorId == authorModel.Id)).ToListAsync();

        //var likes = _context.Likes.Where(a => a.AuthorId == authorModel.Id).Select(x => new LikeDTO(x.CheepId, x.AuthorId)).ToList();

        var cheeps = new List<CheepDTO>();

        foreach (var cheep in list)
        {
            cheeps.Add(new CheepDTO(cheep.Author.UserName, cheep.Author.Email, cheep.Text, cheep.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), cheep.Id, cheep.Likes.Count()));
        }

        return cheeps;

        
    }

    public async Task<int> likeCount(int cheepID){
        var cheepModel = await _context.Cheeps.Include(a => a.Likes).FirstOrDefaultAsync(c => c.Id == cheepID);

        return cheepModel.Likes.Count();
    }

    public async Task<bool> AuthorHasLiked(AuthorDTO author, int cheepID){
        var authorModel = await _context.Authors.Include(a => a.Likes).FirstOrDefaultAsync(a => a.UserName == author.Name);
        var cheepModel = await _context.Cheeps.Include(a => a.Likes).FirstOrDefaultAsync(c => c.Id == cheepID);

        var likeModel = new Like
        {
            Author = authorModel!,
            AuthorId = authorModel!.Id,
            Cheep = cheepModel!,
            CheepId = cheepModel!.Id
        };

        return _context.Likes.Contains(likeModel);
    }


    public async Task ToggleLike(AuthorDTO author, int cheepID)
    {
        var authorModel = await _context.Authors.FirstOrDefaultAsync(a => a.UserName == author.Name);
        var cheepModel = await _context.Cheeps.FirstOrDefaultAsync(c => c.Id == cheepID);

        var likeModel = new Like
        {
            Author = authorModel!,
            AuthorId = authorModel!.Id,
            Cheep = cheepModel!,
            CheepId = cheepModel!.Id
        };
        
        if (_context.Likes.Contains(likeModel))
        {
            authorModel.Likes.Remove(likeModel);
            cheepModel.Likes.Remove(likeModel);
            _context.Likes.Remove(likeModel);

        }
        else
        {
            _context.Likes.Add(likeModel);
            authorModel.Likes.Add(likeModel);
            cheepModel.Likes.Add(likeModel);

        }

        await _context.SaveChangesAsync();
    }


}