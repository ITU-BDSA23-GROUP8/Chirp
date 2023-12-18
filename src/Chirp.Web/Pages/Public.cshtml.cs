using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Chirp.Infrastructure.Migrations;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authorrepository;

    private readonly ILikeRepository _likerepository;


    public required List<CheepDTO> Cheeps { get; set; }

    public List<AuthorDTO> Following { get; set; }

    public List<CheepDTO> Likes { get; set; }

    public PublicModel(ICheepRepository repo, IAuthorRepository authorrepo, ILikeRepository likerepo)
    {
        _cheeprepository = repo;
        _authorrepository = authorrepo;
        _likerepository = likerepo;

    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        var message = Request.Form["message"];

        if (!message.IsNullOrEmpty()){
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var author = new AuthorDTO(userName, userEmail);
            var cheep = new CheepDTO(userName, userEmail, message!, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), -1, 0);
            await _cheeprepository.CreateCheep(cheep, author);
        }

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
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
            Following = following.ToList();

            var likes = await _likerepository.GetLikedCheeps(new AuthorDTO(userName, userEmail));
            Likes = likes.ToList();
        }


        return Page();
    }

    public async Task<IActionResult> OnPostFollow(string AuthorName, string AuthorEmail)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        
        var current = new AuthorDTO(userName, userEmail);
        var author = new AuthorDTO(AuthorName, AuthorEmail);
        
        await _authorrepository.Follow(author, current);
        
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostUnFollow(string AuthorName, string AuthorEmail)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        var current = new AuthorDTO(userName, userEmail);
        var author = new AuthorDTO(AuthorName, AuthorEmail);
        await _authorrepository.UnFollow(author, current);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLike(int cheepID){
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        await _likerepository.Like(new AuthorDTO(userName, userEmail), cheepID);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnlike(int cheepID){
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        await _likerepository.UnLike(new AuthorDTO(userName, userEmail), cheepID);

        return RedirectToPage();
    }

    

    
}
