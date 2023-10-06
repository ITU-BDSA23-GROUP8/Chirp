using Microsoft.AspNetCore.Mvc;
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
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t == 0) t = 1;
        Cheeps = _service.GetCheepsFromAuthor(author, t);
        return Page();
    }
}
