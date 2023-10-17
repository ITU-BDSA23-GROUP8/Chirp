namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int page, int offset);

    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset);

    public Task<AuthorDTO?> GetAuthorFromName(string user);

    public Task<AuthorDTO?> GetAuthorFromEmail(string email);

    public void CreateAuthor(AuthorDTO author);

    public void CreateCheep(CheepDTO cheep, AuthorDTO author);

}