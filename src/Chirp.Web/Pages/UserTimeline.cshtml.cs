using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Infrastructure.Migrations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Razor.Pages;

/// <summary>
/// Razor PageModel for author-specific cheeps, like endpoint "/<author>". 
/// Fetches a list of cheeps and other DTO's according to user input and other conditions.
/// </summary>

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorrepository;
    private readonly ILikeRepository _likerepository;

    // Cheeps is the list of cheeps currently being showed. Is assigned in OnGet() handler.
    public required List<CheepDTO> Cheeps { get; set; }

    // Likes is a list of cheeps, that the author has liked. 
    public List<CheepDTO> Likes { get; set; }

    // Following is a list of authors that the user is following.
    public List<AuthorDTO> Following { get; set; } = new List<AuthorDTO>();

    // pageRequest is used for paging, s.t. only 32 cheeps are shown at a time. 
    public int pageRequest { get; set; }

    /// <summary>
    /// Constructor uses dependency injection to get necessary repositories (services). 
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="authorrepo"></param>
    /// <param name="likerepo"></param>
    public UserTimelineModel(ICheepRepository repo, IAuthorRepository authorrepo, ILikeRepository likerepo)
    {
        _repository = repo;
        _authorrepository = authorrepo;
        _likerepository = likerepo;
    }

    /// <summary>
    /// OnGet() is handler method called when UserTimeline page is first navigated to.
    /// Fetches what cheeps should be shown according to whether the user is authenticated and other conditions.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<ActionResult> OnGet(string author)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        pageRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (pageRequest == 0)
        {
            pageRequest = 1;
        }

        if (User.Identity.IsAuthenticated)
        {
            // if user is logged in, use AuthorRepository to fetch list of authors user is following
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
            Following = following.ToList();


            if (userName.Equals(author))
            {
                // if user is logged in and author = user, then show cheeps from user and following
                var cheeps = await _repository.GetCheepsFromFollowing(userName, pageRequest, (pageRequest - 1) * 32);
                Cheeps = cheeps.ToList();
            }
            else
            {
                // if user is logged in but author != user, then show cheeps from author
                var cheeps = await _repository.GetCheepsFromAuthor(author, pageRequest, (pageRequest - 1) * 32);
                Cheeps = cheeps.ToList();
            }

            // if user is logged in, fetch what cheeps the user has liked. 
            var likes = await _likerepository.GetLikedCheeps(new AuthorDTO(userName, userEmail));
            Likes = likes.ToList();
        }
        else
        {
            // if user is not logged in, show cheeps from author
            var cheeps = await _repository.GetCheepsFromAuthor(author, pageRequest, (pageRequest - 1) * 32);
            Cheeps = cheeps.ToList();
        }


        return Page();
    }

    /// <summary>
    /// OnPostFollow() is a named page handler, that is called as a post method in UserTimeline.cshtml
    /// Handles a user following another author. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="AuthorName"></param>
    /// <param name="AuthorEmail"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostFollow(string AuthorName, string AuthorEmail)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var current = new AuthorDTO(userName, userEmail);
            var author = new AuthorDTO(AuthorName, AuthorEmail);
            await _authorrepository.Follow(author, current);
        }

        return RedirectToPage();
    }

    /// <summary>
    /// OnPostUnFollow() is a named page handler, that is called as a post method in UserTimeline.cshtml
    /// Handles a user unfollowing another author. 
    /// The methods returns RedirectToPage(), which calls a GET request and therefore calls OnGet() again.
    /// </summary>
    /// <param name="AuthorName"></param>
    /// <param name="AuthorEmail"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostUnFollow(string AuthorName, string AuthorEmail)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var current = new AuthorDTO(userName, userEmail);
            var author = new AuthorDTO(AuthorName, AuthorEmail);
            await _authorrepository.UnFollow(author, current);
        }


        return RedirectToPage();
    }

    /// <summary>
    /// OnPostLike() is a named page handler, that is called as a POST method in UserTimeline.cshtml
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
    /// OnPostUnlike() is a named page handler, that is called as a POST method in UserTimeline.cshtml
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
