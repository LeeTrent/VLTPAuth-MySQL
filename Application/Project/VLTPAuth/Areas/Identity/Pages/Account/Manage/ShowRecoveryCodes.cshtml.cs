// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

//namespace Microsoft.AspNetCore.Identity.UI.Pages.Account.Manage.Internal
namespace VLTPAuth.Areas.Identity.Pages.Account.Manage
{
    public class ShowRecoveryCodesModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly IOracleService _oracleService;

        public ShowRecoveryCodesModel
        (
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,  
            ILogger<LoginWith2faModel> logger,
            IOracleService oracleService
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _oracleService = oracleService;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // _logger.LogInformation("[ShowRecoveryCodesModel][OnPost] => Calling _signInManager.GetTwoFactorAuthenticationUserAsync()");
            // var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            // if (user == null)
            // {
            //     throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            // }

            _logger.LogInformation("[ShowRecoveryCodesModel][OnPost] => Calling _userManager.GetUserAsync(User)");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidOperationException($"[ShowRecoveryCodesModel][OnPost] => Call to _userManager.GetUserAsync(User) returned null.");
            }

            _logger.LogInformation("Calling OracleService to insert row into the VLTA.VLTP_AUTH table");   
            _logger.LogInformation("Parameters passed: USER_ID={0}, SSN={1}", user.Id, user.UserName);   

            /////////////////////////////////////////////////////////////////////////                
            // OracleService
            /////////////////////////////////////////////////////////////////////////                
            // Calling OracleService to insert a row into the VLTA.VLTP_AUTH table
            // Two (2) column values will be inserted: USER_ID and SSN
            // IMPORTANT:
            // VLTP_AUTH.USER_ID    maps to AspNetUsers.Id 
            // VLTP_AUTH.SSN        maps to AspNetUser.UserName 
            /////////////////////////////////////////////////////////////////////////
            //_oracleService.InsertRow(user.Id, user.UserName);

            _logger.LogInformation("[ShowRecoveryCodesModel][OnPost] => Calling _signInManager.SignOutAsync()");
            await _signInManager.SignOutAsync();

            _logger.LogInformation("[ShowRecoveryCodesModel][OnPost] => Redirecting to VLTP website");
            return Redirect(string.Format("https://apps.ocfo.gsa.gov/ords/volta/volta.volta_main?id={0}", user.Id));
        }
    }
}