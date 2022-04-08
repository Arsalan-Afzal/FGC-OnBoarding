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
        public HomeController(ILogger<HomeController> logger, UserManager<FGC_OnBoardingUser> userManager, FGC_OnBoardingContext context, IWebHostEnvironment env)
        {
            _logger = logger;
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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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

            _context.BuisnessAttachemtns.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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

            _context.BuisnessAttachemtns.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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
                    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    string UserId = applicationUser?.Id; // will give the user's Email
                    var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

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

            _context.BuisnessAttachemtns.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            _context.PersonalDocuments.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            _context.PersonalDocuments.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            try
            {
                IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                string UserId = applicationUser?.Id; // will give the user's Email
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            _context.PersonalDocuments.Remove(BuisnessAttachment);
            _context.SaveChanges();

            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            return Json(true);
        }
        /////////////////////
        public IActionResult Index()
        {
            IdentityUser applicationUser =  _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            string user = applicationUser?.Id;
            string UserId = applicationUser?.UserName; // will give the user's Email
            ViewBag.LoginUserName = UserId;
            var UsersApplicationCount = _context.BuisnessProfile.Where(x => x.UserId == user && x.IsComplete == true).ToList();
            if(UsersApplicationCount.Count > 0)
            {
                return View();
            }
            else
            {
              return  RedirectToAction("NewApplication");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// ///////////////Forms
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> NewApplication()
        {
            
            
            return View();
        }
        public async Task<IActionResult> GetCurrentForm(int? id)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email.
            int CurrentForm = 0;
            var flag = false;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
                        if(id == 5 || id== 6 || id == 8)
                        {
                            if(BuisnessProfile.BuisnessTypeId == 1)
                            {
                                var OwnerShipInformation = _context.OwnerShip.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                                if (OwnerShipInformation != null)
                                {
                                    flag = true;
                                    return Json(new { form = CurrentForm, flag = flag ,btid = BuisnessProfile.BuisnessTypeId });
                                }
                                else
                                {
                                    flag = false;
                                    return Json(new { form = CurrentForm, flag = flag , btid = BuisnessProfile.BuisnessTypeId });
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
                                    return Json(new { form = CurrentForm, flag = flag , btid = BuisnessProfile.BuisnessTypeId });
                                }
                            }
                            else if (BuisnessProfile.BuisnessTypeId == 3 || BuisnessProfile.BuisnessTypeId == 4)
                            {
                                var DirectorsInformation = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();
                                if (DirectorsInformation != null)
                                {
                                    flag = true;
                                    return Json(new { form = CurrentForm, flag = flag , btid = BuisnessProfile.BuisnessTypeId });
                                }
                                else
                                {
                                    flag = false;
                                    return Json(new { form = CurrentForm, flag = flag , btid = BuisnessProfile.BuisnessTypeId });
                                }
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                        return Json(new { form = CurrentForm , flag = flag });
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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            var BuisnessApplications = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.IsComplete == true).ToList();

            return View(BuisnessApplications);
        }
        public async Task<IActionResult> ServiceRequirment()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            string Currency = "";
            int BuisnessSector = 0;
            int BuisnessType = 0;
            int BuisnessProfileId = 0;
            int CurrentForm = 0;

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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


            return View();
        }
        public async Task<IActionResult> ShowBuisnessProfile(int BuisnessProfileId, string CurrencyIds, int BuisnessType, int BuisnessSector)
        {


            //var MyBuisnessProfileId = BuisnessProfileId;
            int BuisnessTypeId = 0;
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (BuisnessProfileId != 0) //Check If It is Existing Buisness Profile 
            {
                if (UserId != null)
                {
                    var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();

                    if (BuisnessProfile != null)
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
                            _context.SaveChanges();
                        }
                        BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        // ViewBag.BuisnessId = MyBuisnessProfileId;
                        var TodayDAte = DateTime.Now.ToString("dd-MM-yyyy");

                        //BuisnessProfile.RegistrationDate =Convert.ToDateTime(TodayDAte);
                        //BuisnessProfile.TradeStartingDate = Convert.ToDateTime(TodayDAte);
                        ViewBag.BuisnessType = BuisnessTypeId;
                        return View(BuisnessProfile);
                    }
                    else
                    {
                        //ViewBag.BuisnessId = MyBuisnessProfileId;
                        return View(new BuisnessProfile());
                    }
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

                _context.BuisnessProfile.Add(objBuisnessProfile);
                _context.SaveChanges();
                // MyBuisnessProfileId = objBuisnessProfile.BuisnessProfileId;
                // ViewBag.BuisnessId = MyBuisnessProfileId;
                return View(objBuisnessProfile);


            }
        }
        public async Task<IActionResult> ShowAuthorizeRepresentatives()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
                                                 // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
        public async Task<IActionResult> ShowBuisnessInformation()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
                                                 // var BuisnessType = 0;

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
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
        public async Task<IActionResult> ShowFinancialInformation()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
        public async Task<IActionResult> ShowDirectors()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
                                                 // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
        public async Task<IActionResult> ShowTrustees()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
                                                 // var BuisnessType = 0;
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
        public async Task<IActionResult> ShowOwners()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
        public async Task<IActionResult> ShowCharityDocuments()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    var CharityDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();

                    var Charity1Files = CharityDocs.Where(x => x.DocumentType == "Charity1").ToList();
                    var Charity2Files = CharityDocs.Where(x => x.DocumentType == "Charity2").ToList();
                    var Charity3Files = CharityDocs.Where(x => x.DocumentType == "Charity3").ToList();

                    ViewBag.Charity1 = Charity1Files;
                    ViewBag.Charity2 = Charity2Files;
                    ViewBag.Charity3 = Charity3Files;

                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }
            return View();

        }
        public async Task<IActionResult> ShowBuisnessDocuments()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {

                    var BusinessDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();
                    var Buisness1Files = BusinessDocs.Where(x => x.DocumentType == "Buisness1").ToList();
                    var Buisness2Files = BusinessDocs.Where(x => x.DocumentType == "Buisness2").ToList();
                    var Buisness3Files = BusinessDocs.Where(x => x.DocumentType == "Buisness3").ToList();
                    ViewBag.Buisness1 = Buisness1Files;
                    ViewBag.Buisness2 = Buisness2Files;
                    ViewBag.Buisness3 = Buisness3Files;


                    ViewBag.BuisnessType = BuisnessProfile.BuisnessTypeId;
                    ViewBag.BuisnessProfileId = BuisnessProfile.BuisnessProfileId;
                }
            }
            return View();

        }
        public async Task<IActionResult> ShowCompanyDocuments()
        {

            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    var CompanyDocs = _context.BuisnessAttachemtns.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();
                   
                    var Compnay1Files = CompanyDocs.Where(x => x.DocumentType == "Company1").ToList();
                    var Compnay2Files = CompanyDocs.Where(x => x.DocumentType == "Company2").ToList();
                    var Compnay3Files = CompanyDocs.Where(x => x.DocumentType == "Company3").ToList();
                    
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
        public async Task<IActionResult> ShowSoloPersonalDocuments()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
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
                        var SoloPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType=="Sole1").ToList();
                        var SoloPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole2").ToList();
                        var SoloPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == soleDoc.Id && x.DocumentType == "Sole3").ToList();
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
        public async Task<IActionResult> ShowcharityPersonalDocuments()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email


            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
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

                        var CharityPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity1").ToList();
                        var CharityPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity1").ToList();
                        var CharityPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == CharityDoc.Id && x.DocumentType == "Charity1").ToList();
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
        public async Task<IActionResult> ShowBuisnessPersonalDocuments()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
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


                        var BuisnessPersonalDocs1 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness1").ToList();
                        var BuisnessPersonalDocs2 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness2").ToList();
                        var BuisnessPersonalDocs3 = _context.PersonalDocuments.Where(x => x.DocumentTypeId == BuisnessDoc.Id && x.DocumentType == "Buisness3").ToList();
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
        public async Task<IActionResult> ShowDecleration()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.BuisnessProfileId == BuisnessProfileId).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                if (BuisnessProfile != null)
                {
                    BuisnessProfile.IsComplete = true;
                    BuisnessProfile.SubmitDate = DateTime.Now;
                    _context.SaveChanges();
                }
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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                if (objFields.BuisnessName != null)
                {
                    BuisnessProfile.BuisnessName = objFields.BuisnessName;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "Buisnessname")
                    {
                        BuisnessProfile.BuisnessName = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///  ///////////Buisness Address Update//////////
                if (objFields.Address != null)
                {
                    BuisnessProfile.Address = objFields.Address;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address")
                    {
                        BuisnessProfile.Address = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness City Update//////////
                if (objFields.City != null)
                {
                    BuisnessProfile.City = objFields.City;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city")
                    {
                        BuisnessProfile.City = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness County Update//////////
                if (objFields.County != null)
                {
                    BuisnessProfile.County = objFields.County;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county")
                    {
                        BuisnessProfile.County = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness PostCode Update//////////
                if (objFields.PostCode != null)
                {
                    BuisnessProfile.PostCode = objFields.PostCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode")
                    {
                        BuisnessProfile.PostCode = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness Country Update//////////
                if (objFields.Country != null)
                {
                    BuisnessProfile.Country = objFields.Country;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country")
                    {
                        BuisnessProfile.Country = string.Empty;
                        _context.SaveChanges();

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
                        _context.SaveChanges();
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
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness BuisnessEmail Update//////////
                if (objFields.BuisnessEmail != null)
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
                            _context.SaveChanges();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            BuisnessProfile.BuisnessEmail = objFields.BuisnessEmail;
                            _context.SaveChanges();
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
                    if (objFields.FieldName == "buisnessemail")
                    {
                        BuisnessProfile.BuisnessEmail = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness UTR Update//////////
                if (objFields.UTR != null)
                {
                    BuisnessProfile.UTR = objFields.UTR;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "utr")
                    {
                        BuisnessProfile.UTR = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness CharityNumber Update//////////
                if (objFields.CharityNumber != null)
                {
                    BuisnessProfile.CharityNumber = objFields.CharityNumber;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "charitynumber")
                    {
                        BuisnessProfile.CharityNumber = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness IncorporationNumber Update//////////
                if (objFields.IncorporationNumber != null)
                {
                    BuisnessProfile.IncorporationNumber = objFields.IncorporationNumber;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "incopnumber")
                    {
                        BuisnessProfile.IncorporationNumber = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness NoOfDirectors_Partners Update//////////
                if (objFields.NoOfDirectors_Partners != null)
                {
                    BuisnessProfile.NoOfDirectors_Partners = objFields.NoOfDirectors_Partners;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "noofdirectors")
                    {
                        BuisnessProfile.NoOfDirectors_Partners = null;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness NoOfTrustees Update//////////
                if (objFields.NoOfTrustees != null)
                {
                    BuisnessProfile.NoOfTrustees = objFields.NoOfTrustees;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nooftrustees")
                    {
                        BuisnessProfile.NoOfTrustees = null;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegistrationDate Update//////////
                if (objFields.RegistrationDate != null)
                {
                    BuisnessProfile.RegistrationDate = objFields.RegistrationDate;
                    _context.SaveChanges();

                }
                /////////////////
                ///
                ///  ///////////Buisness TradeStartingDate Update//////////
                if (objFields.TradeStartingDate != null)
                {
                    BuisnessProfile.TradeStartingDate = objFields.TradeStartingDate;
                    _context.SaveChanges();

                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredAdresss Update//////////
                if (objFields.RegisteredAdresss != null)
                {
                    BuisnessProfile.RegisteredAdresss = objFields.RegisteredAdresss;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "raddress")
                    {
                        BuisnessProfile.RegisteredAdresss = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCity Update//////////
                if (objFields.RegisteredCity != null)
                {
                    BuisnessProfile.RegisteredCity = objFields.RegisteredCity;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcity")
                    {
                        BuisnessProfile.RegisteredCity = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCounty Update//////////
                if (objFields.RegisteredCounty != null)
                {
                    BuisnessProfile.RegisteredCounty = objFields.RegisteredCounty;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcounty")
                    {
                        BuisnessProfile.RegisteredCounty = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredPostCode Update//////////
                if (objFields.RegisteredPostCode != null)
                {
                    BuisnessProfile.RegisteredPostCode = objFields.RegisteredPostCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rpost")
                    {
                        BuisnessProfile.RegisteredPostCode = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                ///  ///////////Buisness RegisteredCountry Update//////////
                if (objFields.RegisteredCountry != null)
                {
                    BuisnessProfile.RegisteredCountry = objFields.RegisteredCountry;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "rcountry")
                    {
                        BuisnessProfile.RegisteredCountry = string.Empty;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                /////////////////
                ///
                /////////////IF Buisness Registered Adrress is Other than Address//////////////
                if (objFields.RegisteredAdress)
                {
                    BuisnessProfile.RegisteredAdress = objFields.RegisteredAdress;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    BuisnessProfile.RegisteredAdress = false;
                    _context.SaveChanges();
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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                //var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                ///
                var AuthorizedRepresentative = _context.AuthorizedRepresentative.Where(x => x.RepresentativeId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != "")
                {
                    AuthorizedRepresentative.FirstName = objFields.FirstName;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname")
                    {
                        AuthorizedRepresentative.FirstName = objFields.FirstName;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }

                if (objFields.LastName != null && objFields.LastName != "")
                {
                    AuthorizedRepresentative.LastName = objFields.LastName;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname")
                    {
                        AuthorizedRepresentative.LastName = objFields.LastName;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != "")
                {
                    AuthorizedRepresentative.Country = objFields.Country;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country")
                    {
                        AuthorizedRepresentative.Country = objFields.Country;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Address1 != null && objFields.Address1 != "")
                {
                    AuthorizedRepresentative.Address1 = objFields.Address1;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address1")
                    {
                        AuthorizedRepresentative.Address1 = objFields.Address1;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Address2 != null && objFields.Address2 != "")
                {
                    AuthorizedRepresentative.Address2 = objFields.Address2;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address2")
                    {
                        AuthorizedRepresentative.Address2 = objFields.Address2;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != "")
                {
                    AuthorizedRepresentative.City = objFields.City;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city")
                    {
                        AuthorizedRepresentative.City = objFields.City;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != "")
                {
                    AuthorizedRepresentative.PostCode = objFields.PostCode;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode")
                    {
                        AuthorizedRepresentative.PostCode = objFields.PostCode;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != "")
                {
                    AuthorizedRepresentative.County = objFields.County;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county")
                    {
                        AuthorizedRepresentative.County = objFields.County;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.DOB != null)
                {
                    AuthorizedRepresentative.DOB = objFields.DOB;
                    _context.SaveChanges();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        AuthorizedRepresentative.DOB = objFields.DOB;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null)
                {
                    AuthorizedRepresentative.PhoneNumber = objFields.PhoneNumber;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber")
                    {
                        AuthorizedRepresentative.PhoneNumber = objFields.PhoneNumber;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != "")
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
                            _context.SaveChanges();
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
                    if (objFields.FieldName == "email")
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
                                _context.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                    }
                }
                if (objFields.PositionInBuisness != null && objFields.PositionInBuisness != "")
                {
                    AuthorizedRepresentative.PositionInBuisness = objFields.PositionInBuisness;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "positioninbuisness")
                    {
                        AuthorizedRepresentative.PositionInBuisness = objFields.PositionInBuisness;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.RoleIncharity != null && objFields.RoleIncharity != "")
                {
                    AuthorizedRepresentative.RoleIncharity = objFields.RoleIncharity;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "roleincharity")
                    {
                        AuthorizedRepresentative.RoleIncharity = objFields.RoleIncharity;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.PositionInComany != null && objFields.PositionInComany != "")
                {
                    AuthorizedRepresentative.PositionInComany = objFields.PositionInComany;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "positionincompany")
                    {
                        AuthorizedRepresentative.PositionInComany = objFields.PositionInComany;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }

            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveBuisnessInformationFields(BuisnessInformation objFields)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessInformations = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId && x.BuisnessTypeId == BuisnessProfile.BuisnessTypeId).FirstOrDefault();
                if (objFields.Answer1 != null && objFields.Answer1 != "")
                {
                    BuisnessInformations.Answer1 = objFields.Answer1;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1")
                    {
                        BuisnessInformations.Answer1 = objFields.Answer1;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer2 != null && objFields.Answer2 != "")
                {
                    BuisnessInformations.Answer2 = objFields.Answer2;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2")
                    {
                        BuisnessInformations.Answer2 = objFields.Answer2;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer3 != null && objFields.Answer3 != "")
                {
                    BuisnessInformations.Answer3 = objFields.Answer3;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3")
                    {
                        BuisnessInformations.Answer3 = objFields.Answer3;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer4 != null && objFields.Answer4 != "")
                {
                    BuisnessInformations.Answer4 = objFields.Answer4;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4")
                    {
                        BuisnessInformations.Answer4 = objFields.Answer4;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer5 != null && objFields.Answer5 != "")
                {
                    BuisnessInformations.Answer5 = objFields.Answer5;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5")
                    {
                        BuisnessInformations.Answer5 = objFields.Answer5;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer6 != null && objFields.Answer6 != "")
                {
                    BuisnessInformations.Answer6 = objFields.Answer6;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6")
                    {
                        BuisnessInformations.Answer6 = objFields.Answer6;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer7 != null && objFields.Answer7 != "")
                {
                    BuisnessInformations.Answer7 = objFields.Answer7;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7")
                    {
                        BuisnessInformations.Answer7 = objFields.Answer7;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer8 != null && objFields.Answer8 != "")
                {
                    BuisnessInformations.Answer8 = objFields.Answer8;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8")
                    {
                        BuisnessInformations.Answer8 = objFields.Answer8;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Answer9 != null && objFields.Answer9 != "")
                {
                    BuisnessInformations.Answer9 = objFields.Answer9;
                    BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9")
                    {
                        BuisnessInformations.Answer9 = objFields.Answer9;
                        BuisnessInformations.BuisnessTypeId = BuisnessProfile.BuisnessTypeId;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveFinancialInformationFields(FinancialInformation objFields)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var FinancialInformation = _context.FinancialInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.PerMonth != null)
                {
                    FinancialInformation.PerMonth = objFields.PerMonth;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1")
                    {
                        FinancialInformation.PerMonth = objFields.PerMonth;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PerAnum != null)
                {
                    FinancialInformation.PerAnum = objFields.PerAnum;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2")
                    {
                        FinancialInformation.PerAnum = objFields.PerAnum;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PaymentIncoming != null)
                {
                    FinancialInformation.PaymentIncoming = objFields.PaymentIncoming;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3")
                    {
                        FinancialInformation.PaymentIncoming = objFields.PaymentIncoming;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PaymentOutgoing != null)
                {
                    FinancialInformation.PaymentOutgoing = objFields.PaymentOutgoing;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4")
                    {
                        FinancialInformation.PaymentOutgoing = objFields.PaymentOutgoing;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.TransactionIncoming != null)
                {
                    FinancialInformation.TransactionIncoming = objFields.TransactionIncoming;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5")
                    {
                        FinancialInformation.TransactionIncoming = objFields.TransactionIncoming;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.TransactionOutgoing != null)
                {
                    FinancialInformation.TransactionOutgoing = objFields.TransactionOutgoing;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6")
                    {
                        FinancialInformation.TransactionOutgoing = objFields.TransactionOutgoing;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.NoOfPaymentsPerMonth != null)
                {
                    FinancialInformation.NoOfPaymentsPerMonth = objFields.NoOfPaymentsPerMonth;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7")
                    {
                        FinancialInformation.NoOfPaymentsPerMonth = objFields.NoOfPaymentsPerMonth;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.VolumePermonth != null)
                {
                    FinancialInformation.VolumePermonth = objFields.VolumePermonth;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8")
                    {
                        FinancialInformation.VolumePermonth = objFields.VolumePermonth;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.BankName != null)
                {
                    FinancialInformation.BankName = objFields.BankName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9")
                    {
                        FinancialInformation.BankName = objFields.BankName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.BankAddress != null)
                {
                    FinancialInformation.BankAddress = objFields.BankAddress;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "10")
                    {
                        FinancialInformation.BankAddress = objFields.BankAddress;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.SortCode != null)
                {
                    FinancialInformation.SortCode = objFields.SortCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "11")
                    {
                        FinancialInformation.SortCode = objFields.SortCode;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.AccountName != null)
                {
                    FinancialInformation.AccountName = objFields.AccountName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "12")
                    {
                        FinancialInformation.AccountName = objFields.AccountName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.AccountNumber != null)
                {
                    FinancialInformation.AccountNumber = objFields.AccountNumber;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "13")
                    {
                        FinancialInformation.AccountNumber = objFields.AccountNumber;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.IBAN != null)
                {
                    FinancialInformation.IBAN = objFields.IBAN;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "14")
                    {
                        FinancialInformation.IBAN = objFields.IBAN;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.SwiftCode != null)
                {
                    FinancialInformation.SwiftCode = objFields.SwiftCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "15")
                    {
                        FinancialInformation.SwiftCode = objFields.SwiftCode;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.AccountCurrency != null)
                {
                    FinancialInformation.AccountCurrency = objFields.AccountCurrency;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "16")
                    {
                        FinancialInformation.AccountCurrency = objFields.AccountCurrency;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }

                if (objFields.AccountDetails)
                {
                    FinancialInformation.AccountDetails = objFields.AccountDetails;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    FinancialInformation.AccountDetails = false;
                    _context.SaveChanges();
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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var Director = _context.DirectorAndShareHolders.Where(x => x.DirectorId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != "")
                {
                    Director.FirstName = objFields.FirstName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname")
                    {
                        Director.FirstName = objFields.FirstName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null && objFields.LastName != "")
                {
                    Director.LastName = objFields.LastName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname")
                    {
                        Director.LastName = objFields.LastName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Address != null && objFields.Address != "")
                {
                    Director.Address = objFields.Address;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address")
                    {
                        Director.Address = objFields.Address;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != "")
                {
                    Director.City = objFields.City;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city")
                    {
                        Director.City = objFields.City;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != "")
                {
                    Director.PostCode = objFields.PostCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode")
                    {
                        Director.PostCode = objFields.PostCode;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != "")
                {
                    Director.Country = objFields.Country;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country")
                    {
                        Director.Country = objFields.Country;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null && objFields.Nationality != "")
                {
                    Director.Nationality = objFields.Nationality;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nationality")
                    {
                        Director.Nationality = objFields.Nationality;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != "")
                {
                    Director.County = objFields.County;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county")
                    {
                        Director.County = objFields.County;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.DOB != null)
                {
                    Director.DOB = objFields.DOB;
                    _context.SaveChanges();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        Director.DOB = objFields.DOB;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != "")
                {
                    Director.PhoneNumber = objFields.PhoneNumber;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber")
                    {
                        Director.PhoneNumber = objFields.PhoneNumber;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != "")
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
                            _context.SaveChanges();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Director.Email = objFields.Email;
                            _context.SaveChanges();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }


                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "email")
                    {
                        Director.Email = objFields.Email;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.ShareHolders_percentage != null)
                {
                    Director.ShareHolders_percentage = objFields.ShareHolders_percentage;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "shareholders")
                    {
                        Director.ShareHolders_percentage = objFields.ShareHolders_percentage;
                        _context.SaveChanges();

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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var Trustees = _context.Trustees.Where(x => x.TrusteeId == objFields.Formid).FirstOrDefault();
                if (objFields.FirstName != null && objFields.FirstName != "")
                {
                    Trustees.FirstName = objFields.FirstName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "firstname")
                    {
                        Trustees.FirstName = objFields.FirstName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null && objFields.LastName != "")
                {
                    Trustees.LastName = objFields.LastName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "lastname")
                    {
                        Trustees.LastName = objFields.LastName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Address != null && objFields.Address != "")
                {
                    Trustees.Address = objFields.Address;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "address")
                    {
                        Trustees.Address = objFields.Address;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.City != null && objFields.City != "")
                {
                    Trustees.City = objFields.City;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "city")
                    {
                        Trustees.City = objFields.City;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null && objFields.PostCode != "")
                {
                    Trustees.PostCode = objFields.PostCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "postcode")
                    {
                        Trustees.PostCode = objFields.PostCode;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Country != null && objFields.Country != "")
                {
                    Trustees.Country = objFields.Country;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "country")
                    {
                        Trustees.Country = objFields.Country;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null && objFields.Nationality != "")
                {
                    Trustees.Nationality = objFields.Nationality;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "nationality")
                    {
                        Trustees.Nationality = objFields.Nationality;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.County != null && objFields.County != "")
                {
                    Trustees.County = objFields.County;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "county")
                    {
                        Trustees.County = objFields.County;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.DOB != null)
                {
                    Trustees.DOB = objFields.DOB;
                    _context.SaveChanges();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "dob")
                    {
                        Trustees.DOB = objFields.DOB;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.AppointmentDate != null)
                {
                    Trustees.AppointmentDate = objFields.AppointmentDate;
                    _context.SaveChanges();

                    //return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "appdate")
                    {
                        Trustees.AppointmentDate = objFields.AppointmentDate;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null && objFields.PhoneNumber != "")
                {
                    Trustees.PhoneNumber = objFields.PhoneNumber;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "phonenumber")
                    {
                        Trustees.PhoneNumber = objFields.PhoneNumber;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Email != null && objFields.Email != "")
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
                            _context.SaveChanges();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Trustees.Email = objFields.Email;
                            _context.SaveChanges();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }


                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "email")
                    {
                        Trustees.Email = objFields.Email;
                        _context.SaveChanges();

                        return Json(true);
                    }
                }
                if (objFields.Role != null || objFields.Role != "")
                {
                    Trustees.Role = objFields.Role;
                    _context.SaveChanges();

                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "role")
                    {
                        Trustees.Role = objFields.Role;
                        _context.SaveChanges();

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
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var Owner = _context.OwnerShip.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.FirstName != null)
                {
                    Owner.FirstName = objFields.FirstName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "1")
                    {
                        Owner.FirstName = objFields.FirstName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.LastName != null)
                {
                    Owner.LastName = objFields.LastName;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "2")
                    {
                        Owner.LastName = objFields.LastName;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Address != null)
                {
                    Owner.Address = objFields.Address;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "3")
                    {
                        Owner.Address = objFields.Address;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.City != null)
                {
                    Owner.City = objFields.City;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "4")
                    {
                        Owner.City = objFields.City;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PostCode != null)
                {
                    Owner.PostCode = objFields.PostCode;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "5")
                    {
                        Owner.PostCode = objFields.PostCode;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.County != null)
                {
                    Owner.County = objFields.County;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "6")
                    {
                        Owner.County = objFields.County;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Country != null)
                {
                    Owner.Country = objFields.Country;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "7")
                    {
                        Owner.Country = objFields.Country;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Nationality != null)
                {
                    Owner.Nationality = objFields.Nationality;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "8")
                    {
                        Owner.Nationality = objFields.Nationality;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.DOB != null)
                {
                    Owner.DOB = objFields.DOB;
                    _context.SaveChanges();
                    // return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "9")
                    {
                        Owner.DOB = objFields.DOB;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.PhoneNumber != null)
                {
                    Owner.PhoneNumber = objFields.PhoneNumber;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "10")
                    {
                        Owner.PhoneNumber = objFields.PhoneNumber;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
                if (objFields.Email != null)
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
                            _context.SaveChanges();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            Owner.Email = objFields.Email;
                            _context.SaveChanges();
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }

                    Owner.Email = objFields.Email;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    if (objFields.FieldName == "11")
                    {
                        Owner.Email = string.Empty;
                        _context.SaveChanges();
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSolePersonalDocsData(SoleDocuments objFields)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDoc = _context.SoleDocuments.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.isPop)
                {
                    SoleDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                    //return Json(true);
                }
                else
                {
                    SoleDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                 //   return Json(true);
                }
               if(objFields.RelationShip != null && objFields.RelationShip != "")
                {

                    SoleDoc.RelationShip = objFields.RelationShip;
                    _context.SaveChanges();
                    return Json(true);
                   
                }
            }

                return Json(false);

        }
        [HttpPost]
        public async Task<IActionResult> SaveCharityPersonalDocsData(CharityDocument objFields)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDoc = _context.CharityDocument.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.isPop)
                {
                    CharityDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                    //return Json(true);
                }
                else
                {
                    CharityDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                    //   return Json(true);
                }
                if (objFields.RelationShip != null && objFields.RelationShip != "")
                {

                    CharityDoc.RelationShip = objFields.RelationShip;
                    _context.SaveChanges();
                    return Json(true);

                }
            }

            return Json(false);

        }
        [HttpPost]
        public async Task<IActionResult> SavebuisnessPersonalDocsData(BuisnessDocuments objFields)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var buisnessDoc = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();

                if (objFields.isPop)
                {
                    buisnessDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                    //return Json(true);
                }
                else
                {
                    buisnessDoc.isPop = objFields.isPop;
                    _context.SaveChanges();
                    //   return Json(true);
                }
                if (objFields.RelationShip != null && objFields.RelationShip != "")
                {

                    buisnessDoc.RelationShip = objFields.RelationShip;
                    _context.SaveChanges();
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
            }
            else if (BuisnessProfile.BuisnessTypeId == 3 || BuisnessProfile.BuisnessTypeId == 4)
            {
                if (BuisnessProfile.IncorporationNumber == null || BuisnessProfile.IncorporationNumber == "")
                {
                    objValidations.Add(new ModelValidations { Validationid = "Validation9", Message = "Enter Incorporation Number" });

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
            var DirectorsInformations = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();
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
            var OwnersInformation = _context.OwnerShip.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).FirstOrDefault();

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
            return Json(objValidations);
        }
        public async Task<IActionResult> ValidateTrusteeFields(int BuisnessProfileId)
        {
            List<ModelValidations> objValidations = new List<ModelValidations>();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();
            var TrusteesInformations = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfile.BuisnessProfileId).ToList();
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
        public async Task<IActionResult> SetCurrentForm(int Form)
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string UserId = applicationUser?.Id; // will give the user's Email

            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
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

            _context.AuthorizedRepresentative.Remove(Authorized);
            _context.SaveChanges();
            return Json(true);
        }
        public async Task<IActionResult> GetAllRepresentative(int BuisnessProfileId)
        {
           
            var Authorized = _context.AuthorizedRepresentative.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false).ToList();
           foreach(var auth in Authorized)
            {
                if (auth.DOB != null)
                {
                    auth.DateOfBirth = auth.DOB.ToString("yyyy-MM-dd");
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

            _context.DirectorAndShareHolders.Remove(Director);
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
            var Directors = _context.DirectorAndShareHolders.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false).ToList();
          
            foreach(var dir in Directors)
            {
                if(dir.DOB != null)
                {
                    dir.Dateofbirth = dir.DOB.ToString("yyyy-MM-dd");
                }else
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

            _context.Trustees.Remove(Trustee);
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
            var Trustees = _context.Trustees.Where(x => x.BuisnessProfileId == BuisnessProfileId && x.Isdefault == false).ToList();
            foreach (var dir in Trustees)
            {
                if (dir.DOB != null)
                {
                    dir.Dateofbirth = dir.DOB.ToString("yyyy-MM-dd");
                }
                else
                {
                    dir.Dateofbirth = "yyyy-MM-dd";
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
    }
}
