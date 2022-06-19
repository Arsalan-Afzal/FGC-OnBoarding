using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FGC_OnBoarding.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using FGC_OnBoarding.Data;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using FGC_OnBoarding.Models.Users;
using FGC_OnBoarding.Helpers;

namespace FGC_OnBoarding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly SignInManager<FGC_OnBoardingUser> _signInManager;

        private readonly FGC_OnBoardingContext _dbcontext;

        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<FGC_OnBoardingUser> signInManager,
            ILogger<LoginModel> logger,
            FGC_OnBoardingContext dbcontext,
            UserManager<FGC_OnBoardingUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbcontext = dbcontext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(user.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
     {
            new Claim("Email", user.Email),
            new Claim("FullName", user.Name),
            new Claim("UserId",user.Id),
            new Claim("Role", "Administrator"),
        };

                    var claimsIdentity = new ClaimsIdentity(
       claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties {
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    var remoteIpAddress = this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
                    string Country = GetUserCountryByIp(remoteIpAddress.ToString());

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = user.Name;
                    objnewLogs.IPAdress = remoteIpAddress.ToString();
                    objnewLogs.Action = "Login";
                    objnewLogs.Email = user.Email;
                    objnewLogs.CustomerId = user.Id;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.LoginTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;
                    _dbcontext.CustomerLogs.Add(objnewLogs);
                    _dbcontext.SaveChanges();
                    _logger.LogInformation("User logged in.");

                    return Redirect("~/Home/Index");
                    //                  return LocalRedirect(returnUrl);


                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        public class IpInfo
        {
            public string Country { get; set; }
        }
        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();

            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }
        public static string GetClientIPAddress(HttpContext context)
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
