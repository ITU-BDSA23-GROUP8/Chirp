using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;

/// <summary>
/// The class 'LikeRepository' inherits from the interface 'ILikeRepository'. 
/// This repository is responsible for interacting 
/// with the databse, to retieve data about Likes. 
/// 
/// This class has methods such as 'GetLikedCheeps', 'Like', 'UnLike' and other methods, 
/// which are used when interacting with Likes. 
/// </summary>

public class LikeRepository : ILikeRepository
{
    private readonly ChirpContext _context;

    public LikeRepository(ChirpContext context)
    {
        _context = context;
    }


    //The method 'GetLikedCheeps' loads all the Cheeps that a certain 'Author' have liked. 
    public async Task<List<CheepDTO>> GetLikedCheeps(AuthorDTO author)
    {

        var authorModel = await _context.Authors.Include(a => a.Likes).FirstOrDefaultAsync(a => a.UserName == author.Name);

        var list = await _context.Cheeps.Include(x => x.Author).Where(x => x.Likes.Any(y => y.AuthorId == authorModel!.Id)).ToListAsync();

        var cheeps = new List<CheepDTO>();

        foreach (var cheep in list)
        {
            cheeps.Add(new CheepDTO(cheep.Author.UserName!, cheep.Author.Email!, cheep.Text, cheep.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), cheep.Id, cheep.Likes.Count()));
        }

        return cheeps;

    }

    //The method 'LikeCount' counts all the Likes from a certain Cheep.
    public async Task<int> LikeCount(int cheepID)
    {
        var cheepModel = await _context.Cheeps.FirstOrDefaultAsync(c => c.Id == cheepID);

        return cheepModel!.Likes.Count();
    }


    // If the 'Like' doesn't already exist on that Cheep from the Author, who has liked it,
    // then it creates a new Like on that Cheep. 

    public async Task Like(AuthorDTO author, int cheepID)
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

        if (!_context.Likes.Contains(likeModel))
        {
            _context.Likes.Add(likeModel);
        }


        await _context.SaveChangesAsync();
    }

    // If the 'Like' exist on that Cheep from the Author, who has liked it,
    // then it removes their [Like on that Cheep. 
    public async Task UnLike(AuthorDTO author, int cheepID)
    {
        var authorModel = await _context.Authors.FirstOrDefaultAsync(a => a.UserName == author.Name);
        var cheepModel = await _context.Cheeps.FirstOrDefaultAsync(c => c.Id == cheepID);

        var likeModel = await _context.Likes.FirstOrDefaultAsync(a => a.CheepId == cheepModel!.Id && a.AuthorId == authorModel!.Id);

        if (_context.Likes.Contains(likeModel))
        {
            authorModel!.Likes.Remove(likeModel!);
            cheepModel!.Likes.Remove(likeModel!);
            _context.Likes.Remove(likeModel!);


        }
        await _context.SaveChangesAsync();
    }




}