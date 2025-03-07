// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;

namespace Chirp.Web.Areas.Identity.Pages.Account
{
    /// <summary>
    /// LogoutModel page model handles logging a user out.
    /// The page model is taken from ASP.NET Core Identity Razor Class Library and adapted to fit our specific needs. 
    /// </summary>
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<Author> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// OnPost() is a named page handler which uses SignInManager to sign a user out. 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            
            return RedirectToPage("/Public");
            
        }
    }
}
