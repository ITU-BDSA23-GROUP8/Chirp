﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repo)
    {
        _repository = repo;
    }

    public async Task<ActionResult> OnGet(string author)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        var urlRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (urlRequest == 0) {
            urlRequest = 1;
        }

        var cheeps = await _repository.GetCheepsFromAuthor(author, urlRequest, (urlRequest - 1) * 32);
        Cheeps = cheeps.ToList();
        return Page();
    }
}
