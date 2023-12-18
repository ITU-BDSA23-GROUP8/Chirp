using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Chirp.Infrastructure;



public class Cheep{
    public int Id {get; set;}
    [MaxLength(160, ErrorMessage = "Message too long")]
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
    public int AuthorId {get; set;}

    public List<Like> Likes {get; set;} = new List<Like>();

}




