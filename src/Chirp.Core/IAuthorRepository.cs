namespace Chirp.Core;

public interface IAuthorRepository
{

    public Task<AuthorDTO?> GetAuthorFromName(string user);

    public Task<AuthorDTO?> GetAuthorFromEmail(string email);

    public void CreateAuthor(AuthorDTO author);

    public void Follow(AuthorDTO author, AuthorDTO follower);

    public void UnFollow(AuthorDTO author, AuthorDTO follower);

    public Task<List<AuthorDTO>> GetFollowers(AuthorDTO author);

    public Task<List<AuthorDTO>> GetFollowing(AuthorDTO author);

    
    public Task<bool> IsFollowing(AuthorDTO author, AuthorDTO follower);


}