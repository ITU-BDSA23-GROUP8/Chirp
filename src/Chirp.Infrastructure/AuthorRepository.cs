using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;

using System.Security.Permissions;
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
        }
        else
        {
            throw new ArgumentException("Author already exists");
        }
    }

    public async Task Follow(AuthorDTO author, AuthorDTO follower)
    {

        var authorModel = _context.Authors.FirstOrDefault(a => a.UserName == author.Name);
        var followerModel = _context.Authors.FirstOrDefault(b => b.UserName == follower.Name);

        if (authorModel != null && followerModel != null)
        {
            if (!authorModel.Followers.Contains(followerModel))
            {
                authorModel.Followers.Add(followerModel);
                followerModel.Following.Add(authorModel);

            }

        }
        await _context.SaveChangesAsync();


    }

    public async Task UnFollow(AuthorDTO author, AuthorDTO follower)
    {
        var authorModel = await _context.Authors.Include(a => a.Followers).FirstOrDefaultAsync(a => a.UserName == author.Name);
        var followerModel = await _context.Authors.Include(a => a.Following).FirstOrDefaultAsync(b => b.UserName == follower.Name);

        if (authorModel != null && followerModel != null)
        {

            var isFollow = await IsFollowing(author, follower);
            if (isFollow)
            {

                authorModel.Followers.Remove(followerModel);
                followerModel.Following.Remove(authorModel);
            }
        }
        await _context.SaveChangesAsync();

    }

    public async Task<List<AuthorDTO>> GetFollowers(AuthorDTO author)
    {
        var list = await _context.Authors
        .Where(x => x.Following.Any(y => y.Email == author.Email))
        .ToListAsync();



        var Followers = new List<AuthorDTO>();
        foreach (var Follower in list)
        {
            Followers.Add(new AuthorDTO(Follower.UserName, Follower.Email));
        }

        return Followers;
    }
    public async Task<List<AuthorDTO>> GetFollowing(AuthorDTO author)
    {
        var list = await _context.Authors.Where(x => x.Followers.Any(y => y.Email == author.Email)).ToListAsync();

        var Following = new List<AuthorDTO>();

        foreach (var Follower in list)
        {
            Following.Add(new AuthorDTO(Follower.UserName, Follower.Email));
        }

        return Following;
    }

    public async Task<bool> IsFollowing(AuthorDTO author, AuthorDTO follower)
    {
        var list = await GetFollowing(follower);

        return list.Contains(author);
    }


}