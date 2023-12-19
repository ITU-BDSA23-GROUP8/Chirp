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

/// <summary>
/// Razor PageModel for Public page with endpoint "/". Fetches all cheeps from newest to oldest. 
/// Handles user input such as paging, posting a cheep, following and liking. 
/// </summary>

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheeprepository;
    private readonly IAuthorRepository _authorrepository;
    private readonly ILikeRepository _likerepository;

    // Cheeps is the list of cheeps currently being showed. Is assigned in OnGet() handler.
    public required List<CheepDTO> Cheeps { get; set; }

    // Likes is a list of cheeps, that the author has liked. 
    public List<AuthorDTO> Following { get; set; }

    // Following is a list of authors that the user is following.
    public List<CheepDTO> Likes { get; set; }

    // pageRequest is used for paging, s.t. only 32 cheeps are shown at a time. 
    public int pageRequest { get; set; }

    /// <summary>
    /// Constructor uses dependency injection to access services, used to access data and logic.
    /// </summary>
    public PublicModel(ICheepRepository repo, IAuthorRepository authorrepo, ILikeRepository likerepo)
    {
        _cheeprepository = repo;
        _authorrepository = authorrepo;
        _likerepository = likerepo;

    }

    /// <summary>
    /// Razor page handler for posting a cheep as a user. 
    /// </summary>
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        var message = Request.Form["message"];

        if (!message.IsNullOrEmpty())
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var author = new AuthorDTO(userName, userEmail);
            var cheep = new CheepDTO(userName, userEmail, message!, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), -1, 0);

            await _cheeprepository.CreateCheep(cheep, author);
        }

        return LocalRedirect(returnUrl);
    }

    /// <summary>
    /// OnGet() is handler method called when PublicTimeline page is first navigated to.
    /// Fetches what cheeps should be shown according to whether the user is authenticated and other conditions.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
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
        if (User.Identity.IsAuthenticated)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
            Following = following.ToList();

            var likes = await _likerepository.GetLikedCheeps(new AuthorDTO(userName, userEmail));
            Likes = likes.ToList();
        }


        return Page();
    }

    /// <summary>
    /// OnPostFollow() is a named page handler, that is called as a post method in Public.cshtml
    /// Handles a user following another author. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="AuthorName"></param>
    /// <param name="AuthorEmail"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostFollow(string AuthorName, string AuthorEmail)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        var current = new AuthorDTO(userName, userEmail);
        var author = new AuthorDTO(AuthorName, AuthorEmail);

        await _authorrepository.Follow(author, current);

        return RedirectToPage();
    }

    /// <summary>
    /// OnPostUnFollow() is a named page handler, that is called as a post method in Public.cshtml
    /// Handles a user unfollowing another author. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="AuthorName"></param>
    /// <param name="AuthorEmail"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostUnFollow(string AuthorName, string AuthorEmail)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        var current = new AuthorDTO(userName, userEmail);
        var author = new AuthorDTO(AuthorName, AuthorEmail);
        await _authorrepository.UnFollow(author, current);

        return RedirectToPage();
    }


    /// <summary>
    /// OnPostLike() is a named page handler, that is called as a POST method in Public.cshtml
    /// Handles a user liking a cheep. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="cheepID"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostLike(int cheepID)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        await _likerepository.Like(new AuthorDTO(userName, userEmail), cheepID);

        return RedirectToPage();
    }

    /// <summary>
    /// OnPostUnlike() is a named page handler, that is called as a POST method in Public.cshtml
    /// Handles a user removing a like on a cheep. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="cheepID"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostUnlike(int cheepID)
    {
        var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        await _likerepository.UnLike(new AuthorDTO(userName, userEmail), cheepID);

        return RedirectToPage();
    }




}
