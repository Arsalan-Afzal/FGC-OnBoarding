using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FGC_OnBoarding.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace FGC_OnBoarding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<FGC_OnBoardingUser> _signInManager;
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<FGC_OnBoardingUser> userManager,
            SignInManager<FGC_OnBoardingUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {


            [Required(ErrorMessage = "Email Required")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Your Name Required")]
            public string Name { get; set; }
            [Required(ErrorMessage ="BuisnessName Required")]
            [Display(Name = "BuisnessName")]
            public string BuisnessName { get; set; }
            [Required(ErrorMessage = "Password Required")]
            [StringLength(100, ErrorMessage = "The Password must be at least 8 or more characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [Required(ErrorMessage = "Confirm Password Required")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new FGC_OnBoardingUser {UserName= Input.UserName,Email = Input.UserName,  Name = Input.Name,BuisnessName=Input.BuisnessName};
                var CheckEmail = await _userManager.FindByEmailAsync(Input.UserName);
                if (CheckEmail == null)
                {
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.UserName, "Confirm your email",
                            $"Hi " + Input.Name + " ," + "<br/><br/>" + "Thank you for creating account with FGC-Capital.Please Verify your Email by <a href=" + HtmlEncoder.Default.Encode(callbackUrl) + ">clicking here</a>." + "<br/><br/> " + "Regards," + "<br/>" + "OnBoarding Team" + "<br/>" + "FGC-Capital" + "<br/><br/>" + " <img src='https://ci4.googleusercontent.com/proxy/Ijy_ufJDMMBULhWYgAsrj0fr2fvZrDS0mpXJWi2JUg0Utl69zaiqiUy5p8-VDLMZvkl3-u_BR_ml_pcVosmrvEfoaoVAug_2WBaMtYPOTFCZGSQwiSclvJWZJFB5=s0-d-e1-ft#https://www.fgc-capital.com/apply-business-application/image/fgc_email.jpg' border='0' width='100' height='35' class='CToWUd'>");


                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.UserName, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                     bool passwordflag = false;
                    foreach (var error in result.Errors)
                    {
                        if (error.Description.Contains("Password") && passwordflag == false)
                        {
                           passwordflag =true;
                            ModelState.AddModelError(string.Empty, "Password must have atleast one Capital letter, Small letter, Special Character, Numeric value");
                           
                        }
                        else if(!error.Description.Contains("Password"))
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This email is already used with another account.please use another email");
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
