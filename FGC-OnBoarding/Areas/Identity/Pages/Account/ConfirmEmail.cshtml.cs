using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FGC_OnBoarding.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using FGC_OnBoarding.Models.Buisness;
using FGC_OnBoarding.Helpers;
using System.Net;
using static FGC_OnBoarding.Areas.Identity.Pages.Account.LoginModel;
using System.Globalization;
using Newtonsoft.Json;
using FGC_OnBoarding.Data;

namespace FGC_OnBoarding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly FGC_OnBoardingContext _dbcontext;

        public ConfirmEmailModel(FGC_OnBoardingContext dbcontext, UserManager<FGC_OnBoardingUser> userManager)
        {
            _userManager = userManager;
            _dbcontext = dbcontext;

        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            var remoteIpAddress = this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            string Country = GetUserCountryByIp(remoteIpAddress.ToString());

            Customers ObjCustomers = new Customers();
            ObjCustomers.Date = DateTime.Now.DateTime_UK();
            ObjCustomers.Email = user.Email;
            ObjCustomers.Name = user.Name;
            ObjCustomers.BuisnessName = user.BuisnessName;
            ObjCustomers.UserId = userId;
            ObjCustomers.Country = Country;
            ObjCustomers.Ip = remoteIpAddress.ToString();
            _dbcontext.Customers.Add(ObjCustomers);
            _dbcontext.SaveChanges();

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
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
    }

}
