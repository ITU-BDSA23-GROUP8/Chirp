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
    private readonly ICheepRepository _repository;
    public required List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository repo)
    {
        _repository = repo;
    }

    public int pageRequest {get; set;}


    public async Task<ActionResult> OnGet()
    {
         // https://learn.microsoft.com/en-us/dotnet/api/system.web.httprequest.querystring?view=netframework-4.8.1
        // used when looking for a specific page in the url, e.g. ?page=12
        pageRequest = Convert.ToInt32(Request.Query["page"]);

        // to start on page 1
        if (pageRequest == 0) {
            pageRequest = 1;
        }

        var cheeps = await _repository.GetCheeps(pageRequest, (pageRequest - 1) * 32);
        Cheeps = cheeps.ToList();
        return Page();
    }
}
