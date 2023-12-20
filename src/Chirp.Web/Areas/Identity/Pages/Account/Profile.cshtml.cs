using System.Security.Claims;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#nullable disable

namespace MyApp.Namespace
{
    /// <summary>
    /// ProfileModel is a page model that fetches and models data specific for the logged in user. 
    /// </summary>
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

        /// <summary>
        /// Constructor depends on abstractions like interfaces according to dependency injection.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="authorrepo"></param>
        /// <param name="likerepo"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public ProfileModel(ICheepRepository repo, IAuthorRepository authorrepo, ILikeRepository likerepo, UserManager<Author> userManager, SignInManager<Author> signInManager)
        {
            _repository = repo;
            _authorrepository = authorrepo;
            _likerepository = likerepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// OnGetAsync() uses the repositories to fetch relevant data. 
        /// </summary>
        /// <returns></returns>

        public async Task OnGetAsync()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

                // fetch user's following
                var following = await _authorrepository.GetFollowing(new AuthorDTO(userName, userEmail));
                Following = following.ToList();

                // fetch user claims for name and email
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var emailClaim = claimsIdentity!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                Email = emailClaim!.Value;

                var nameClaim = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                Name = nameClaim!.Value;

                // fetch user's cheeps
                var cheeps = await _repository.GetAllCheepsFromAuthor(Name);
                Cheeps = cheeps.ToList();

            }

        }

        /// <summary>
        /// OnPostForgetMe() is a named page handler, that handles Forget Me functionality.
        /// Uses UserManager and SignInManager to delete user and signout. 
        /// All likes from user are deleted first. 
        /// </summary>
        /// <returns></returns>
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
