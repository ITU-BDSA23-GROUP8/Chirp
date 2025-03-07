using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;

/// <summary>
/// An 'Author' is a user of the application. Each 'Author' is connected to a list of 'Cheeps', 'Followers', 'Following', 
/// and 'Likes'. Each list start off as empty. 
/// The 'Author' class inherits from the 'IdentityUaer<int>', which is a class provided by ASP.NET. 
/// </summary>

public class Author : IdentityUser<int>{
    public required List<Cheep>Cheeps { get; set;}
    public List<Author>Followers { get; set;} = new List<Author>();
    public List<Author>Following { get; set;} = new List<Author>();
    public List<Like> Likes {get; set;} = new List<Like>();
}
