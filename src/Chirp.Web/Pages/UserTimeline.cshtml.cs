﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Infrastructure.Migrations;
using System.Security.Claims;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorrepository;
    public required List<CheepDTO> Cheeps { get; set; } 

    public List<AuthorDTO> Following { get; set; } = new List<AuthorDTO>();

    public UserTimelineModel(ICheepRepository repo, IAuthorRepository authorrepo)
    {
        _repository = repo;
        _authorrepository = authorrepo;
    }

    public int pageRequest { get; set; }
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
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
            Following = following.ToList();
            Console.WriteLine("following: " + Following.Count);

            if (userName.Equals(author))
            {
                Console.WriteLine("own timeline");
                var cheeps = await _repository.GetCheepsFromFollowing(userName, pageRequest, (pageRequest - 1) * 32);
                Cheeps = cheeps.ToList();
            }
            else
            {
                var cheeps = await _repository.GetCheepsFromAuthor(author, pageRequest, (pageRequest - 1) * 32);
                Cheeps = cheeps.ToList();
            }
        }
        else
        {
            var cheeps = await _repository.GetCheepsFromAuthor(author, pageRequest, (pageRequest - 1) * 32);
            Cheeps = cheeps.ToList();
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
}
