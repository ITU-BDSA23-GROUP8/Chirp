namespace Chirp.Core;

/// <summary>
/// Interface abstraction of the Like data model.
/// Holds the methods for Like.
/// Facilitates the seperations of concerns between the Data Layer and the Business Layer. 
/// </summary>

public interface ILikeRepository
{
    public Task<List<CheepDTO>> GetLikedCheeps(AuthorDTO author);
    public Task<int> LikeCount(int cheepID);
    public Task Like(AuthorDTO author, int cheepID);
    public Task UnLike(AuthorDTO author, int cheepID);

}