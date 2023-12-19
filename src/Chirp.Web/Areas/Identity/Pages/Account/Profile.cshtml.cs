using System.Security.Claims;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#nullable disable

namespace MyApp.Namespace
{
    public class ProfileModel : PageModel
    {
        public List<AuthorDTO> Following { get; set; }
        public required List<CheepDTO> Cheeps { get; set; }
        private readonly ICheepRepository _repository;
        private readonly IAuthorRepository _authorrepository;
        private readonly ILikeRepository _likerepository;
        private readonly UserManager<Author> _userManager;
        private readonly SignInManager<Author> _signInManager;

        public string Name;

        public string Email;

        public ProfileModel(ICheepRepository repo, IAuthorRepository authorrepo, ILikeRepository likerepo, UserManager<Author> userManager, SignInManager<Author> signInManager)
        {
            _repository = repo;
            _authorrepository = authorrepo;
            _likerepository = likerepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

                var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
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

        public async Task<IActionResult> OnPostForgetMe()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _likerepository.DeleteLikes(new AuthorDTO(user.UserName, user.Email));
                await _userManager.DeleteAsync(user);
                await _signInManager.SignOutAsync();
            }
            return Redirect("/");
        }
    }
}
