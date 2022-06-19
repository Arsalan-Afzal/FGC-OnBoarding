using FGC_OnBoarding.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using FGC_OnBoarding.Data;
using Microsoft.EntityFrameworkCore;
using FGC_OnBoarding.Areas.Identity.Data;
using FGC_OnBoarding.Models.Buisness;
using System.Text.RegularExpressions;
using System.Net.Mail;
using FGC_OnBoarding.Models.Validations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FGC_OnBoarding.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using FGC_OnBoarding.Models.Users;
using System.Net;
using static FGC_OnBoarding.Areas.Identity.Pages.Account.LoginModel;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;

namespace FGC_OnBoarding.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        //  private readonly ILogger<UserManager> _userManager;
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly FGC_OnBoardingContext _context;
        private readonly IEmailSender _emailSender;
        public int BuisnessProfileSaved = 0;
        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, UserManager<FGC_OnBoardingUser> userManager, FGC_OnBoardingContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        /// <summary>
        /// //////////
        /// </summary>
        /// <param>Buisness Documents</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BuisnessFile1(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            List<int> AttachementIds = new List<int>();
            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> BuisnessFile2(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> BuisnessFile3(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteBuisnessFile1(int Documentid)
        {

            var BuisnessAttachment = _context.BuisnessAttachemtns.Where(x => x.DocumentId == Documentid).FirstOrDefault();

            BuisnessAttachment.IsDelete = true;
            //   _context.BuisnessAttachemtns.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }

        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// //////////
        /// </summary>
        /// <param>Buisness Documents</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CharityFile1(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //   string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CharityFile2(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CharityFile3(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteCharityFile1(int Documentid)
        {

            var BuisnessAttachment = _context.BuisnessAttachemtns.Where(x => x.DocumentId == Documentid).FirstOrDefault();

            BuisnessAttachment.IsDelete = true;//_context.BuisnessAttachemtns.(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }

        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        /// 

        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// //////////
        /// </summary>
        /// <param>Buisness Documents</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CompanyFile1(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //   string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CompanyFile2(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //   string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CompanyFile3(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            try
            {
                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteCompanyFile1(int Documentid)
        {

            var BuisnessAttachment = _context.BuisnessAttachemtns.Where(x => x.DocumentId == Documentid).FirstOrDefault();

            BuisnessAttachment.IsDelete = true;//_context.BuisnessAttachemtns.(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        ///  Sole Personal Documents Saving//////////
        [HttpPost]
        public async Task<IActionResult> SoloPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //   string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Sole1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> SoloPersonalFile2(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Sole2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> SoloPersonalFile3(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                // string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Sole3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteSolePersonalFiles(int Documentid)
        {
            var BuisnessAttachment = _context.PersonalDocuments.Where(x => x.DocumentId == Documentid).FirstOrDefault();
            BuisnessAttachment.IsDelete = true;//_context.BuisnessAttachemtns.(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        /////////////////////
        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        ///  Sole Personal Documents Saving//////////Charity Personal Files
        [HttpPost]
        public async Task<IActionResult> CharityPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Charity1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CharityPersonalFile2(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Charity2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CharityPersonalFile3(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();


            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Charity3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteCharityPersonalFiles(int Documentid)
        {
            var BuisnessAttachment = _context.PersonalDocuments.Where(x => x.DocumentId == Documentid).FirstOrDefault();
            BuisnessAttachment.IsDelete = true;//_context.BuisnessAttachemtns.(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        /////////////////////
        /// <summary>
        /// 
        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        ///  Sole Personal Documents Saving//////////Charity Personal Files
        [HttpPost]
        public async Task<IActionResult> BuisnessPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Buisness1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> BuisnessPersonalFile2(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Buisness2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> BuisnessPersonalFile3(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();


            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == buisnessprofileid).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

                foreach (var formFile in UploadedFiles)
                {
                    //string uref = Reference.GetUniqueReference("EMR");
                    var FileName = DateTime.Now.ToString("dd-MM-yyy-hh-mm-ss") + "-" + formFile.FileName;
                    FileName = FileName.Replace(" ", String.Empty);

                    var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    PersonalDocuments obj = new PersonalDocuments();
                    obj.DocumentType = "Buisness3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);
                    //return Json("Upload image succesfully.");
                }
                return Json(AttachementIds);
            }
            catch (Exception e)
            {
                return Json("Please Try Again !!");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteBuisnessPersonalFiles(int Documentid)
        {
            var BuisnessAttachment = _context.PersonalDocuments.Where(x => x.DocumentId == Documentid).FirstOrDefault();
            BuisnessAttachment.IsDelete = true;

            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        /////////////////////
        public IActionResult Index()
        {
            //IdentityUser applicationUser = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            string user = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserId = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault(); // will give the user's Email
            ViewBag.LoginUserName = UserId;
            //var UsersApplicationCount = _context.BuisnessProfile.Where(x => x.UserId == user && x.IsComplete == true).ToList();
            //if (UsersApplicationCount.Count > 0)
            //{
            //    var PendingApplication = _context.BuisnessProfile.Where(x => x.UserId == user && x.IsComplete == false).FirstOrDefault();
            //    if (PendingApplication != null)
            //    {
            //        return RedirectToAction("NewApplication");
            //    }
            //    else
            //    {
            //        return View();
            //    }

            //}
            //else
            //{

            //    return RedirectToAction("NewApplication");
            //}

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// ///////////////Forms
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> NewApplication(int? BuisnessProfileId)
        {
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);


            //ViewBag.username = applicationUser.Id;

            //var Users =
            ViewBag.BuisnessProfileId = BuisnessProfileId;
            return View();
        }
        public async Task<IActionResult> GetCurrentForm(int? id, int? BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email.
            int CurrentForm = 0;
            var flag = false;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.Form = BuisnessProfile.CurrentForm;
                    CurrentForm = BuisnessProfile.CurrentForm;
                    if (id != null)
                    {
                        if (id == 3)
                        {
                            var BuisnessInformation = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                            if (BuisnessInformation != null)
                            {
                                flag = true;
                                return Json(new { form = CurrentForm, flag = flag });
                            }
                            else
                            {
                                flag = false;
                                return Json(new { form = CurrentForm, flag = flag });
                            }
                        }
                        if (id == 5 || id == 6 || id == 8)
                        {
                            if (BuisnessProfile.BuisnessTypeId == 1)
                            {
                                var OwnerShipInformation = _context.OwnerShip.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                                if (OwnerShipInformation != null)
                                {
                                    flag = true;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                                else
                                {
                                    flag = false;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                            }
                            else if (BuisnessProfile.BuisnessTypeId == 2)
                            {
                                var TrusteeInformation = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                                if (TrusteeInformation != null)
                                {
                                    flag = true;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                                else
                                {
                                    flag = false;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                            }
                            else if (BuisnessProfile.BuisnessTypeId == 3 || BuisnessProfile.BuisnessTypeId == 4)
                            {
                                var DirectorsInformation = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                                if (DirectorsInformation != null)
                                {
                                    flag = true;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                                else
                                {
                                    flag = false;
                                    return Json(new { form = CurrentForm, flag = flag, btid = BuisnessProfile.BuisnessTypeId });
                                }
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                        return Json(new { form = CurrentForm, flag = flag });
                    }
                }
            }
            return Json(true);
        }
        public async Task<IActionResult> ConfirmationMessage()
        {
            return View();
        }
        public async Task<IActionResult> BrowseOldApplications()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            var BuisnessApplications = _context.BuisnessProfile.Where(x => x.UserId == UserId).OrderByDescending(x => x.BuisnessProfileId).ToList();

            return View(BuisnessApplications);
        }
        public async Task<IActionResult> ServiceRequirment(int? BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string Currency = "";
            int BuisnessSector = 0;
            int BuisnessType = 0;
            int CurrentForm = 0;
            BuisnessProfile BuisnessProfile = new BuisnessProfile();
            if (UserId != null)
            {

                if (BuisnessProfileId != null)
                {
                    BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
                }


                if (BuisnessProfile != null)
                {
                    ViewBag.Form = BuisnessProfile.CurrentForm;
                    Currency = BuisnessProfile.CurrencyId;
                    BuisnessSector = Convert.ToInt32(BuisnessProfile.BuisnessSectorId);
                    BuisnessType = BuisnessProfile.BuisnessTypeId;
                    BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    CurrentForm = BuisnessProfile.CurrentForm;
                }
                else
                {
                    CurrentForm = 0;
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
            ViewBag.Currency = Currency;
            ViewBag.BuisnessSector = BuisnessSector;
            ViewBag.BuisnessType = BuisnessType;
            ViewBag.BuisnessProfileId = BuisnessProfileId;
            ViewBag.CurrentForm = CurrentForm;

            if (BuisnessProfile != null)
            {
                return View(BuisnessProfile);
            }
            else
            {
                return View();
            }

        }
        public async Task<IActionResult> ShowBuisnessProfile(int BuisnessProfileId, string CurrencyIds, int BuisnessType, int BuisnessSector)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //var MyBuisnessProfileId = BuisnessProfileId;
            int BuisnessTypeId = 0;
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            if (BuisnessProfileId != 0) //Check If It is Existing Buisness Profile 
            {
                if (UserId != null)
                {
                    var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();

                    var AuthorizeRepresentatives = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.IsDelete == false && x.Isdefault == false).ToList();
                    foreach (var Auth in AuthorizeRepresentatives)
                    {
                        if (Auth.FirstName == null && Auth.LastName == null && Auth.Address1 == null && Auth.Address2 == null && Auth.City == null && Auth.PostCode == null && Auth.County == null && Auth.Country == null && Auth.DOB == null && Auth.PhoneNumber == null && Auth.Email == null && Auth.City == null && Auth.PositionInBuisness == null && Auth.PositionInComany == null && Auth.RoleIncharity == null)
                        {
                            _context.AuthorizedRepresentative.Remove(Auth);
                            await _context.SaveChangesAsync();
                        }
                    }


                    if (BuisnessType == BuisnessProfile.BuisnessTypeId)
                    {
                        if (CurrencyIds != null && BuisnessType != 0 && BuisnessSector != 0)
                        {
                            BuisnessProfile.CurrencyId = CurrencyIds;
                            BuisnessProfile.BuisnessTypeId = BuisnessType;
                            if (BuisnessType != 2)
                            {
                                BuisnessProfile.BuisnessSectorId = BuisnessSector;
                            }
                            //BuisnessProfile.CurrentForm = 1;
                            await _context.SaveChangesAsync();


                        }
                        return View(BuisnessProfile);
                    }
                    else
                    {
                        _context.BuisnessProfile.Remove(BuisnessProfile);
                        await _context.SaveChangesAsync();

                        BuisnessProfile objBuisnessProfile = new BuisnessProfile();
                        objBuisnessProfile.UserId = UserId;
                        if (BuisnessSector != 0)
                        {
                            objBuisnessProfile.BuisnessSectorId = BuisnessSector;
                        }
                        objBuisnessProfile.BuisnessTypeId = BuisnessType;

                        objBuisnessProfile.CurrencyId = CurrencyIds;
                        _context.BuisnessProfile.Add(objBuisnessProfile);
                        await _context.SaveChangesAsync();

                        BuisnessProfileSaved = objBuisnessProfile.BuisnessProfileId;
                        // MyBuisnessProfileId = objBuisnessProfile.BuisnessProfileId;
                        // ViewBag.BuisnessId = MyBuisnessProfileId;
                        return View(objBuisnessProfile);
                    }
                    //BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    //// ViewBag.BuisnessId = MyBuisnessProfileId;
                    //var TodayDAte = DateTime.Now.ToString("dd-MM-yyyy");

                    ////BuisnessProfile.RegistrationDate =Convert.ToDateTime(TodayDAte);
                    ////BuisnessProfile.TradeStartingDate = Convert.ToDateTime(TodayDAte);
                    //ViewBag.BuisnessType = BuisnessTypeId;
                    //return View(BuisnessProfile);


                }
                else
                {
                    // ViewBag.BuisnessId = MyBuisnessProfileId;
                    return Redirect("~/identity/account/login");
                }
            }
            else /// If not Existing Buisness Profile Then create new Buisness Profile
            {

                BuisnessProfile objBuisnessProfile = new BuisnessProfile();
                objBuisnessProfile.UserId = UserId;
                if (BuisnessSector != 0)
                {
                    objBuisnessProfile.BuisnessSectorId = BuisnessSector;
                }
                objBuisnessProfile.BuisnessTypeId = BuisnessType;

                objBuisnessProfile.CurrencyId = CurrencyIds;
                _context.BuisnessProfile.Add(objBuisnessProfile);
                BuisnessProfileSaved = objBuisnessProfile.BuisnessProfileId;
                _context.SaveChanges();
                // MyBuisnessProfileId = objBuisnessProfile.BuisnessProfileId;
                // ViewBag.BuisnessId = MyBuisnessProfileId;
                return View(objBuisnessProfile);


            }
        }
        public async Task<IActionResult> ShowAuthorizeRepresentatives(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var AuthorizedRepresentatives = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.Isdefault == true).FirstOrDefault();
                    if (AuthorizedRepresentatives == null)
                    {
                        AuthorizedRepresentative obj = new AuthorizedRepresentative();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        obj.Isdefault = true;
                        _context.AuthorizedRepresentative.Add(obj);
                        _context.SaveChanges();
                        ViewBag.authorizeId = obj.RepresentativeId;

                        return View(obj);
                    }
                    else
                    {
                        ViewBag.authorizeId = AuthorizedRepresentatives.RepresentativeId;
                        return View(AuthorizedRepresentatives);
                    }
                }
                else
                {
                    ViewBag.BuisnessProfileId = 0;
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }

        }
        public async Task<IActionResult> ShowBuisnessInformation(int BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            // var BuisnessType = 0;

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    var AuthorizeRepresentatives = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.IsDelete == false && x.Isdefault == false).ToList();
                    foreach (var Auth in AuthorizeRepresentatives)
                    {
                        if (Auth.FirstName == null && Auth.LastName == null && Auth.Address1 == null && Auth.Address2 == null && Auth.City == null && Auth.PostCode == null && Auth.County == null && Auth.Country == null && Auth.DOB == null && Auth.PhoneNumber == null && Auth.Email == null && Auth.City == null && Auth.PositionInBuisness == null && Auth.PositionInComany == null && Auth.RoleIncharity == null)
                        {
                            _context.AuthorizedRepresentative.Remove(Auth);
                          await  _context.SaveChangesAsync();
                        }
                    }



                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var BuisnessInformations = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.BuisnessTypeId == BuisnessProfile.BuisnessTypeId).FirstOrDefault();
                    if (BuisnessInformations == null)
                    {
                        BuisnessInformation obj = new BuisnessInformation();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;

                        _context.BuisnessInformation.Add(obj);
                        _context.SaveChanges();
                        return View(obj);
                    }
                    else
                    {
                        return View(BuisnessInformations);
                    }
                }
                else
                {
                    //ViewBag.BuisnessProfileId = 0;
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
        }
        public async Task<IActionResult> ShowFinancialInformation(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //   string UserId = applicationUser?.Id;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var FinancialInformations = _context.FinancialInformation.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                    if (FinancialInformations == null)
                    {
                        FinancialInformation obj = new FinancialInformation();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        // obj.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.FinancialInformation.Add(obj);
                        _context.SaveChanges();
                        return View(obj);
                    }
                    else
                    {
                        return View(FinancialInformations);
                    }
                }
                else
                {
                    //ViewBag.BuisnessProfileId = 0;
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
        }
        public async Task<IActionResult> ShowDirectors(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var Directors = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.Isdefault == true).FirstOrDefault();
                    if (Directors == null)
                    {
                        DirectorAndShareHolders obj = new DirectorAndShareHolders();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        obj.Isdefault = true;
                        _context.DirectorAndShareHolders.Add(obj);
                        _context.SaveChanges();
                        ViewBag.directorId = obj.DirectorId;
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.directorId = Directors.DirectorId;
                        return View(Directors);
                    }
                }
                else
                {
                    //ViewBag.BuisnessProfileId = 0;
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }

        }
        public async Task<IActionResult> ShowTrustees(int BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var Trustees = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.Isdefault == true).FirstOrDefault();
                    if (Trustees == null)
                    {
                        Trustees obj = new Trustees();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        obj.Isdefault = true;
                        _context.Trustees.Add(obj);
                        _context.SaveChanges();
                        ViewBag.TrusteeId = obj.TrusteeId;
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.TrusteeId = Trustees.TrusteeId;
                        return View(Trustees);
                    }
                }
                else
                {
                    //ViewBag.BuisnessProfileId = 0;
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
        }
        public async Task<IActionResult> ShowOwners(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var Owners = _context.OwnerShip.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.Isdefault == true).FirstOrDefault();
                    if (Owners == null)
                    {
                        OwnerShip obj = new OwnerShip();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        obj.Isdefault = true;
                        _context.OwnerShip.Add(obj);
                        _context.SaveChanges();
                        ViewBag.OwnerShipID = obj.OwnerShipID;
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.directorId = Owners.OwnerShipID;
                        Owners.Mydob = Owners.DOB?.ToString("yyyy-MM-dd");
                        return View(Owners);
                    }
                }
                else
                {
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }

        }
        /// <summary>
        /// ///////////////Forms Documents///////////
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ShowCharityDocuments(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    var CharityDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();

                    var Charity1Files = CharityDocs.Where(x => x.DocumentType == "Charity1" && x.IsDelete == false).ToList();
                    var Charity2Files = CharityDocs.Where(x => x.DocumentType == "Charity2" && x.IsDelete == false).ToList();
                    var Charity3Files = CharityDocs.Where(x => x.DocumentType == "Charity3" && x.IsDelete == false).ToList();

                    ViewBag.Charity1 = Charity1Files;
                    ViewBag.Charity2 = Charity2Files;
                    ViewBag.Charity3 = Charity3Files;

                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }
            return View();

        }
        public async Task<IActionResult> ShowBuisnessDocuments(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {

                    var BusinessDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();
                    var Buisness1Files = BusinessDocs.Where(x => x.DocumentType == "Buisness1" && x.IsDelete == false).ToList();
                    var Buisness2Files = BusinessDocs.Where(x => x.DocumentType == "Buisness2" && x.IsDelete == false).ToList();
                    var Buisness3Files = BusinessDocs.Where(x => x.DocumentType == "Buisness3" && x.IsDelete == false).ToList();
                    ViewBag.Buisness1 = Buisness1Files;
                    ViewBag.Buisness2 = Buisness2Files;
                    ViewBag.Buisness3 = Buisness3Files;


                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }
            return View();

        }
        public async Task<IActionResult> ShowCompanyDocuments(int BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    var CompanyDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();

                    var Compnay1Files = CompanyDocs.Where(x => x.DocumentType == "Company1" && x.IsDelete == false).ToList();
                    var Compnay2Files = CompanyDocs.Where(x => x.DocumentType == "Company2" && x.IsDelete == false).ToList();
                    var Compnay3Files = CompanyDocs.Where(x => x.DocumentType == "Company3" && x.IsDelete == false).ToList();

                    ViewBag.Company1 = Compnay1Files;
                    ViewBag.Company2 = Compnay2Files;
                    ViewBag.Company3 = Compnay3Files;

                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }

            return View();
        }
        /// <summary>
        /// ///////////////Forms Documents End///////////
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> ShowSoloPersonalDocuments(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.pop = BuisnessProfile.Ispep;
                    ViewBag.RelationShip = BuisnessProfile.Peprelationship;
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var soleDoc = _context.SoleDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                    if (soleDoc == null)
                    {
                        SoleDocuments obj = new SoleDocuments();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        _context.SoleDocuments.Add(obj);
                        _context.SaveChanges();
                        ViewBag.soleDocId = obj.Id;

                        return View(obj);
                    }
                    else
                    {
                        ViewBag.soleDocId = soleDoc.Id;
                        var SoloPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole1" && x.IsDelete == false).ToList();
                        var SoloPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole2" && x.IsDelete == false).ToList();
                        var SoloPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole3" && x.IsDelete == false).ToList();
                        ViewBag.SoloPersonalDocs1 = SoloPersonalDocs1;
                        ViewBag.SoloPersonalDocs2 = SoloPersonalDocs2;
                        ViewBag.SoloPersonalDocs3 = SoloPersonalDocs3;
                        return View(soleDoc);
                    }
                }
                else
                {
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
        }
        public async Task<IActionResult> ShowcharityPersonalDocuments(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email


            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.pop = BuisnessProfile.Ispep;
                    ViewBag.RelationShip = BuisnessProfile.Peprelationship;
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var CharityDoc = _context.CharityDocument.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                    if (CharityDoc == null)
                    {
                        CharityDocument obj = new CharityDocument();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        _context.CharityDocument.Add(obj);
                        _context.SaveChanges();
                        ViewBag.charotyDocId = obj.Id;
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.charotyDocId = CharityDoc.Id;

                        var CharityPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity1" && x.IsDelete == false).ToList();
                        var CharityPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity2" && x.IsDelete == false).ToList();
                        var CharityPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity3" && x.IsDelete == false).ToList();
                        ViewBag.charityPersonalDocs1 = CharityPersonalDocs1;
                        ViewBag.charityPersonalDocs2 = CharityPersonalDocs2;
                        ViewBag.charityPersonalDocs3 = CharityPersonalDocs3;


                        return View(CharityDoc);
                    }
                }
                else
                {
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }
        }
        public async Task<IActionResult> ShowBuisnessPersonalDocuments(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.pop = BuisnessProfile.Ispep;
                    ViewBag.RelationShip = BuisnessProfile.Peprelationship;
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                    var BuisnessDoc = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                    if (BuisnessDoc == null)
                    {
                        BuisnessDocuments obj = new BuisnessDocuments();
                        obj.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                        _context.BuisnessDocuments.Add(obj);
                        _context.SaveChanges();
                        ViewBag.BuisnessDocId = obj.Id;
                        return View(obj);
                    }
                    else
                    {
                        ViewBag.BuisnessDocId = BuisnessDoc.Id;


                        var BuisnessPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness1" && x.IsDelete == false).ToList();
                        var BuisnessPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness2" && x.IsDelete == false).ToList();
                        var BuisnessPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness3" && x.IsDelete == false).ToList();
                        ViewBag.BuisnessPersonalDocs1 = BuisnessPersonalDocs1;
                        ViewBag.BuisnessPersonalDocs2 = BuisnessPersonalDocs2;
                        ViewBag.BuisnessPersonalDocs3 = BuisnessPersonalDocs3;

                        return View(BuisnessDoc);
                    }
                }
                else
                {
                    return Redirect("~/identity/account/login");
                }
            }
            else
            {
                return Redirect("~/identity/account/login");
            }

        }
        /// <summary>
        /// //////////// Forms End
        /// </summary>
        /// <summary>
        /// ///////////////Show Delaration///////////
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> ShowDecleration(int BuisnessProfileId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }
            return View();
        }
        public async Task<IActionResult> SubmitApplication(int BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();

            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //   string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    BuisnessProfile.IsComplete = true;
                    BuisnessProfile.CurrentStatus = "New";
                    BuisnessProfile.IsOnboarded = true;
                    BuisnessProfile.SubmitDate = DateTime.Now.DateTime_UK();
                    _context.SaveChanges();
                }
                var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 1).Select(x => x.Email).ToList();
                foreach (var itm in Emails)
                {
                    //EmailsString += itm + ",";
                    await _emailSender.SendEmailAsync(itm, "New Application (" + BuisnessProfile.BuisnessName + ")",
                        $"Hi onboarding Team, <br/><br/>A new customer <b>(" + BuisnessProfile.BuisnessName + ")</b> submitted onboarding application, Please check admin page to see for further processing of the application.<br/><br/><br/><br/>" +
                        //"<b>Customer Name: </b>" + UserName + "<br/>" +
                        //"<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        ////"<b>Section Name: </b>PEP DECLERATION<br/>" +
                        ////"<b>Field Name: </b>Is Pep<br/>" +
                        ////"<b>Old Value: </b>" + OldPepYesNo + "<br/>" +
                        ////"<b>New Value: </b>" + NewPepYesNo + "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
                string ip = GetClientIPAddress(HttpContext);
                string Country = GetUserCountryByIp(ip);

                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                //objnewLogs.OldValue = OldPepYesNo;
                //objnewLogs.NewValue = NewPepYesNo;
                //objnewLogs.FormName = "Pep Decleration";
                //objnewLogs.FieldName = "Is Pep";
                objnewLogs.IPAdress = ip;
                objnewLogs.Action = "New Application Submitted";
                objnewLogs.Email = UserEmail;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);
                _context.SaveChanges();
            }

            return RedirectToAction("ConfirmationMessage");
        }
        /// <summary>
        /// //////////// Delaration End
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        /// <summary>
        /// ///////////////Saving Forms Data On FocusOut
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<IActionResult> SaveBuisnessFields(BuisnessProfile objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                if (objFields.BuisnessName != null && objFields.BuisnessName != BuisnessProfile.BuisnessName)
                {
                    BuisnessProfile.BuisnessName = objFields.BuisnessName;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "Buisnessname" && objFields.BuisnessName != BuisnessProfile.BuisnessName)
                    {
                        BuisnessProfile.BuisnessName = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///  ///////////Buisness Address Update//////////
                if (objFields.Address != null && objFields.Address != BuisnessProfile.Address)
                {
                    BuisnessProfile.Address = objFields.Address;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address" && objFields.Address != BuisnessProfile.Address)
                    {
                        BuisnessProfile.Address = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness City Update//////////
                if (objFields.City != null && objFields.City != BuisnessProfile.City)
                {
                    BuisnessProfile.City = objFields.City;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city" && objFields.City != BuisnessProfile.City)
                    {
                        BuisnessProfile.City = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness County Update//////////
                if (objFields.County != null && objFields.County != BuisnessProfile.County)
                {
                    BuisnessProfile.County = objFields.County;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county" && objFields.County != BuisnessProfile.County)
                    {
                        BuisnessProfile.County = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness PostCode Update//////////
                if (objFields.PostCode != null && objFields.PostCode != BuisnessProfile.PostCode)
                {
                    BuisnessProfile.PostCode = objFields.PostCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode" && objFields.PostCode != BuisnessProfile.PostCode)
                    {
                        BuisnessProfile.PostCode = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness Country Update//////////
                if (objFields.Country != null && objFields.Country != BuisnessProfile.Country)
                {
                    BuisnessProfile.Country = objFields.Country;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country" && objFields.Country != BuisnessProfile.Country)
                    {
                        BuisnessProfile.Country = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness BuisnessWebsite Update//////////
                if (objFields.BuisnessWebsite != null)
                {
                    objFields.BuisnessWebsite = objFields.BuisnessWebsite.Trim();

                    Regex pattern = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
                    Match match = pattern.Match(objFields.BuisnessWebsite);
                    if (match.Success == true)
                    {
                        BuisnessProfile.BuisnessWebsite = objFields.BuisnessWebsite;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Json(new { flag = true, validation = "Invalid Website Url" });
                    }
                }
                else
                {
                    if (objFields.FieldName == "buisnesswebsite")
                    {
                        BuisnessProfile.BuisnessWebsite = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness BuisnessEmail Update//////////
                if (objFields.BuisnessEmail != null && objFields.BuisnessEmail != BuisnessProfile.BuisnessEmail)
                {

                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.BuisnessEmail);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            BuisnessProfile.BuisnessEmail = objFields.BuisnessEmail;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            BuisnessProfile.BuisnessEmail = objFields.BuisnessEmail;
                            await _context.SaveChangesAsync();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }
                }
                else
                {
                    if (objFields.FieldName == "buisnessemail" && objFields.BuisnessEmail != BuisnessProfile.BuisnessEmail)
                    {
                        BuisnessProfile.BuisnessEmail = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness UTR Update//////////
                if (objFields.UTR != null && objFields.UTR != BuisnessProfile.UTR)
                {
                    BuisnessProfile.UTR = objFields.UTR;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "utr" && objFields.UTR != BuisnessProfile.UTR)
                    {
                        BuisnessProfile.UTR = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness CharityNumber Update//////////
                if (objFields.CharityNumber != null && objFields.CharityNumber != BuisnessProfile.CharityNumber)
                {
                    BuisnessProfile.CharityNumber = objFields.CharityNumber;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "charitynumber" && objFields.CharityNumber != BuisnessProfile.CharityNumber)
                    {
                        BuisnessProfile.CharityNumber = string.Empty;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness IncorporationNumber Update//////////
                if (objFields.IncorporationNumber != null && objFields.IncorporationNumber != BuisnessProfile.IncorporationNumber)
                {
                    BuisnessProfile.IncorporationNumber = objFields.IncorporationNumber;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "incopnumber" && objFields.IncorporationNumber != BuisnessProfile.IncorporationNumber)
                    {
                        BuisnessProfile.IncorporationNumber = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness NoOfDirectors_Partners Update//////////
                if (objFields.NoOfDirectors_Partners != null && objFields.NoOfDirectors_Partners != BuisnessProfile.NoOfDirectors_Partners)
                {
                    BuisnessProfile.NoOfDirectors_Partners = objFields.NoOfDirectors_Partners;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "noofdirectors" && objFields.NoOfDirectors_Partners != BuisnessProfile.NoOfDirectors_Partners)
                    {
                        BuisnessProfile.NoOfDirectors_Partners = null;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness NoOfTrustees Update//////////
                if (objFields.NoOfTrustees != null && objFields.NoOfTrustees != BuisnessProfile.NoOfTrustees)
                {
                    BuisnessProfile.NoOfTrustees = objFields.NoOfTrustees;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nooftrustees" && objFields.NoOfTrustees != BuisnessProfile.NoOfTrustees)
                    {
                        BuisnessProfile.NoOfTrustees = null;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegistrationDate Update//////////
                if (objFields.RegistrationDate != BuisnessProfile.RegistrationDate)
                {
                    BuisnessProfile.RegistrationDate = objFields.RegistrationDate;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                /////////////////
                ///
                ///  ///////////Buisness TradeStartingDate Update//////////
                if (objFields.TradeStartingDate != BuisnessProfile.TradeStartingDate)
                {
                    BuisnessProfile.TradeStartingDate = objFields.TradeStartingDate;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredAdresss Update//////////
                if (objFields.RegisteredAdresss != null && objFields.RegisteredAdresss != BuisnessProfile.RegisteredAdresss)
                {
                    BuisnessProfile.RegisteredAdresss = objFields.RegisteredAdresss;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "raddress" && objFields.RegisteredAdresss != BuisnessProfile.RegisteredAdresss)
                    {
                        BuisnessProfile.RegisteredAdresss = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCity Update//////////
                if (objFields.RegisteredCity != null && objFields.RegisteredCity != BuisnessProfile.RegisteredCity)
                {
                    BuisnessProfile.RegisteredCity = objFields.RegisteredCity;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcity" && objFields.RegisteredCity != BuisnessProfile.RegisteredCity)
                    {
                        BuisnessProfile.RegisteredCity = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCounty Update//////////
                if (objFields.RegisteredCounty != null && objFields.RegisteredCounty != BuisnessProfile.RegisteredCounty)
                {
                    BuisnessProfile.RegisteredCounty = objFields.RegisteredCounty;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcounty" && objFields.RegisteredCounty != BuisnessProfile.RegisteredCounty)
                    {
                        BuisnessProfile.RegisteredCounty = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredPostCode Update//////////
                if (objFields.RegisteredPostCode != null && objFields.RegisteredPostCode != BuisnessProfile.RegisteredPostCode)
                {
                    BuisnessProfile.RegisteredPostCode = objFields.RegisteredPostCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rpost" && objFields.RegisteredPostCode != BuisnessProfile.RegisteredPostCode)
                    {
                        BuisnessProfile.RegisteredPostCode = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCountry Update//////////
                if (objFields.RegisteredCountry != null && objFields.RegisteredCountry != BuisnessProfile.RegisteredCountry)
                {
                    BuisnessProfile.RegisteredCountry = objFields.RegisteredCountry;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcountry" && objFields.RegisteredCountry != BuisnessProfile.RegisteredCountry)
                    {
                        BuisnessProfile.RegisteredCountry = string.Empty;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                /////////////IF Buisness Registered Adrress is Other than Address//////////////
                if (objFields.RegisteredAdress)
                {
                    BuisnessProfile.RegisteredAdress = objFields.RegisteredAdress;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    BuisnessProfile.RegisteredAdress = false;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                //////////////////////////////////
            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveRepresentativeFields(AuthorizedRepresentative objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                //var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                ///
                var AuthorizedRepresentative = _context.AuthorizedRepresentative.Where(x => x.RepresentativeId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != AuthorizedRepresentative.FirstName)
                {
                    AuthorizedRepresentative.FirstName = objFields.FirstName;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname" && objFields.FirstName != AuthorizedRepresentative.FirstName)
                    {
                        AuthorizedRepresentative.FirstName = objFields.FirstName;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }

                if (objFields.LastName != null && objFields.LastName != AuthorizedRepresentative.LastName)
                {
                    AuthorizedRepresentative.LastName = objFields.LastName;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname" && objFields.LastName != AuthorizedRepresentative.LastName)
                    {
                        AuthorizedRepresentative.LastName = objFields.LastName;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != AuthorizedRepresentative.Country)
                {
                    AuthorizedRepresentative.Country = objFields.Country;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country")
                    {
                        AuthorizedRepresentative.Country = objFields.Country;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Address1 != null && objFields.Address1 != AuthorizedRepresentative.Address1)
                {
                    AuthorizedRepresentative.Address1 = objFields.Address1;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address1" && objFields.Address1 != AuthorizedRepresentative.Address1)
                    {
                        AuthorizedRepresentative.Address1 = objFields.Address1;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Address2 != null && objFields.Address2 != AuthorizedRepresentative.Address2)
                {
                    AuthorizedRepresentative.Address2 = objFields.Address2;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address2" && objFields.Address2 != AuthorizedRepresentative.Address2)
                    {
                        AuthorizedRepresentative.Address2 = objFields.Address2;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != AuthorizedRepresentative.City)
                {
                    AuthorizedRepresentative.City = objFields.City;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city" && objFields.City != AuthorizedRepresentative.City)
                    {
                        AuthorizedRepresentative.City = objFields.City;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != AuthorizedRepresentative.PostCode)
                {
                    AuthorizedRepresentative.PostCode = objFields.PostCode;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode" && objFields.PostCode != AuthorizedRepresentative.PostCode)
                    {
                        AuthorizedRepresentative.PostCode = objFields.PostCode;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != AuthorizedRepresentative.County)
                {
                    AuthorizedRepresentative.County = objFields.County;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county" && objFields.County != AuthorizedRepresentative.County)
                    {
                        AuthorizedRepresentative.County = objFields.County;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.DOB != AuthorizedRepresentative.DOB)
                {
                    AuthorizedRepresentative.DOB = objFields.DOB;
                    await _context.SaveChangesAsync();
                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        AuthorizedRepresentative.DOB = objFields.DOB;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != AuthorizedRepresentative.PhoneNumber)
                {
                    AuthorizedRepresentative.PhoneNumber = objFields.PhoneNumber;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber" && objFields.PhoneNumber != AuthorizedRepresentative.PhoneNumber)
                    {
                        AuthorizedRepresentative.PhoneNumber = objFields.PhoneNumber;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != AuthorizedRepresentative.Email)
                {


                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            AuthorizedRepresentative.Email = objFields.Email;
                            // _context.SaveChanges();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            AuthorizedRepresentative.Email = objFields.Email;
                            await _context.SaveChangesAsync();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }


                    //AuthorizedRepresentative.Email = objFields.Email;
                    ////_context.SaveChanges();

                    ////return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "email" && objFields.Email != AuthorizedRepresentative.Email)
                    {
                        try
                        {
                            var mail = new MailAddress(objFields.Email);
                            bool isValidEmail = mail.Host.Contains(".");
                            if (!isValidEmail)
                            {
                                AuthorizedRepresentative.Email = objFields.Email;
                                return Json(new { flag = true, validation = "Invalid Email Address" });
                            }
                            else
                            {
                                AuthorizedRepresentative.Email = objFields.Email;
                                await _context.SaveChangesAsync();
                            }
                        }
                        catch (Exception)
                        {
                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                    }
                }
                if (objFields.PositionInBuisness != null && objFields.PositionInBuisness != AuthorizedRepresentative.PositionInBuisness)
                {
                    AuthorizedRepresentative.PositionInBuisness = objFields.PositionInBuisness;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "positioninbuisness" && objFields.PositionInBuisness != AuthorizedRepresentative.PositionInBuisness)
                    {
                        AuthorizedRepresentative.PositionInBuisness = objFields.PositionInBuisness;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.RoleIncharity != null && objFields.RoleIncharity != AuthorizedRepresentative.RoleIncharity)
                {
                    AuthorizedRepresentative.RoleIncharity = objFields.RoleIncharity;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "roleincharity" && objFields.RoleIncharity != AuthorizedRepresentative.RoleIncharity)
                    {
                        AuthorizedRepresentative.RoleIncharity = objFields.RoleIncharity;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.PositionInComany != null && objFields.PositionInComany != AuthorizedRepresentative.PositionInComany)
                {
                    AuthorizedRepresentative.PositionInComany = objFields.PositionInComany;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "positionincompany" && objFields.PositionInComany != AuthorizedRepresentative.PositionInComany)
                    {
                        AuthorizedRepresentative.PositionInComany = objFields.PositionInComany;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }

            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveBuisnessInformationFields(BuisnessInformation objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessInformations = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId && x.BuisnessTypeId == BuisnessProfile.BuisnessTypeId).FirstOrDefault();
                if (objFields.Answer1 != null && objFields.Answer1 != BuisnessInformations.Answer1)
                {
                    BuisnessInformations.Answer1 = objFields.Answer1;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1" && objFields.Answer1 != BuisnessInformations.Answer1)
                    {
                        BuisnessInformations.Answer1 = objFields.Answer1;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer2 != null && objFields.Answer2 != BuisnessInformations.Answer2)
                {
                    BuisnessInformations.Answer2 = objFields.Answer2;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2" && objFields.Answer2 != BuisnessInformations.Answer2)
                    {
                        BuisnessInformations.Answer2 = objFields.Answer2;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer3 != null && objFields.Answer3 != BuisnessInformations.Answer3)
                {
                    BuisnessInformations.Answer3 = objFields.Answer3;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3" && objFields.Answer3 != BuisnessInformations.Answer3)
                    {
                        BuisnessInformations.Answer3 = objFields.Answer3;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer4 != null && objFields.Answer4 != BuisnessInformations.Answer4)
                {
                    BuisnessInformations.Answer4 = objFields.Answer4;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4" && objFields.Answer4 != BuisnessInformations.Answer4)
                    {
                        BuisnessInformations.Answer4 = objFields.Answer4;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer5 != null && objFields.Answer5 != BuisnessInformations.Answer5)
                {
                    BuisnessInformations.Answer5 = objFields.Answer5;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5" && objFields.Answer5 != BuisnessInformations.Answer5)
                    {
                        BuisnessInformations.Answer5 = objFields.Answer5;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer6 != null && objFields.Answer6 != BuisnessInformations.Answer6)
                {
                    BuisnessInformations.Answer6 = objFields.Answer6;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6" && objFields.Answer6 != BuisnessInformations.Answer6)
                    {
                        BuisnessInformations.Answer6 = objFields.Answer6;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer7 != null && objFields.Answer7 != BuisnessInformations.Answer7)
                {
                    BuisnessInformations.Answer7 = objFields.Answer7;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7" && objFields.Answer7 != BuisnessInformations.Answer7)
                    {
                        BuisnessInformations.Answer7 = objFields.Answer7;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer8 != null && objFields.Answer8 != BuisnessInformations.Answer8)
                {
                    BuisnessInformations.Answer8 = objFields.Answer8;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8" && objFields.Answer8 != BuisnessInformations.Answer8)
                    {
                        BuisnessInformations.Answer8 = objFields.Answer8;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Answer9 != null && objFields.Answer9 != BuisnessInformations.Answer9)
                {
                    BuisnessInformations.Answer9 = objFields.Answer9;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9" && objFields.Answer9 != BuisnessInformations.Answer9)
                    {
                        BuisnessInformations.Answer9 = objFields.Answer9;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveFinancialInformationFields(FinancialInformation objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var FinancialInformation = _context.FinancialInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.PerMonth != null && objFields.PerMonth != FinancialInformation.PerMonth)
                {
                    FinancialInformation.PerMonth = objFields.PerMonth;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1" && objFields.PerMonth != FinancialInformation.PerMonth)
                    {
                        FinancialInformation.PerMonth = objFields.PerMonth;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PerAnum != null && objFields.PerAnum != FinancialInformation.PerAnum)
                {
                    FinancialInformation.PerAnum = objFields.PerAnum;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2" && objFields.PerAnum != FinancialInformation.PerAnum)
                    {
                        FinancialInformation.PerAnum = objFields.PerAnum;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PaymentIncoming != null && objFields.PaymentIncoming != FinancialInformation.PaymentIncoming)
                {
                    FinancialInformation.PaymentIncoming = objFields.PaymentIncoming;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3" && objFields.PaymentIncoming != FinancialInformation.PaymentIncoming)
                    {
                        FinancialInformation.PaymentIncoming = objFields.PaymentIncoming;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PaymentOutgoing != null && objFields.PaymentOutgoing != FinancialInformation.PaymentOutgoing)
                {
                    FinancialInformation.PaymentOutgoing = objFields.PaymentOutgoing;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4" && objFields.PaymentOutgoing != FinancialInformation.PaymentOutgoing)
                    {
                        FinancialInformation.PaymentOutgoing = objFields.PaymentOutgoing;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.TransactionIncoming != null && objFields.TransactionIncoming != FinancialInformation.TransactionIncoming)
                {
                    FinancialInformation.TransactionIncoming = objFields.TransactionIncoming;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5" && objFields.TransactionIncoming != FinancialInformation.TransactionIncoming)
                    {
                        FinancialInformation.TransactionIncoming = objFields.TransactionIncoming;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.TransactionOutgoing != null && objFields.TransactionOutgoing != FinancialInformation.TransactionOutgoing)
                {
                    FinancialInformation.TransactionOutgoing = objFields.TransactionOutgoing;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6" && objFields.TransactionOutgoing != FinancialInformation.TransactionOutgoing)
                    {
                        FinancialInformation.TransactionOutgoing = objFields.TransactionOutgoing;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.NoOfPaymentsPerMonth != null && objFields.NoOfPaymentsPerMonth != FinancialInformation.NoOfPaymentsPerMonth)
                {
                    FinancialInformation.NoOfPaymentsPerMonth = objFields.NoOfPaymentsPerMonth;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7" && objFields.NoOfPaymentsPerMonth != FinancialInformation.NoOfPaymentsPerMonth)
                    {
                        FinancialInformation.NoOfPaymentsPerMonth = objFields.NoOfPaymentsPerMonth;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.VolumePermonth != null && objFields.VolumePermonth != FinancialInformation.VolumePermonth)
                {
                    FinancialInformation.VolumePermonth = objFields.VolumePermonth;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8" && objFields.VolumePermonth != FinancialInformation.VolumePermonth)
                    {
                        FinancialInformation.VolumePermonth = objFields.VolumePermonth;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.BankName != null && objFields.BankName != FinancialInformation.BankName)
                {
                    FinancialInformation.BankName = objFields.BankName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9" && objFields.BankName != FinancialInformation.BankName)
                    {
                        FinancialInformation.BankName = objFields.BankName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.BankAddress != null && objFields.BankAddress != FinancialInformation.BankAddress)
                {
                    FinancialInformation.BankAddress = objFields.BankAddress;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "10" && objFields.BankAddress != FinancialInformation.BankAddress)
                    {
                        FinancialInformation.BankAddress = objFields.BankAddress;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.SortCode != null && objFields.SortCode != FinancialInformation.SortCode)
                {
                    FinancialInformation.SortCode = objFields.SortCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "11" && objFields.SortCode != FinancialInformation.SortCode)
                    {
                        FinancialInformation.SortCode = objFields.SortCode;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.AccountName != null && objFields.AccountName != FinancialInformation.AccountName)
                {
                    FinancialInformation.AccountName = objFields.AccountName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "12" && objFields.AccountName != FinancialInformation.AccountName)
                    {
                        FinancialInformation.AccountName = objFields.AccountName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.AccountNumber != null && objFields.AccountNumber != FinancialInformation.AccountNumber)
                {
                    FinancialInformation.AccountNumber = objFields.AccountNumber;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "13" && objFields.AccountNumber != FinancialInformation.AccountNumber)
                    {
                        FinancialInformation.AccountNumber = objFields.AccountNumber;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.IBAN != null && objFields.IBAN != FinancialInformation.IBAN)
                {
                    FinancialInformation.IBAN = objFields.IBAN;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "14" && objFields.IBAN != FinancialInformation.IBAN)
                    {
                        FinancialInformation.IBAN = objFields.IBAN;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.SwiftCode != null && objFields.SwiftCode != FinancialInformation.SwiftCode)
                {
                    FinancialInformation.SwiftCode = objFields.SwiftCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "15" && objFields.SwiftCode != FinancialInformation.SwiftCode)
                    {
                        FinancialInformation.SwiftCode = objFields.SwiftCode;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.AccountCurrency != null && objFields.AccountCurrency != FinancialInformation.AccountCurrency)
                {
                    FinancialInformation.AccountCurrency = objFields.AccountCurrency;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "16" && objFields.AccountCurrency != FinancialInformation.AccountCurrency)
                    {
                        FinancialInformation.AccountCurrency = objFields.AccountCurrency;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }

                if (objFields.AccountDetails)
                {
                    FinancialInformation.AccountDetails = objFields.AccountDetails;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    FinancialInformation.AccountDetails = false;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveDirectorsFields(DirectorAndShareHolders objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var Director = _context.DirectorAndShareHolders.Where(x => x.DirectorId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != Director.FirstName)
                {
                    Director.FirstName = objFields.FirstName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname" && objFields.FirstName != Director.FirstName)
                    {
                        Director.FirstName = objFields.FirstName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null && objFields.LastName != Director.LastName)
                {
                    Director.LastName = objFields.LastName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname" && objFields.LastName != Director.LastName)
                    {
                        Director.LastName = objFields.LastName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Address != null && objFields.Address != Director.Address)
                {
                    Director.Address = objFields.Address;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address" && objFields.Address != Director.Address)
                    {
                        Director.Address = objFields.Address;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != Director.City)
                {
                    Director.City = objFields.City;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city" && objFields.City != Director.City)
                    {
                        Director.City = objFields.City;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != Director.PostCode)
                {
                    Director.PostCode = objFields.PostCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode" && objFields.PostCode != Director.PostCode)
                    {
                        Director.PostCode = objFields.PostCode;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != Director.Country)
                {
                    Director.Country = objFields.Country;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country" && objFields.Country != Director.Country)
                    {
                        Director.Country = objFields.Country;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null && objFields.Nationality != Director.Nationality)
                {
                    Director.Nationality = objFields.Nationality;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nationality" && objFields.Nationality != Director.Nationality)
                    {
                        Director.Nationality = objFields.Nationality;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != Director.County)
                {
                    Director.County = objFields.County;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county" && objFields.County != Director.County)
                    {
                        Director.County = objFields.County;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.DOB != Director.DOB)
                {
                    Director.DOB = objFields.DOB;
                    await _context.SaveChangesAsync();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        Director.DOB = objFields.DOB;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != "")
                {
                    Director.PhoneNumber = objFields.PhoneNumber;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber" && objFields.PhoneNumber != Director.PhoneNumber)
                    {
                        Director.PhoneNumber = objFields.PhoneNumber;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != Director.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            Director.Email = objFields.Email;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Director.Email = objFields.Email;
                            await _context.SaveChangesAsync();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        Director.Email = objFields.Email;
                        await _context.SaveChangesAsync();
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }


                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "email" && objFields.Email != Director.Email)
                    {
                        Director.Email = objFields.Email;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.ShareHolders_percentage != null)
                {
                    Director.ShareHolders_percentage = objFields.ShareHolders_percentage;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "shareholders" && objFields.FieldName != Director.FieldName)
                    {
                        Director.ShareHolders_percentage = objFields.ShareHolders_percentage;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
            }
            else
            {
                return Json(false);
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTrusteesFields(Trustees objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var Trustees = _context.Trustees.Where(x => x.TrusteeId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != Trustees.FirstName)
                {
                    Trustees.FirstName = objFields.FirstName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname" && objFields.FirstName != Trustees.FirstName)
                    {
                        Trustees.FirstName = objFields.FirstName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null && objFields.LastName != Trustees.LastName)
                {
                    Trustees.LastName = objFields.LastName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname" && objFields.LastName != Trustees.LastName)
                    {
                        Trustees.LastName = objFields.LastName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Address != null && objFields.Address != Trustees.Address)
                {
                    Trustees.Address = objFields.Address;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address" && objFields.Address != Trustees.Address)
                    {
                        Trustees.Address = objFields.Address;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != Trustees.City)
                {
                    Trustees.City = objFields.City;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city" && objFields.City != Trustees.City)
                    {
                        Trustees.City = objFields.City;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != Trustees.PostCode)
                {
                    Trustees.PostCode = objFields.PostCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode" && objFields.PostCode != Trustees.PostCode)
                    {
                        Trustees.PostCode = objFields.PostCode;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != Trustees.Country)
                {
                    Trustees.Country = objFields.Country;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country" && objFields.Country != Trustees.Country)
                    {
                        Trustees.Country = objFields.Country;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null && objFields.Nationality != Trustees.Nationality)
                {
                    Trustees.Nationality = objFields.Nationality;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nationality" && objFields.Nationality != Trustees.Nationality)
                    {
                        Trustees.Nationality = objFields.Nationality;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != Trustees.County)
                {
                    Trustees.County = objFields.County;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county" && objFields.County != Trustees.County)
                    {
                        Trustees.County = objFields.County;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.DOB != Trustees.DOB)
                {
                    Trustees.DOB = objFields.DOB;
                    await _context.SaveChangesAsync();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        Trustees.DOB = objFields.DOB;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.AppointmentDate != Trustees.AppointmentDate)
                {
                    Trustees.AppointmentDate = objFields.AppointmentDate;
                    await _context.SaveChangesAsync();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "appdate")
                    {
                        Trustees.AppointmentDate = objFields.AppointmentDate;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != Trustees.PhoneNumber)
                {
                    Trustees.PhoneNumber = objFields.PhoneNumber;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber" && objFields.PhoneNumber != Trustees.PhoneNumber)
                    {
                        Trustees.PhoneNumber = objFields.PhoneNumber;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != Trustees.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            Trustees.Email = objFields.Email;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Trustees.Email = objFields.Email;
                            await _context.SaveChangesAsync();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {

                        Trustees.Email = objFields.Email;
                        await _context.SaveChangesAsync();
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }


                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "email" && objFields.Email != Trustees.Email)
                    {
                        Trustees.Email = objFields.Email;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
                if (objFields.Role != null && objFields.Role != Trustees.Role)
                {
                    Trustees.Role = objFields.Role;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "role" && Trustees.Role != objFields.Role)
                    {
                        Trustees.Role = objFields.Role;
                        await _context.SaveChangesAsync();

                        return Json(true);
                    }
                }
            }
            else
            {
                return Json(false);
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOwnersFields(OwnerShip objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var Owner = _context.OwnerShip.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.FirstName != null && objFields.FirstName != Owner.FirstName)
                {
                    Owner.FirstName = objFields.FirstName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1" && objFields.FirstName != Owner.FirstName)
                    {
                        Owner.FirstName = objFields.FirstName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null && objFields.LastName != Owner.LastName)
                {
                    Owner.LastName = objFields.LastName;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2" && objFields.LastName != Owner.LastName)
                    {
                        Owner.LastName = objFields.LastName;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Address != null && objFields.Address != Owner.Address)
                {
                    Owner.Address = objFields.Address;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3" && objFields.Address != Owner.Address)
                    {
                        Owner.Address = objFields.Address;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != Owner.City)
                {
                    Owner.City = objFields.City;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4" && objFields.City != Owner.City)
                    {
                        Owner.City = objFields.City;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != Owner.PostCode)
                {
                    Owner.PostCode = objFields.PostCode;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5" && objFields.PostCode != Owner.PostCode)
                    {
                        Owner.PostCode = objFields.PostCode;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != Owner.County)
                {
                    Owner.County = objFields.County;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6" && objFields.County != Owner.County)
                    {
                        Owner.County = objFields.County;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != Owner.Country)
                {
                    Owner.Country = objFields.Country;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7" && objFields.Country != Owner.Country)
                    {
                        Owner.Country = objFields.Country;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null && objFields.Nationality != Owner.Nationality)
                {
                    Owner.Nationality = objFields.Nationality;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8" && objFields.Nationality != Owner.Nationality)
                    {
                        Owner.Nationality = objFields.Nationality;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.DOB != Owner.DOB)
                {
                    Owner.DOB = objFields.DOB;
                    await _context.SaveChangesAsync();
                    // return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9")
                    {
                        Owner.DOB = objFields.DOB;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != Owner.PhoneNumber)
                {
                    Owner.PhoneNumber = objFields.PhoneNumber;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "10" && objFields.PhoneNumber != Owner.PhoneNumber)
                    {
                        Owner.PhoneNumber = objFields.PhoneNumber;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != Owner.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            Owner.Email = objFields.Email;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Owner.Email = objFields.Email;
                            await _context.SaveChangesAsync();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {

                        Owner.Email = objFields.Email;
                        await _context.SaveChangesAsync();
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }

                    Owner.Email = objFields.Email;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "11" && objFields.Email != Owner.Email)
                    {
                        try
                        {
                            var mail = new MailAddress(objFields.Email);
                            bool isValidEmail = mail.Host.Contains(".");
                            if (!isValidEmail)
                            {
                                Owner.Email = string.Empty;
                                return Json(new { flag = true, validation = "Invalid Email Address" });
                            }
                            else
                            {
                                Owner.Email = string.Empty;
                                await _context.SaveChangesAsync();
                            }
                        }
                        catch (Exception)
                        {
                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                    }
                }
            }
            return Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> SaveIspopData(BuisnessProfile objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                // var SoleDoc = _context.SoleDocuments.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                if (objFields.Ispep)
                {
                    BuisnessProfile.Ispep = objFields.Ispep;
                    await _context.SaveChangesAsync();
                    //return Json(true);
                }
                else
                {
                    BuisnessProfile.Ispep = objFields.Ispep;
                    await _context.SaveChangesAsync();
                    //   return Json(true);
                }
                if (objFields.Peprelationship != null)
                {

                    BuisnessProfile.Peprelationship = objFields.Peprelationship;
                    await _context.SaveChangesAsync();
                    return Json(true);

                }
            }

            return Json(false);

        }
        [HttpPost]
        public async Task<IActionResult> SaveCharityPersonalDocsData(CharityDocument objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDoc = _context.CharityDocument.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.isPop)
                {
                    CharityDoc.isPop = objFields.isPop;
                    await _context.SaveChangesAsync();
                    //return Json(true);
                }
                else
                {
                    CharityDoc.isPop = objFields.isPop;
                    await _context.SaveChangesAsync();
                    //   return Json(true);
                }
                if (objFields.RelationShip != null && objFields.RelationShip != "")
                {

                    CharityDoc.RelationShip = objFields.RelationShip;
                    await _context.SaveChangesAsync();
                    return Json(true);

                }
            }

            return Json(false);

        }
        [HttpPost]
        public async Task<IActionResult> SavebuisnessPersonalDocsData(BuisnessDocuments objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var buisnessDoc = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.isPop)
                {
                    buisnessDoc.isPop = objFields.isPop;
                    await _context.SaveChangesAsync();
                    //return Json(true);
                }
                else
                {
                    buisnessDoc.isPop = objFields.isPop;
                    await _context.SaveChangesAsync();
                    //   return Json(true);
                }
                if (objFields.RelationShip != null && objFields.RelationShip != "")
                {

                    buisnessDoc.RelationShip = objFields.RelationShip;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
            }
            return Json(false);

        }
        /// <summary>
        /// ///////////////Saving Forms Data On FocusOut End
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// ///////////////Validate Forms
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ValidateBuisnessFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            if (BuisnessProfile.BuisnessName == null || BuisnessProfile.BuisnessName == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "Enter Buisness Name" });
            }
            if (BuisnessProfile.Address == null || BuisnessProfile.Address == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "Enter Address" });
            }
            if (BuisnessProfile.City == null || BuisnessProfile.City == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "Enter City" });
            }
            if (BuisnessProfile.PostCode == null || BuisnessProfile.PostCode == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "Enter Postcode" });
            }
            if (BuisnessProfile.Country == null || BuisnessProfile.Country == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "Select Country" });
            }
            if (BuisnessProfile.TradeStartingDate == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation11", Message = "Select Date" });
            }
            if (BuisnessProfile.RegistrationDate == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Select Date" });
            }
            //if (BuisnessProfile.BuisnessEmail != null)
            //{

            //    try
            //    {
            //        // string email = "ar@gmail.com";
            //        //Console.WriteLine($"The email is {email}");
            //        var mail = new MailAddress(BuisnessProfile.BuisnessEmail);
            //        bool isValidEmail = mail.Host.Contains(".");
            //        if (!isValidEmail)
            //        {
            //            // Console.WriteLine($"The email is invalid");
            //            //BuisnessProfile.BuisnessEmail = BuisnessProfile.BuisnessEmail;
            //            //_context.SaveChanges();

            //            //return Json(new { flag = true, validation = "Invalid Email Address" });
            //            objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "Invalid Email Address" });
            //        }
            //        //else
            //        //{
            //        //    // Console.WriteLine($"The email is valid");
            //        //    BuisnessProfile.BuisnessEmail = objFields.BuisnessEmail;
            //        //    _context.SaveChanges();
            //        //}
            //        //  Console.ReadLine();
            //    }
            //    catch (Exception)
            //    {
            //        objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "Invalid Email Address" });
            //        //  Console.ReadLine();
            //    }
            //}
            if (BuisnessProfile.BuisnessEmail == null || BuisnessProfile.BuisnessEmail == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "Enter Buisness Email" });
            }
            if (BuisnessProfile.BuisnessTypeId == 1)
            {
                if (BuisnessProfile.UTR == null || BuisnessProfile.UTR == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "Enter UTR" });
                }

            }
            else if (BuisnessProfile.BuisnessTypeId == 2)
            {
                if (BuisnessProfile.CharityNumber == null || BuisnessProfile.CharityNumber == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "Enter Charity Number" });
                }
                if (BuisnessProfile.NoOfTrustees == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation12", Message = "Enter Trustees Number" });
                }
            }
            else if (BuisnessProfile.BuisnessTypeId == 3 || BuisnessProfile.BuisnessTypeId == 4)
            {
                if (BuisnessProfile.IncorporationNumber == null || BuisnessProfile.IncorporationNumber == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "Enter Incorporation Number" });

                }
                if (BuisnessProfile.NoOfDirectors_Partners == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation12", Message = "Enter Directors Number" });
                }
            }

            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateBuisnessInfoFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var BuisnessInformation = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.BuisnessTypeId == BuisnessProfile.BuisnessTypeId).FirstOrDefault();

            if (BuisnessInformation.Answer1 == "" || BuisnessInformation.Answer1 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "This field is mandatory" });
            }
            if (BuisnessInformation.Answer2 == "" || BuisnessInformation.Answer2 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "This field is mandatory" });
            }
            if (BuisnessInformation.Answer3 == "" || BuisnessInformation.Answer3 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "This field is mandatory" });
            }
            if (BuisnessInformation.Answer4 == "" || BuisnessInformation.Answer4 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "This field is mandatory" });
            }
            if (BuisnessInformation.Answer5 == "" || BuisnessInformation.Answer5 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "This field is mandatory" });
            }
            if (BuisnessInformation.BuisnessTypeId == 2)
            {
                if (BuisnessInformation.Answer6 == "" || BuisnessInformation.Answer6 == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "This field is mandatory" });
                }
                if (BuisnessInformation.Answer9 == "" || BuisnessInformation.Answer9 == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "This field is mandatory" });
                }
            }
            else
            {
                if (BuisnessInformation.Answer7 == "" || BuisnessInformation.Answer7 == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "This field is mandatory" });
                }

            }
            if (BuisnessInformation.Answer8 == "" || BuisnessInformation.Answer8 == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "This field is mandatory" });
            }
            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateFinancialInfoFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var FinancialInformation = _context.FinancialInformation.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

            if (FinancialInformation.PerMonth == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "This field is mandatory" });
            }
            if (FinancialInformation.PerAnum == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "This field is mandatory" });
            }
            if (FinancialInformation.PaymentIncoming == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "This field is mandatory" });
            }
            if (FinancialInformation.PaymentOutgoing == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "This field is mandatory" });
            }
            if (FinancialInformation.TransactionIncoming == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "This field is mandatory" });
            }
            if (FinancialInformation.TransactionOutgoing == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "This field is mandatory" });
            }
            if (FinancialInformation.NoOfPaymentsPerMonth == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "This field is mandatory" });
            }
            if (FinancialInformation.VolumePermonth == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "This field is mandatory" });
            }

            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateDirectorsFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var DirectorsInformations = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.IsDelete == false).ToList();
            foreach (var Director in DirectorsInformations)
            {
                if (Director.FirstName == null || Director.FirstName == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.LastName == null || Director.LastName == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.Address == null || Director.Address == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.City == null || Director.City == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.PostCode == null || Director.PostCode == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.Country == null || Director.Country == "" || Director.Country == "Select country")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.Nationality == null || Director.Nationality == "" || Director.Country == "Select nationality")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.DOB == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.PhoneNumber == null || Director.PhoneNumber == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                if (Director.Email == null || Director.Email == "")
                {





                    objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
                else
                {

                    try
                    {
                        var mail = new MailAddress(Director.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address", FormId = Director.DirectorId });

                            // return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                    }
                    catch (Exception)
                    {

                        objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address", FormId = Director.DirectorId });
                        //  Console.ReadLine();
                    }



                    // Console.WriteLine($"The email is invalid");


                }
                if (Director.ShareHolders_percentage == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation11", Message = "This field is mandatory", FormId = Director.DirectorId });
                }
            }
            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateOwnersFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var OwnersInformation = _context.OwnerShip.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.IsDelete == false).FirstOrDefault();

            if (OwnersInformation.FirstName == null || OwnersInformation.FirstName == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "This field is mandatory" });
            }
            if (OwnersInformation.LastName == null || OwnersInformation.LastName == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "This field is mandatory" });
            }
            if (OwnersInformation.Address == null || OwnersInformation.Address == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "This field is mandatory" });
            }
            if (OwnersInformation.City == null || OwnersInformation.City == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "This field is mandatory" });
            }
            if (OwnersInformation.PostCode == null || OwnersInformation.PostCode == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "This field is mandatory" });
            }
            if (OwnersInformation.Country == null || OwnersInformation.Country == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "This field is mandatory" });
            }
            if (OwnersInformation.Nationality == null || OwnersInformation.Nationality == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "This field is mandatory" });
            }
            if (OwnersInformation.DOB == null)
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "This field is mandatory" });
            }
            if (OwnersInformation.PhoneNumber == null || OwnersInformation.PhoneNumber == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "This field is mandatory" });
            }
            if (OwnersInformation.Email == null || OwnersInformation.Email == "")
            {
                objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "This field is mandatory" });
            }
            else
            {
                try
                {
                    var mail = new MailAddress(OwnersInformation.Email);
                    bool isValidEmail = mail.Host.Contains(".");
                    if (!isValidEmail)
                    {
                        objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address" });
                        // return Json(new { flag = true, validation = "Invalid Email Address" });
                    }
                }
                catch (Exception)
                {

                    objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address" });
                    //  Console.ReadLine();
                }
                // Console.WriteLine($"The email is invalid");
            }
            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateTrusteeFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var TrusteesInformations = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId && x.IsDelete == false).ToList();
            foreach (var Trustee in TrusteesInformations)
            {
                if (Trustee.FirstName == null || Trustee.FirstName == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation1", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.LastName == null || Trustee.LastName == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation2", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.Address == null || Trustee.Address == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation3", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.City == null || Trustee.City == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation4", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.PostCode == null || Trustee.PostCode == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation5", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.Country == null || Trustee.Country == "" || Trustee.Country == "Select country")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation6", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.Nationality == null || Trustee.Nationality == "" || Trustee.Country == "Select nationality")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation7", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.DOB == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation8", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.PhoneNumber == null || Trustee.PhoneNumber == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.Email == null || Trustee.Email == "")
                {

                    objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                else
                {

                    try
                    {
                        var mail = new MailAddress(Trustee.Email);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            //Trustees.Email = objFields.Email;
                            //await _context.SaveChangesAsync();
                            objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address", FormId = Trustee.TrusteeId });
                            // return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                    }
                    catch (Exception)
                    {

                        objValidations.Add(new ModelValidations { Validationid = "Validation10", Message = "Invalid Email Address", FormId = Trustee.TrusteeId });
                        //  Console.ReadLine();
                    }



                    // Console.WriteLine($"The email is invalid");


                }
                if (Trustee.Role == null || Trustee.Role == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation11", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
                if (Trustee.AppointmentDate == null)
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation12", Message = "This field is mandatory", FormId = Trustee.TrusteeId });
                }
            }
            return Json(objValidations);
        }
        /// <summary>
        /// ///////////////Validate Forms End
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// ///////////////Setting Current Form Index so User Can See That Form which he was filling last time
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SetCurrentForm(int Form, int BuisnessProfileId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    BuisnessProfile.CurrentForm = Form;
                    _context.SaveChanges();
                }
            }

            return Json(true);
        }
        /// <summary>
        /// ///////////////Setting Current Form End
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// ///////////////Representative Form Related Data
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> CreateNewRepresentative(int BuisnessProfileId)
        {
            AuthorizedRepresentative objnew = new AuthorizedRepresentative();
            objnew.BuisnessProfileId = BuisnessProfileId;
            _context.AuthorizedRepresentative.Add(objnew);
            _context.SaveChanges();
            return Json(objnew.RepresentativeId);

        }
        public async Task<IActionResult> RemoveRepresentative(int RepresentativeId)
        {

            var Authorized = _context.AuthorizedRepresentative.Where(x => x.RepresentativeId == RepresentativeId).FirstOrDefault();

            Authorized.IsDelete = true;
            _context.SaveChanges();
            return Json(true);
        }
        public async Task<IActionResult> GetAllRepresentative(int BuisnessProfileId)
        {

            var Authorized = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false && x.IsDelete == false).ToList();
            foreach (var auth in Authorized)
            {
                if (auth.DOB != null)
                {
                    auth.DateOfBirth = auth.DOB?.ToString("yyyy-MM-dd");
                }
                else

                {
                    auth.DateOfBirth = "yyyy-MM-dd";
                }
            }

            return Json(Authorized);
        }
        /// <summary>
        /// ///////////////Representative Form Related Data End
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// ///////////////Directors Form Related Data
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> CreateNewDirector(int BuisnessProfileId)
        {
            DirectorAndShareHolders objnew = new DirectorAndShareHolders();
            objnew.BuisnessProfileId = BuisnessProfileId;
            _context.DirectorAndShareHolders.Add(objnew);
            _context.SaveChanges();
            return Json(objnew.DirectorId);

        }
        public async Task<IActionResult> RemoveDirector(int DirectorId)
        {

            var Director = _context.DirectorAndShareHolders.Where(x => x.DirectorId == DirectorId).FirstOrDefault();

            Director.IsDelete = true;
            _context.SaveChanges();

            var LastDirector = _context.DirectorAndShareHolders.OrderByDescending(x => x.DirectorId).FirstOrDefault();
            if (LastDirector != null)
            {
                return Json(new { directorid = LastDirector.DirectorId });
            }
            else
            {
                return Json(new { directorid = 0 });
            }

        }
        public async Task<IActionResult> GetAllDirectors(int BuisnessProfileId)
        {
            var Directors = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false && x.IsDelete == false).ToList();

            foreach (var dir in Directors)
            {
                if (dir.DOB != null)
                {
                    dir.Dateofbirth = dir.DOB?.ToString("yyyy-MM-dd");
                }
                else
                {
                    dir.Dateofbirth = "yyyy-MM-dd";
                }
            }


            return Json(Directors);
        }
        /// <summary>
        /// ///////////////Directors Form Related Data End
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// ///////////////Trustees Form Related Data
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> CreateNewTrustee(int BuisnessProfileId)
        {
            Trustees objnew = new Trustees();
            objnew.BuisnessProfileId = BuisnessProfileId;
            _context.Trustees.Add(objnew);
            _context.SaveChanges();
            return Json(objnew.TrusteeId);

        }
        public async Task<IActionResult> RemoveTrustee(int TrusteeId)
        {

            var Trustee = _context.Trustees.Where(x => x.TrusteeId == TrusteeId).FirstOrDefault();

            Trustee.IsDelete = true;
            _context.SaveChanges();
            var LastTrustee = _context.Trustees.OrderByDescending(x => x.TrusteeId).FirstOrDefault();
            if (LastTrustee != null)
            {
                return Json(new { trusteeId = LastTrustee.TrusteeId });
            }
            else
            {
                return Json(new { trusteeId = 0 });
            }
        }
        public async Task<IActionResult> GetAllTrustees(int BuisnessProfileId)
        {
            var Trustees = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false && x.IsDelete == false).ToList();
            foreach (var dir in Trustees)
            {
                if (dir.DOB != null)
                {
                    dir.Dateofbirth = dir.DOB?.ToString("yyyy-MM-dd");
                }
                else
                {
                    dir.Dateofbirth = "yyyy-MM-dd";
                }
                if (dir.AppointmentDate != null)
                {
                    dir.DateofAppointment = dir.AppointmentDate?.ToString("yyyy-MM-dd");
                }
                else
                {
                    dir.DateofAppointment = "yyyy-MM-dd";
                }
            }
            return Json(Trustees);
        }
        /// <summary>
        /// ///////////////Directors Form Related Data End
        /// </summary>
        /// <returns></returns>
        /// 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> EditApplication(int BuisnessProfileId, string Link)
        {
            var profileid = Convert.ToInt32(BuisnessProfileId);
            var Application = _context.BuisnessProfile.Include(s => s.BuisnessSector).Where(x => x.BuisnessProfileId == profileid).FirstOrDefault();

            ViewBag.link = Link;
            var CountriesList = _context.Countries.Select(x => x.CountryName).ToList();


            ViewBag.Countries = CountriesList;
            var AuthorizedRepresentatives = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();

            var buisnessType = _context.BuisnessTypes.Where(x => x.BuisnessTypeId == Application.BuisnessTypeId).FirstOrDefault();


            var ApplicationData = (from info in _context.BuisnessProfile
                                   where info.BuisnessProfileId == profileid
                                   select new ParentProfile
                                   {
                                       objBuisness = info,
                                       Authorzels = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == profileid && x.IsDelete == false).ToList(),
                                       BuisnessInformation = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault(),
                                       FinancialInformation = _context.FinancialInformation.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault(),
                                       DirectorAndShareHoldersls = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == profileid && x.IsDelete == false).ToList(),
                                       Trusteesls = _context.Trustees.Where(x => x.BuisnessProfileId == profileid && x.IsDelete == false && x.IsDelete == false).ToList(),
                                       //   OwnerShip = _context.OwnerShip.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault(),

                                   }).FirstOrDefault();


            if (ApplicationData.objBuisness.BuisnessTypeId == 1)
            {
                ApplicationData.Buisness1 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Buisness1" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness2 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Buisness2" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness3 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Buisness3" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();

                var soleDoc = _context.SoleDocuments.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault();

                ApplicationData.Personal1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole1" && x.IsDelete == false).ToList();
                ApplicationData.Personal2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole2" && x.IsDelete == false).ToList();
                ApplicationData.Personal3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole3" && x.IsDelete == false).ToList();

                ApplicationData.OwnerShip = _context.OwnerShip.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault();

            }
            else if (ApplicationData.objBuisness.BuisnessTypeId == 2)
            {
                var CharityDoc = _context.CharityDocument.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault();

                ApplicationData.Personal1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity1" && x.IsDelete == false).ToList();
                ApplicationData.Personal2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity2" && x.IsDelete == false).ToList();
                ApplicationData.Personal3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity3" && x.IsDelete == false).ToList();

                ApplicationData.Buisness1 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Charity1" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness2 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Charity2" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness3 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Charity3" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
            }
            else if (ApplicationData.objBuisness.BuisnessTypeId == 3 || ApplicationData.objBuisness.BuisnessTypeId == 4)
            {
                var BuisnessDoc = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == profileid).FirstOrDefault();

                ApplicationData.Personal1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness1" && x.IsDelete == false).ToList();
                ApplicationData.Personal2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness2" && x.IsDelete == false).ToList();
                ApplicationData.Personal3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness3" && x.IsDelete == false).ToList();

                ApplicationData.Buisness1 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Company1" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness2 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Company2" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
                ApplicationData.Buisness3 = _context.BuisnessAttachemtns.Where(x => x.DocumentType == "Company3" && x.BuisnessProfileId == profileid && x.IsDelete == false).ToList();
            }

            ApplicationData.AuthorizedRepresentative = new AuthorizedRepresentative();
            ApplicationData.DirectorAndShareHolders = new DirectorAndShareHolders();
            ApplicationData.Trustees = new Trustees();

            ViewBag.BuisnessTypeName = buisnessType.Name;

            ViewBag.BuisnesssTypeId = Application.BuisnessTypeId;
            ViewBag.BuisnesssProfileId = Application.BuisnessProfileId;
            ViewBag.BuisnessName = Application.BuisnessName;
            ViewBag.SubmitDate = Application.BuisnessName;
            var CurrencyIds = (dynamic)null;
            var Currencies = "";
            if (Application.CurrencyId.Contains(','))
            {
                CurrencyIds = Application.CurrencyId.Split(',');
                foreach (var curr in CurrencyIds)
                {
                    int CurrencyId = Convert.ToInt32(curr);
                    var CurrencyName = _context.Currency.Where(x => x.CurrencyId == CurrencyId).Select(x => x.Name).FirstOrDefault();

                    Currencies += CurrencyName + ",";
                }

            }
            else
            {
                int CurrencyId = Convert.ToInt32(Application.CurrencyId);
                var CurrencyName = _context.Currency.Where(x => x.CurrencyId == CurrencyId).Select(x => x.Name).FirstOrDefault();
                Currencies = CurrencyName;
            }

            ViewBag.currency = Currencies;
            return View(ApplicationData);
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        public async Task<IActionResult> GetProfileComments(int BuisnessProfileId)
        {



            //  var ProfileComments = _context.ProfileComments.Where(x => x.BuisneesProfileId == BuisnessProfileId).ToList();


            var ProfileComments = (from comments in _context.ProfileComments
                                   where comments.BuisneesProfileId == BuisnessProfileId
                                   select new ProfileComments()
                                   {
                                       BuisneesProfileId = comments.BuisneesProfileId,
                                       Id=comments.Id,
                                       ActionBy=comments.ActionBy,
                                       ActionDateStr=comments.ActionDate.Value.ToString("dd-MMM-yyyy"),
                                       Comments=comments.Comments
                                   }).ToList();



            return Json(ProfileComments);
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
