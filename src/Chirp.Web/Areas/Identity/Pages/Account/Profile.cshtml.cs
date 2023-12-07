using System.Security.Claims;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class ProfileModel : PageModel
    {
        public List<AuthorDTO> Following { get; set; }
        public required List<CheepDTO> Cheeps { get; set; }
        private readonly ICheepRepository _repository;
        private readonly IAuthorRepository _authorrepository;

        public string Name;

        public string Email;

        public ProfileModel(ICheepRepository repo, IAuthorRepository authorrepo)
        {
            _repository = repo;
            _authorrepository = authorrepo;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var following = await _authorrepository.GetFollowing(new AuthorDTO(User.Identity.Name, User.Identity.Name));
                Following = following.ToList();
                
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var emailClaim = claimsIdentity!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                Email = emailClaim!.Value;
                
                var nameClaim = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                Name = nameClaim!.Value;

                var cheeps = await _repository.GetAllCheepsFromAuthor(Name);
                Cheeps = cheeps.ToList();

            }

        }
    }
}
