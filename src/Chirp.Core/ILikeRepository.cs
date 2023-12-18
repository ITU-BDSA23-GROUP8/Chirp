namespace Chirp.Core;

public interface ILikeRepository
{
    public Task<List<CheepDTO>> GetLikedCheeps(AuthorDTO author);

    public Task ToggleLike(AuthorDTO author, int cheepID);

    public Task<int> likeCount(int cheepID);

    //public Task<bool> AuthorHasLiked(AuthorDTO author, int cheepID);
    public Task Like(AuthorDTO author, int cheepID);
    public Task UnLike(AuthorDTO author, int cheepID);

}