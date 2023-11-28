using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;

public class Author : IdentityUser<int>{
    public required List<Cheep>Cheeps { get; set;}
    public required List<Author>Followers { get; set;}
    public required List<Author>Following { get; set;}
}
