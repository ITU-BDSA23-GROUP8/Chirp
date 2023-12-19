namespace Chirp.Core;

public interface IAuthorRepository
{

    public void CreateAuthor(AuthorDTO author);

    public Task Follow(AuthorDTO author, AuthorDTO follower);

    public Task UnFollow(AuthorDTO author, AuthorDTO follower);

    public Task<List<AuthorDTO>> GetFollowing(AuthorDTO author);

    
    public Task<bool> IsFollowing(AuthorDTO author, AuthorDTO follower);


}