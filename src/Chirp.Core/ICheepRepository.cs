namespace Chirp.Core;

/// <summary>
/// Interface abstraction of the Cheep data model.
/// Holds the methods for a Cheep.
/// Facilitates the seperations of concerns between the Data Layer and the Business Layer
/// </summary>

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int page, int offset);

    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string user, int page, int offset);

    public Task<IEnumerable<CheepDTO>> GetAllCheepsFromAuthor(string user);

    public Task<IEnumerable<CheepDTO>> GetCheepsFromFollowing(string user, int page, int offset);

    public Task CreateCheep(CheepDTO cheep, AuthorDTO author);

}