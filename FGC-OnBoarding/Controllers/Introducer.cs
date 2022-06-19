using FGC_OnBoarding.Areas.Identity.Data;
using FGC_OnBoarding.Data;
using FGC_OnBoarding.Helpers;
using FGC_OnBoarding.Models.Buisness;
using FGC_OnBoarding.Models.IntroducersModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Controllers
{

    public class Introducer : Controller
    {
        private readonly ILogger<Introducer> _logger;
        private readonly IWebHostEnvironment _env;
        //  private readonly ILogger<UserManager> _userManager;
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly FGC_OnBoardingContext _context;
        public int BuisnessProfileSaved = 0;
        public Introducer(ILogger<Introducer> logger, UserManager<FGC_OnBoardingUser> userManager, FGC_OnBoardingContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminLogin()
        {
            ViewBag.message = TempData["msg"];

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(IntroducerUsers objUser)
        {
            try
            {
                var Introducer = _context.IntroducerUsers.Where(x => x.UserName == objUser.UserName && x.Password == objUser.Password).FirstOrDefault();
                if (Introducer != null)
                {
                    if (Introducer.Status == "Active")
                    {
                        var claims = new List<Claim>
                    {
                      new Claim("Email", Introducer.Email),
                      new Claim("FullName", Introducer.FirstName + " " + Introducer.LastName),
                       new Claim("IntroducerId", Introducer.IntroducerId.ToString()),
                      new Claim("UserId",Introducer.UserId.ToString()),
                       new Claim("Role","Introducer"),
                    };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("NewApplications");
                    }
                    else
                    {
                        TempData["msg"] = "This account has been blocked, please contact Administrator";
                        return RedirectToAction("AdminLogin");
                    }
                }
                else
                {
                    TempData["msg"] = "Invalid Username and Passowrd! Please try again";
                    return RedirectToAction("AdminLogin");
                }
             
            }
            catch (Exception e)
            {
                TempData["msg"] = "Something went wrong";
                return RedirectToAction("AdminLogin");
            }


        }
        public IActionResult NewApplications(int? ComingIntroducerId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int IntroducerUserId = 0;
            if (ComingIntroducerId != null)
            {
                IntroducerUserId = Convert.ToInt32(ComingIntroducerId);
                ViewBag.ComingIntroudcerId = ComingIntroducerId;
            }
            else
            {
                IntroducerUserId = Convert.ToInt32(claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault());
            }
            

             int IntroducerId = Convert.ToInt32(claims.Where(x => x.Type == "IntroducerId").Select(x => x.Value).FirstOrDefault());
            var IntroducersApplications = _context.IntroducersUsersApplications.Where(x => x.IntroducerUserId == IntroducerUserId).Select(x=>x.ApplicationId).ToList();

            var IntroduceName = _context.Introducers.Where(x => x.IntroducerId == IntroducerId).Select(x=>x.IntroducerName).FirstOrDefault();

            var IntroducerUser = _context.IntroducerUsers.Where(x => x.UserId == IntroducerUserId).FirstOrDefault();
          
            ViewBag.introducerName = IntroduceName;
            ViewBag.introducerUserName = IntroducerUser.FirstName + " "+ IntroducerUser.LastName;
            var Applciations = (from app in _context.BuisnessProfile
                                join buisnesstype in _context.BuisnessTypes on app.BuisnessTypeId equals buisnesstype.BuisnessTypeId
                                select new BuisnessProfile()
                                {
                                    BuisnessProfileId = app.BuisnessProfileId,
                                    BuisnessTypeName = buisnesstype.Name,
                                    BuisnessName = app.BuisnessName,
                                    SubmitDate = app.SubmitDate,
                                    ActionCount = _context.ProfileComments.Where(x => x.BuisneesProfileId == app.BuisnessProfileId).ToList().Count,
                                }).ToList();
            var NewApplicationss = Applciations.Where(Profile => IntroducersApplications.Contains(Profile.BuisnessProfileId) && Profile.IsDeclined==false && Profile.IsClient==false&& Profile.IsDelete==false).ToList();
           
            
            return View(NewApplicationss);
        }



        public IActionResult ApprovedApplications(int? ComingIntroducerId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int IntroducerUserId = 0;
            if (ComingIntroducerId != null)
            {
                IntroducerUserId = Convert.ToInt32(ComingIntroducerId);
                ViewBag.ComingIntroudcerId = ComingIntroducerId;
            }
            else
            {
                IntroducerUserId = Convert.ToInt32(claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault());
            }
            int IntroducerId = Convert.ToInt32(claims.Where(x => x.Type == "IntroducerId").Select(x => x.Value).FirstOrDefault());
            var IntroducersApplications = _context.IntroducersUsersApplications.Where(x => x.IntroducerUserId == IntroducerUserId).Select(x => x.ApplicationId).ToList();

            var IntroduceName = _context.Introducers.Where(x => x.IntroducerId == IntroducerId).Select(x => x.IntroducerName).FirstOrDefault();

            var IntroducerUser = _context.IntroducerUsers.Where(x => x.UserId == IntroducerUserId).FirstOrDefault();
            ViewBag.introducerName = IntroduceName;
            ViewBag.introducerUserName = IntroducerUser.FirstName + " " + IntroducerUser.LastName;
            var Applciations = (from app in _context.BuisnessProfile
                                join buisnesstype in _context.BuisnessTypes on app.BuisnessTypeId equals buisnesstype.BuisnessTypeId
                                select new BuisnessProfile()
                                {
                                    BuisnessProfileId = app.BuisnessProfileId,
                                    BuisnessTypeName = buisnesstype.Name,
                                    BuisnessName = app.BuisnessName,
                                    SubmitDate = app.SubmitDate,
                                    ApprovedDate = app.ApprovedDate,
                                    ActionCount = _context.ProfileComments.Where(x=>x.BuisneesProfileId == app.BuisnessProfileId).ToList().Count,
                                }).ToList();
            var NewApplicationss = Applciations.Where(Profile => IntroducersApplications.Contains(Profile.BuisnessProfileId) && Profile.IsApproved == true && Profile.IsDeclined == false && Profile.IsClient == false && Profile.IsDelete == false).ToList();
            return View(NewApplicationss);
        }

        public IActionResult SaveProfileComment(ProfileComments objComments)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var IntroducerNAme = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();

              objComments.ActionBy = IntroducerNAme;
              objComments.ActionDate= DateTime.Now.DateTime_UK();

            _context.ProfileComments.Add(objComments);
            _context.SaveChanges();

            return Json(true);

        }

    }
}
