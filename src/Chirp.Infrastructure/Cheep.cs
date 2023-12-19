using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Chirp.Infrastructure;

/// <summary>
/// A 'Cheep' is a message posted by an authenticated 'Author'. 
/// A 'Cheep' has a restriction of 160 char's, and contains an 'Author', Text string, and TimeStamp. 
/// It also holdes a reference to the 'Author' who posts the Cheep.
/// Each 'Cheep' contain a List of Likes. 
/// </summary>

public class Cheep
{
    public int Id { get; set; }
    [MaxLength(160, ErrorMessage = "Message too long")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
    public int AuthorId { get; set; }
    public List<Like> Likes { get; set; } = new List<Like>();

}




