using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;

public class Author : IdentityUser<int>{
    public required List<Cheep>Cheeps { get; set;}
}
