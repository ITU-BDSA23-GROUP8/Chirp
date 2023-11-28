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
                Cheeps = new List<Cheep>(),
                Followers = new List<Author>(),
                Following = new List<Author>()
            };

            _context.Authors
            .Add(AuthorModel);
            _context.SaveChanges();
        } else {
            throw new ArgumentException("Author already exists");
        }
    }

    public void Follow(AuthorDTO author, AuthorDTO follower){
        
        var authorModel = _context.Authors.FirstOrDefault(a => a.Email == author.Email);
        var followerModel = _context.Authors.FirstOrDefault(b => b.Email == follower.Email);

        if(authorModel != null && followerModel != null){
            if(!authorModel.Following.Contains(followerModel)){
                authorModel.Following.Add(followerModel);
                followerModel.Followers.Add(authorModel);
            }
            _context.SaveChanges();
        }


    }

    public void UnFollow(AuthorDTO author, AuthorDTO follower){
        var authorModel = _context.Authors.FirstOrDefault(a => a.Email == author.Email);
        var followerModel = _context.Authors.FirstOrDefault(b => b.Email == follower.Email);

        if(authorModel != null && followerModel != null){
            if(authorModel.Following.Contains(followerModel)){
                authorModel.Following.Remove(followerModel);
                followerModel.Followers.Remove(authorModel);
            }
            _context.SaveChanges();
        }
    }

    public async Task<List<AuthorDTO>> GetFollowers(AuthorDTO author){
        var authorModel = _context.Authors.FirstOrDefault(a => a.Email == author.Email);
        var followers = new List<AuthorDTO>();
        foreach(var follower in authorModel.Followers){
            followers.Add(new AuthorDTO(follower.UserName, follower.Email));
        }

        return followers;
    }
    public async Task<List<AuthorDTO>> GetFollowing(AuthorDTO author){
        var authorModel = _context.Authors.FirstOrDefault(a => a.Email == author.Email);
        var following = new List<AuthorDTO>();
        foreach(var follower in authorModel.Following){
            following.Add(new AuthorDTO(follower.UserName, follower.Email));
        }

        return following;
    }


}