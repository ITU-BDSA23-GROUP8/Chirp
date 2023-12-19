namespace Chirp.Core;

public interface ILikeRepository
{
    public Task<List<CheepDTO>> GetLikedCheeps(AuthorDTO author);
    public Task<int> LikeCount(int cheepID);
    public Task Like(AuthorDTO author, int cheepID);
    public Task UnLike(AuthorDTO author, int cheepID);

}