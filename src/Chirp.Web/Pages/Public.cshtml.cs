using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authorrepository;
    public required List<CheepDTO> Cheeps { get; set; }

    public List<AuthorDTO> Following { get; set; }

    public PublicModel(ICheepRepository repo, IAuthorRepository authorrepo)
    {
        _cheeprepository = repo;
        _authorrepository = authorrepo;

    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        var message = Request.Form["message"];

        var author = new AuthorDTO(User.Identity.Name, User.Identity.Name);
        var cheep = new CheepDTO(User.Identity.Name, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        await _cheeprepository.CreateCheep(cheep, author);

        return LocalRedirect(returnUrl);
    }

    public int pageRequest { get; set; }

    public async Task<ActionResult> OnGet()
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        pageRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (pageRequest == 0)
        {
            pageRequest = 1;
        }

        var cheeps = await _cheeprepository.GetCheeps(pageRequest, (pageRequest - 1) * 32);
        Cheeps = cheeps.ToList();
        if (User.Identity.IsAuthenticated){
            var following = await _authorrepository.GetFollowing(new AuthorDTO(User.Identity.Name, User.Identity.Name));
            Following = following.ToList();
        }


        return Page();
    }

    public async Task<IActionResult> OnPostFollow(string Author)
    {
        var current = new AuthorDTO(User.Identity.Name, User.Identity.Name);
        var author = new AuthorDTO(Author, Author);
        _authorrepository.Follow(author, current);

        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostUnFollow(string Author)
    {
        var current = new AuthorDTO(User.Identity.Name, User.Identity.Name);
        var author = new AuthorDTO(Author, Author);
        _authorrepository.UnFollow(author, current);

        return RedirectToPage();
    }
}
