// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#nullable disable

namespace Chirp.Web.Areas.Identity.Pages.Account;

public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<Author> _signInManager;
    private readonly UserManager<Author> _userManager;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<Author> signInManager,
        UserManager<Author> userManager,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string LoginProvider { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public IActionResult OnGetAsync(string returnUrl = null)
    {
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("GitHub", redirectUrl);
        return new ChallengeResult("GitHub", properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login");
        }
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToPage("./Login");
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            // Store the access token and resign in so the token is included in the cookie
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            var props = new AuthenticationProperties();
            props.StoreTokens(info.AuthenticationTokens);
            await _signInManager.SignInAsync(user, props, info.LoginProvider);

            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
            return RedirectToPage("/Public");
        }
        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }
        else
        {
            // If the user does not have an account, then ask the user to create an account.
            ReturnUrl = returnUrl;
            LoginProvider = info.LoginProvider;
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                Input = new InputModel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };
            }
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                //var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }


                var user = new Author
                {
                    UserName = "name",
                    Email = "mail",
                    Cheeps = new List<Cheep>()
                };



                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
                    {
                        await _userManager.SetUserNameAsync(user, info.Principal.FindFirstValue(ClaimTypes.Name));
                        user.UserName = info.Principal.FindFirstValue(ClaimTypes.Name);
                    }
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                    {
                        await _userManager.SetEmailAsync(user, info.Principal.FindFirstValue(ClaimTypes.Email));
                        user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);

                    }
                    if (user.UserName!.Equals("name") || user.Email!.Equals("mail"))
                    {
                        throw new ApplicationException("Error loading user information from external login.");
                    }

                    createResult = await _userManager.AddLoginAsync(user, info);
                    if (createResult.Succeeded)
                    {

                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);

                        await _signInManager.SignInAsync(user, props, authenticationMethod: info.LoginProvider);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToPage("/Public");
                    }
                }
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ReturnUrl = returnUrl;
            return RedirectToPage("/Public");
        }
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }

            var user = new Author
            {
                UserName = "name",
                Email = "email",
                Cheeps = new List<Cheep>()
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
                {
                    await _userManager.SetUserNameAsync(user, info.Principal.FindFirstValue(ClaimTypes.Name));
                    user.UserName = info.Principal.FindFirstValue(ClaimTypes.Name);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    await _userManager.SetEmailAsync(user, info.Principal.FindFirstValue(ClaimTypes.Email));
                    user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);

                }

                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {

                    // Include the access token in the properties
                    var props = new AuthenticationProperties();
                    props.StoreTokens(info.AuthenticationTokens);

                    await _signInManager.SignInAsync(user, props, authenticationMethod: info.LoginProvider);
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                    return RedirectToPage();
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        ReturnUrl = returnUrl;
        return Page();
    }
}