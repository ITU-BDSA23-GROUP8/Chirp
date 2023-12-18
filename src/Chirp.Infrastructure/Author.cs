using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;

public class Author : IdentityUser<int>{
    public required List<Cheep>Cheeps { get; set;}
    public List<Author>Followers { get; set;} = new List<Author>();
    public List<Author>Following { get; set;} = new List<Author>();
    public List<Like> Likes {get; set;} = new List<Like>();
}
