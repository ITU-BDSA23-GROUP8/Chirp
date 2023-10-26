namespace Chirp.Core;

public interface IAuthorRepository
{

    public Task<AuthorDTO?> GetAuthorFromName(string user);

    public Task<AuthorDTO?> GetAuthorFromEmail(string email);

    public void CreateAuthor(AuthorDTO author);


}