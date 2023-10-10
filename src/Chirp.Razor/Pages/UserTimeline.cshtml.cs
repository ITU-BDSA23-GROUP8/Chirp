﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        var urlRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (urlRequest == 0) {
            urlRequest = 1;
        }

        Cheeps = _service.GetCheepsFromAuthor(author, urlRequest);
        return Page();
    }
}
