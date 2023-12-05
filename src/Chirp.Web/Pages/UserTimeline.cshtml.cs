﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Infrastructure.Migrations;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorrepository;
    public required List<CheepDTO> Cheeps { get; set; }

    public List<AuthorDTO> Following {get; set;} 

    public UserTimelineModel(ICheepRepository repo, IAuthorRepository authorrepo)
    {
        _repository = repo;
        _authorrepository = authorrepo;
    }

    public int pageRequest {get; set;}
    public async Task<ActionResult> OnGet(string author)
    {
        
        
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        pageRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (pageRequest == 0) {
            pageRequest = 1;
        }

        var cheeps = await _repository.GetCheepsFromAuthor(author, pageRequest, (pageRequest - 1) * 32);
        Cheeps = cheeps.ToList();

        if (User.Identity.IsAuthenticated){
            var following = await _authorrepository.GetFollowing(new AuthorDTO(User.Identity.Name, User.Identity.Name));
            Following = following.ToList();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostFollow(string AuthorName, string AuthorEmail)
    {
        var current = new AuthorDTO(User.Identity.Name, User.Identity.Name);
        var author = new AuthorDTO(AuthorName, AuthorEmail);
        await _authorrepository.Follow(author, current);
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostUnFollow(string AuthorName, string AuthorEmail)
    {
        var current = new AuthorDTO(User.Identity.Name, User.Identity.Name);
        var author = new AuthorDTO(AuthorName, AuthorEmail);
        await _authorrepository.UnFollow(author, current);

        return RedirectToPage();
    }
}
