using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FGC_OnBoarding.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using FGC_OnBoarding.Data;
using FGC_OnBoarding.Models.Users;
using System.Security.Claims;
using FGC_OnBoarding.Helpers;

namespace FGC_OnBoarding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<FGC_OnBoardingUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly FGC_OnBoardingContext _dbcontext;

        public LogoutModel(SignInManager<FGC_OnBoardingUser> signInManager, ILogger<LogoutModel> logger, FGC_OnBoardingContext dbcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> OnGet()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();


            //var remoteIpAddress = this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            //string Country = GetUserCountryByIp(remoteIpAddress.ToString());

            //CustomerLogs objnewLogs = new CustomerLogs();
            //objnewLogs.CustomerName = UserName;
            //objnewLogs.IPAdress = remoteIpAddress.ToString();
            //objnewLogs.Action = "Logout";
            //objnewLogs.Email = UserEmail;
            //objnewLogs.CustomerId = UserId;
            //objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
            //objnewLogs.CountryName = Country;
            //_dbcontext.CustomerLogs.Add(objnewLogs);
            //_dbcontext.SaveChanges();

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return Redirect("~/Identity/account/login");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            //await _signInManager.SignOutAsync();
           

            await HttpContext.SignOutAsync(
           CookieAuthenticationDefaults.AuthenticationScheme);


           


            _logger.LogInformation("User logged out.");

            return Redirect("~/Identity/account/login");
            //if (returnUrl != null)
            //{

            //    return LocalRedirect(returnUrl);
            //}
            //else
            //{
            //    return RedirectToPage();
            //}
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
