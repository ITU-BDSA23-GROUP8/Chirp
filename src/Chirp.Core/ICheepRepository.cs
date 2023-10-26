namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int page, int offset);

    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset);

    public void CreateCheep(CheepDTO cheep, AuthorDTO author);

}