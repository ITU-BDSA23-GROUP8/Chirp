namespace Chirp.Infrastructure;

public class Like{

    public required Author Author { get; set; }
    public int AuthorId {get; set;}

    public required Cheep Cheep { get; set; }
    public int CheepId {get; set;}

}