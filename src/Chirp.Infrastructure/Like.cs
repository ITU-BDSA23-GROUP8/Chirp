namespace Chirp.Infrastructure;

/// <summary>
/// A 'Like' is a represenation of a like given by an 'Author' to a 'Cheep'. 
/// It holdes the reference to the 'Author' the 'Cheep'- id's. 
/// </summary>

public class Like
{

    public required Author Author { get; set; }
    public int AuthorId { get; set; }
    public required Cheep Cheep { get; set; }
    public int CheepId { get; set; }

}