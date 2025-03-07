namespace Chirp.Core;

/// <summary>
/// Interface abstraction of the Author data model.
/// Holds the methods for an Author.
/// Facilitates the seperations of concerns between the Data Layer and the Business Layer
/// </summary>

public interface IAuthorRepository
{
    public void CreateAuthor(AuthorDTO author);

    public Task Follow(AuthorDTO author, AuthorDTO follower);

    public Task UnFollow(AuthorDTO author, AuthorDTO follower);

    public Task<List<AuthorDTO>> GetFollowing(AuthorDTO author);

    public Task<bool> IsFollowing(AuthorDTO author, AuthorDTO follower);
}