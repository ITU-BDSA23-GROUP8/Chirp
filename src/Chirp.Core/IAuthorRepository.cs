namespace Chirp.Core;

/// <summary>
/// Interface abstraction of the Author data model.
/// Holds the methods for an Author.
/// Facilitates the seperations of concerns between the Data Layer and the Business Layer
/// </summary>

public interface IAuthorRepository
{
    public Task<AuthorDTO?> GetAuthorFromName(string user);
    public Task<AuthorDTO?> GetAuthorFromEmail(string email);
    public void CreateAuthor(AuthorDTO author);
    public Task Follow(AuthorDTO author, AuthorDTO follower);
    public Task UnFollow(AuthorDTO author, AuthorDTO follower);
    public Task<List<AuthorDTO>> GetFollowers(AuthorDTO author);
    public Task<List<AuthorDTO>> GetFollowing(AuthorDTO author);
    public Task<bool> IsFollowing(AuthorDTO author, AuthorDTO follower);
}