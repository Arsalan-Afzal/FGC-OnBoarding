using FGC_OnBoarding.Areas.Identity.Data;
using FGC_OnBoarding.Data;
using FGC_OnBoarding.Helpers;
using FGC_OnBoarding.Models;
using FGC_OnBoarding.Models.Buisness;
using FGC_OnBoarding.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Controllers
{
    [Authorize]
    public class ClientApplication : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        //  private readonly ILogger<UserManager> _userManager;
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly FGC_OnBoardingContext _context;
        private readonly IEmailSender _emailSender;
        //private readonly OnboardingExtentions _context;
        public ClientApplication(ILogger<HomeController> logger, UserManager<FGC_OnBoardingUser> userManager, FGC_OnBoardingContext context, IWebHostEnvironment env, IEmailSender emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _env = env;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> EditPep(ParentProfile objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                string ip = GetClientIPAddress(HttpContext);
                string Country = GetUserCountryByIp(ip);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.objBuisness.BuisnessProfileId).FirstOrDefault();

                if (BuisnessProfile.Ispep != objFields.objBuisness.Ispep)
                {

                    var EmailsString = "";
                    var OldPepYesNo = "";
                    var NewPepYesNo = "";
                    if (BuisnessProfile.Ispep)
                    {
                        OldPepYesNo = "Yes-" + BuisnessProfile.Peprelationship;
                    }
                    else
                    {
                        OldPepYesNo = "No";
                    }
                    if (objFields.objBuisness.Ispep)
                    {
                        NewPepYesNo = "Yes-" + objFields.objBuisness.Peprelationship;
                    }
                    else
                    {
                        NewPepYesNo = "No";
                    }

                    var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/><br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>PEP DECLERATION<br/>" +
                            "<b>Field Name: </b>Is Pep<br/>" +
                            "<b>Old Value: </b>" + OldPepYesNo + "<br/>" +
                            "<b>New Value: </b>" + NewPepYesNo + "<br/><br/><br/><br/>" +
                            "Thanks<br/><br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;

                        objnewLogs.OldValue = OldPepYesNo;
                        objnewLogs.NewValue = NewPepYesNo;
                        objnewLogs.FormName = "Pep Decleration";
                        objnewLogs.FieldName = "Is Pep";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Action = "Pep Decleration Updated";
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;

                        objnewLogs.OldValue = OldPepYesNo;
                        objnewLogs.NewValue = NewPepYesNo;
                        objnewLogs.FormName = "Pep Decleration";
                        objnewLogs.FieldName = "Is Pep";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Action = "Pep Decleration Updated";
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    BuisnessProfile.Ispep = objFields.objBuisness.Ispep;
                    BuisnessProfile.Peprelationship = objFields.objBuisness.Peprelationship;

                    _context.SaveChanges();
                }
                return RedirectToAction("EditApplication", "Home", new { BuisnessProfileId = objFields.objBuisness.BuisnessProfileId, Link = "link5" });


            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditCurrency(ParentProfile objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                string ip = GetClientIPAddress(HttpContext);
                string Country = GetUserCountryByIp(ip);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.objBuisness.BuisnessProfileId).FirstOrDefault();

                var OldCurrencyNAmes = "";
                if (BuisnessProfile.CurrencyId.Contains(','))
                {
                    var Currencyarray = BuisnessProfile.CurrencyId.Split(',');
                    foreach (var cur in Currencyarray)
                    {
                        if (cur != "")
                        {
                            var CurrencyID = Convert.ToInt32(cur);
                            var Currencyname = _context.Currency.Where(x => x.CurrencyId == CurrencyID).Select(x => x.Name).FirstOrDefault();
                            OldCurrencyNAmes += Currencyname + ",";
                        }

                    }
                    OldCurrencyNAmes = OldCurrencyNAmes.Remove(OldCurrencyNAmes.Length - 1);
                }
                else
                {
                    var CurrencyID = Convert.ToInt32(BuisnessProfile.CurrencyId);
                    var Currencyname = _context.Currency.Where(x => x.CurrencyId == CurrencyID).Select(x => x.Name).FirstOrDefault();
                    OldCurrencyNAmes += Currencyname;
                }
                var NewCurrencyNAmes = "";
                if (objFields.objBuisness.CurrencyId.Contains(','))
                {
                    var Currencyarray = objFields.objBuisness.CurrencyId.Split(',');
                    foreach (var cur in Currencyarray)
                    {
                        if (cur != "")
                        {
                            var CurrencyID = Convert.ToInt32(cur);
                            var Currencyname = _context.Currency.Where(x => x.CurrencyId == CurrencyID).Select(x => x.Name).FirstOrDefault();
                            NewCurrencyNAmes += Currencyname + ",";
                        }
                    }
                    NewCurrencyNAmes = OldCurrencyNAmes.Remove(OldCurrencyNAmes.Length - 1);
                }
                else
                {
                    var CurrencyID = Convert.ToInt32(objFields.objBuisness.CurrencyId);
                    var Currencyname = _context.Currency.Where(x => x.CurrencyId == CurrencyID).Select(x => x.Name).FirstOrDefault();
                    NewCurrencyNAmes += Currencyname;
                }


                var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                foreach (var itm in Emails)
                {
                    //EmailsString += itm + ",";
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Service Requirment<br/>" +
                        "<b>Field Name: </b> Currency<br/>" +
                        "<b>Old Value: </b>" + OldCurrencyNAmes + "<br/>" +
                        "<b>New Value: </b>" + NewCurrencyNAmes + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          " IT Team" +
                        "");
                }
                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    objnewLogs.OldValue = OldCurrencyNAmes;
                    objnewLogs.NewValue = NewCurrencyNAmes;
                    objnewLogs.FormName = "Service Requirment";
                    objnewLogs.FieldName = "Currency";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Service Requirment Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = OldCurrencyNAmes;
                    objnewLogs.NewValue = NewCurrencyNAmes;
                    objnewLogs.FormName = "Service Requirment";
                    objnewLogs.FieldName = "Currency";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Service Requirment Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }

                BuisnessProfile.CurrencyId = objFields.objBuisness.CurrencyId;
                _context.SaveChanges();
                return RedirectToAction("EditApplication", "Home", new { BuisnessProfileId = objFields.objBuisness.BuisnessProfileId });
            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveBuisnessFields(BuisnessProfile objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email

            if (UserId != null)
            {

                string ip = GetClientIPAddress(HttpContext);
                string Country = GetUserCountryByIp(ip);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                ///
                if (objFields.FieldName == "Buisnessname" && objFields.FieldValue != BuisnessProfile.BuisnessName)
                {
                    try
                    {
                        // var ClientIPAddr = HttpContext.Connection.RemoteIpAddress?.ToString();

                        foreach (var itm in Emails)
                        {
                            //EmailsString += itm + ",";
                            await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                                $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                                "<b>Customer Name: </b>" + UserName + "<br/>" +
                                "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                                "<b>Section Name: </b>Buisness Profile<br/>" +
                                "<b>Field Name: </b> Buisness Name<br/>" +
                                "<b>Old Value: </b>" + BuisnessProfile.BuisnessName + "<br/>" +
                                "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                                "Thanks<br/>" +
                                  "FGC,<br/>" +
                                  " IT Team" +
                                "");
                        }
                        if (Role == "Introducer")
                        {
                            IntroducerLogs objnewLogs = new IntroducerLogs();
                            objnewLogs.IntroducerName = UserName;
                            objnewLogs.OldValue = BuisnessProfile.BuisnessName;
                            objnewLogs.NewValue = objFields.FieldValue;
                            objnewLogs.FormName = "Buisness Profile";
                            objnewLogs.FieldName = "Buisness Name";
                            objnewLogs.IPAdress = ip;
                            objnewLogs.Action = "Buisess Profile Updated";
                            objnewLogs.Email = UserEmail;
                            objnewLogs.IntroducerId = UserId;
                            objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                            objnewLogs.CountryName = Country;

                            _context.IntroducerLogs.Add(objnewLogs);
                        }
                        else
                        {
                            CustomerLogs objnewLogs = new CustomerLogs();
                            objnewLogs.CustomerName = UserName;
                            objnewLogs.OldValue = BuisnessProfile.BuisnessName;
                            objnewLogs.NewValue = objFields.FieldValue;
                            objnewLogs.FormName = "Buisness Profile";
                            objnewLogs.FieldName = "Buisness Name";
                            objnewLogs.IPAdress = ip;
                            objnewLogs.Action = "Buisess Profile Updated";
                            objnewLogs.Email = UserEmail;
                            objnewLogs.CustomerId = UserId;
                            objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                            objnewLogs.CountryName = Country;

                            _context.CustomerLogs.Add(objnewLogs);
                        }

                        await _context.SaveChangesAsync();

                        BuisnessProfile.BuisnessName = objFields.FieldValue;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                    }
                    return Json(true);
                }

                /////////////////
                ///  ///////////Buisness Address Update//////////

                else if (objFields.FieldName == "address" && objFields.FieldValue != BuisnessProfile.Address)
                {


                    // var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team,<br/> <br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business Adress<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.Address + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.Address;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Business Adress";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();

                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.Address = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness City Update//////////

                else if (objFields.FieldName == "city" && objFields.FieldValue != BuisnessProfile.City)
                {

                    //var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business City<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.City + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              " IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.City;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Business City";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();

                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.City = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness County Update//////////

                else if (objFields.FieldName == "county" && objFields.FieldValue != BuisnessProfile.County)
                {
                    //var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business County<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.County + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              " IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.County;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Business County";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();


                    BuisnessProfile.County = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness PostCode Update//////////

                else if (objFields.FieldName == "postcode" && objFields.FieldValue != BuisnessProfile.PostCode)
                {
                    //  var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business Postcode<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.PostCode + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.PostCode;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Business Postcode";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.PostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness Country Update//////////

                else if (objFields.FieldName == "country" && objFields.FieldValue != BuisnessProfile.Country)
                {

                    // var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business Country<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.Country + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.Country;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.FieldName = "Business Country";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.Country = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness BuisnessWebsite Update//////////

                else if (objFields.FieldName == "buisnesswebsite" && objFields.FieldValue != BuisnessProfile.BuisnessWebsite)
                {

                    // var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();


                    //   await _context.SaveChangesAsync();

                    objFields.BuisnessWebsite = objFields.FieldValue.Trim();

                    Regex pattern = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
                    Match match = pattern.Match(objFields.BuisnessWebsite);
                    if (match.Success == true)
                    {

                        foreach (var itm in Emails)
                        {
                            //EmailsString += itm + ",";
                            await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                                $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                                "<b>Customer Name: </b>" + UserName + "<br/>" +
                                "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                                "<b>Section Name: </b>Buisness Profile<br/>" +
                                "<b>Field Name: </b> Business Website<br/>" +
                                "<b>Old Value: </b>" + BuisnessProfile.BuisnessWebsite + "<br/>" +
                                "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                                "Thanks<br/>" +
                                  "FGC,<br/>" +
                                  "IT Team" +
                                "");
                        }


                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = BuisnessProfile.BuisnessWebsite;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Profile";
                        objnewLogs.FieldName = "Business Website";
                        objnewLogs.Action = "Buisess Profile Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.CustomerLogs.Add(objnewLogs);

                        BuisnessProfile.BuisnessWebsite = objFields.FieldValue;
                        await _context.SaveChangesAsync();
                        return Json(true);
                    }
                    else
                    {
                        return Json(new { flag = true, validation = "Invalid Website Url" });
                    }

                    //BuisnessProfile.BuisnessWebsite = string.Empty;
                    //await _context.SaveChangesAsync();

                    //return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness BuisnessEmail Update//////////

                else if (objFields.FieldName == "buisnessemail" && objFields.FieldValue != BuisnessProfile.BuisnessEmail)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.FieldValue);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            //BuisnessProfile.BuisnessEmail = objFields.FieldValue;
                            //await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");

                            // var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
                            foreach (var itm in Emails)
                            {
                                //EmailsString += itm + ",";
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")<b/> has been updated:<br/>" +
                                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                                    "<b>Section Name: </b>Buisness Profile<br/>" +
                                    "<b>Field Name: </b> Business Email<br/>" +
                                    "<b>Old Value: </b>" + BuisnessProfile.BuisnessEmail + "<br/>" +
                                    "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                                    "Thanks<br/>" +
                                      "FGC,<br/>" +
                                      "IT Team" +
                                    "");
                            }


                            CustomerLogs objnewLogs = new CustomerLogs();
                            objnewLogs.CustomerName = UserName;
                            objnewLogs.OldValue = BuisnessProfile.BuisnessEmail;
                            objnewLogs.NewValue = objFields.FieldValue;
                            objnewLogs.FormName = "Buisness Profile";
                            objnewLogs.FieldName = "Business Email";
                            objnewLogs.Action = "Buisess Profile Updated";
                            objnewLogs.IPAdress = ip;
                            objnewLogs.Email = UserEmail;
                            objnewLogs.CustomerId = UserId;
                            objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                            objnewLogs.CountryName = Country;

                            _context.CustomerLogs.Add(objnewLogs);
                            await _context.SaveChangesAsync();


                            BuisnessProfile.BuisnessEmail = objFields.FieldValue;
                            await _context.SaveChangesAsync();
                            return Json(true);
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }
                }

                /////////////////
                ///
                ///  ///////////Buisness UTR Update//////////

                else if (objFields.FieldName == "utr" && objFields.FieldValue != BuisnessProfile.UTR)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Business UTR<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.UTR + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.UTR;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "UTR";
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.UTR = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness CharityNumber Update//////////

                else if (objFields.FieldName == "charitynumber" && objFields.FieldValue != BuisnessProfile.CharityNumber)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Charity Number<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.CharityNumber + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              " IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.CharityNumber;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Charity Number";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.CharityNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness IncorporationNumber Update//////////

                else if (objFields.FieldName == "incopnumber" && objFields.FieldValue != BuisnessProfile.IncorporationNumber)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Incorporation Number<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.IncorporationNumber + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.IncorporationNumber;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Incorporation Number";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.IncorporationNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness NoOfDirectors_Partners Update//////////

                else if (objFields.FieldName == "noofdirectors" && Convert.ToInt32(objFields.FieldValue) != BuisnessProfile.NoOfDirectors_Partners)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> No Of Directors<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.IncorporationNumber + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.NoOfDirectors_Partners.ToString();
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "No Of Directors";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.NoOfDirectors_Partners = Convert.ToInt32(objFields.FieldValue);
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness NoOfTrustees Update//////////

                else if (objFields.FieldName == "nooftrustees" && Convert.ToInt32(objFields.FieldValue) != BuisnessProfile.NoOfTrustees)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> No Of Trustees<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.IncorporationNumber + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.NoOfTrustees.ToString();
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "No Of Trustees";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.NoOfTrustees = Convert.ToInt32(objFields.FieldValue);
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegistrationDate Update//////////
                else if (objFields.FieldName == "RegDate" && Convert.ToDateTime(objFields.FieldValue) != BuisnessProfile.RegistrationDate)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registration Date<br/>" +
                            "<b>Old Value: </b>" + Convert.ToDateTime(BuisnessProfile.RegistrationDate).ToString("dd-MMM-yyyy") + "<br/>" +
                            "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Convert.ToDateTime(BuisnessProfile.RegistrationDate).ToString("dd-MMM-yyyy");
                    objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Registration Date";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();
                    BuisnessProfile.RegistrationDate = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                /////////////////
                ///
                ///  ///////////Buisness TradeStartingDate Update//////////
                else if (objFields.FieldName == "TradeDate" && Convert.ToDateTime(objFields.FieldValue) != BuisnessProfile.TradeStartingDate)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Trade Start Date<br/>" +
                            "<b>Old Value: </b>" + Convert.ToDateTime(BuisnessProfile.TradeStartingDate).ToString("dd-MMM-yyyy") + "<br/>" +
                            "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Convert.ToDateTime(BuisnessProfile.TradeStartingDate).ToString("dd-MMM-yyyy");
                    objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Trade Start Date";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.TradeStartingDate = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredAdresss Update//////////

                else if (objFields.FieldName == "raddress" && objFields.FieldValue != BuisnessProfile.RegisteredAdresss)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registered Address<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.RegisteredAdresss + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.RegisteredAdresss;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Register Address";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredAdresss = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredCity Update//////////

                else if (objFields.FieldName == "rcity" && objFields.FieldValue != BuisnessProfile.RegisteredCity)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registered City<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.RegisteredCity + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.RegisteredCity;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Register City";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredCity = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredCounty Update//////////

                else if (objFields.FieldName == "rcounty" && objFields.FieldValue != BuisnessProfile.RegisteredCounty)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registered County<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.RegisteredCounty + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.RegisteredCounty;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Register County";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredCity = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredCounty = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredPostCode Update//////////

                else if (objFields.FieldName == "rpost" && objFields.FieldValue != BuisnessProfile.RegisteredPostCode)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registered Postcode<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.RegisteredPostCode + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.RegisteredPostCode;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Register Postcode";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredPostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                /////////////////
                ///
                ///  ///////////Buisness RegisteredCountry Update//////////

                else if (objFields.FieldName == "rcountry" && objFields.FieldValue != BuisnessProfile.RegisteredCountry)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/> <br/>Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Profile<br/>" +
                            "<b>Field Name: </b> Registered Country<br/>" +
                            "<b>Old Value: </b>" + BuisnessProfile.RegisteredCountry + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = BuisnessProfile.RegisteredCountry;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Buisness Profile";
                    objnewLogs.FieldName = "Register Country";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Action = "Buisess Profile Updated";
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.CountryName = Country;

                    _context.CustomerLogs.Add(objnewLogs);
                    await _context.SaveChangesAsync();

                    BuisnessProfile.RegisteredCountry = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveBuisnessInformationFields(BuisnessInformation objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                var BuisnessInformations = _context.BuisnessInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId && x.BuisnessTypeId == objFields.BuisnessTypeId).FirstOrDefault();
                var QuestionArray = objFields.Question.Split('<');
                var Question = QuestionArray[0];
                if (objFields.FieldName == "Answer1" && objFields.FieldValue != BuisnessInformations.Answer1)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer1 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    BuisnessInformations.Answer1 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "Answer2" && objFields.FieldValue != BuisnessInformations.Answer2)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer2 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer2 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;


                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Answer3" && objFields.FieldValue != BuisnessInformations.Answer3)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer3 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer3 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Answer4" && objFields.FieldValue != BuisnessInformations.Answer4)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer4 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer4 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }

                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Answer5" && objFields.FieldValue != BuisnessInformations.Answer5)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer5 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer5 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Answer6" && objFields.FieldValue != BuisnessInformations.Answer6)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer6 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer6 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "Answer7" && objFields.FieldValue != BuisnessInformations.Answer7)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer7 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer7 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "Answer8" && objFields.FieldValue != BuisnessInformations.Answer8)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer8 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer8 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "Answer9" && objFields.FieldValue != BuisnessInformations.Answer9)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Buisness Information<br/>" +
                            "<b>Field Name: </b>" + Question + "<br/>" +
                            "<b>Old Value: </b>" + BuisnessInformations.Answer9 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    BuisnessInformations.Answer9 = objFields.FieldValue;
                    BuisnessInformations.BuisnessTypeId = objFields.BuisnessTypeId;
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        //objnewLogs.OldValue = BuisnessInformations.Email;
                        //objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Buisness Information";
                        objnewLogs.Action = "Buisness Information Updated";
                        //objnewLogs.FieldName = "Email";
                        objnewLogs.Remarks = objFields.FieldValue;
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOwnersFields(OwnerShip objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                var Owner = _context.OwnerShip.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();


                if (objFields.FieldName == "OwnerFirstName" && objFields.FieldValue != Owner.FirstName)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/>  Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>First Name<br/>" +
                            "<b>Old Value: </b>" + Owner.FirstName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/> " +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.CustomerLogs.Add(objnewLogs);

                    }

                    await _context.SaveChangesAsync();
                    Owner.FirstName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                if (objFields.FieldName == "OwnerLastName" && objFields.FieldValue != Owner.LastName)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Last Name<br/>" +
                            "<b>Old Value: </b>" + Owner.LastName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.CustomerLogs.Add(objnewLogs);

                    }


                    await _context.SaveChangesAsync();

                    Owner.LastName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerAddress" && objFields.FieldValue != Owner.Address)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Address<br/>" +
                            "<b>Old Value: </b>" + Owner.Address + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.Address;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Address";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.Address;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Address";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.CustomerLogs.Add(objnewLogs);

                    }

                    await _context.SaveChangesAsync();

                    Owner.Address = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                if (objFields.FieldName == "OwnerCity" && objFields.FieldValue != Owner.City)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>City<br/>" +
                            "<b>Old Value: </b>" + Owner.City + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "City";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "City";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;

                        _context.CustomerLogs.Add(objnewLogs);

                    }

                    await _context.SaveChangesAsync();
                    Owner.City = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                if (objFields.FieldName == "OwnerPostCode" && objFields.FieldValue != Owner.PostCode)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Post Code<br/>" +
                            "<b>Old Value: </b>" + Owner.PostCode + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }

                    Owner.PostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerCounty" && objFields.FieldValue != Owner.County)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>County<br/>" +
                            "<b>Old Value: </b>" + Owner.County + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "County";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "County";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Owner.County = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerCountry" && objFields.FieldValue != Owner.Country)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Country<br/>" +
                            "<b>Old Value: </b>" + Owner.Country + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }

                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerNationality" && objFields.FieldValue != Owner.Nationality)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Nationality<br/>" +
                            "<b>Old Value: </b>" + Owner.Nationality + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.Nationality;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Nationality";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.Nationality;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Nationality";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Owner.Nationality = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerDOB" && Convert.ToDateTime(objFields.FieldValue) != Owner.DOB)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Date Of Birth<br/>" +
                            "<b>Old Value: </b>" + Convert.ToDateTime(Owner.DOB).ToString("dd-MMM-yyyy") + "<br/>" +
                            "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Owner.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Owner.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Owners Information";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Owner.DOB = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerPhoneNumber" && objFields.FieldValue != Owner.PhoneNumber)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Owner Information<br/>" +
                            "<b>Field Name: </b>Phone Number<br/>" +
                            "<b>Old Value: </b>" + Owner.PhoneNumber + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Owner.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";

                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Owner.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Owners Information";

                        objnewLogs.Action = "Owners Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                    }


                    Owner.PhoneNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                if (objFields.FieldName == "OwnerEmail" && objFields.FieldValue != Owner.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.FieldValue);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");

                            foreach (var itm in Emails)
                            {
                                //EmailsString += itm + ",";
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                                    "<b>Section Name: </b>Owner Information<br/>" +
                                    "<b>Field Name: </b>Email<br/>" +
                                    "<b>Old Value: </b>" + Owner.Email + "<br/>" +
                                    "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                                    "Thanks<br/>" +
                                      "FGC,<br/>" +
                                      "IT Team" +
                                    "");
                            }
                            if (Role == "Introducer")
                            {
                                IntroducerLogs objnewLogs = new IntroducerLogs();
                                objnewLogs.IntroducerName = UserName;
                                objnewLogs.OldValue = Owner.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Owners Information";
                                objnewLogs.Action = "Owners Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.Email = UserEmail;
                                objnewLogs.IntroducerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.CountryName = Country;
                                _context.IntroducerLogs.Add(objnewLogs);

                            }
                            else
                            {
                                CustomerLogs objnewLogs = new CustomerLogs();
                                objnewLogs.CustomerName = UserName;
                                objnewLogs.OldValue = Owner.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Owners Information";
                                objnewLogs.Action = "Owners Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CustomerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.CountryName = Country;
                                _context.CustomerLogs.Add(objnewLogs);

                            }



                            Owner.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {

                            foreach (var itm in Emails)
                            {
                                //EmailsString += itm + ",";
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                                    "<b>Section Name: </b>Owner Information<br/>" +
                                    "<b>Field Name: </b>Email<br/>" +
                                    "<b>Old Value: </b>" + Owner.Email + "<br/>" +
                                    "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                                    "Thanks<br/>" +
                                      "FGC,<br/>" +
                                      "IT Team" +
                                    "");
                            }
                            if (Role == "Introducer")
                            {
                                IntroducerLogs objnewLogs = new IntroducerLogs();
                                objnewLogs.IntroducerName = UserName;
                                objnewLogs.OldValue = Owner.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Owners Information";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.Action = "Owners Information Updated";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CountryName = Country;
                                objnewLogs.IntroducerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                _context.IntroducerLogs.Add(objnewLogs);

                            }
                            else
                            {
                                CustomerLogs objnewLogs = new CustomerLogs();
                                objnewLogs.CustomerName = UserName;
                                objnewLogs.OldValue = Owner.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Owners Information";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.Action = "Owners Information Updated";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CountryName = Country;
                                objnewLogs.CustomerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                _context.CustomerLogs.Add(objnewLogs);
                            }


                            // Console.WriteLine($"The email is valid");
                            Owner.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();
                            return Json(true);
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }

                }
            }
            return Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> SaveRepresentativeFields(AuthorizedRepresentative objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email

            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //var BuisnessProfile = _context.BuisnessProfile.Where(x => x.UserId == UserId && x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                //BuisnessProfile.BuisnessName
                ///////////Buisness Name Update//////////
                ///
                var AuthorizedRepresentative = _context.AuthorizedRepresentative.Where(x => x.RepresentativeId == objFields.Formid).FirstOrDefault();

                if (objFields.FieldName == "Authfirstname" && objFields.FieldValue != AuthorizedRepresentative.FirstName)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Authorize Representative Information<br/>" +
                            "<b>Field Name: </b>First Name<br/>" +
                            "<b>Old Value: </b>" + AuthorizedRepresentative.FirstName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    AuthorizedRepresentative.FirstName = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authlastname" && objFields.FieldValue != AuthorizedRepresentative.LastName)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Authorize Representative Information<br/>" +
                            "<b>Field Name: </b>Last Name<br/>" +
                            "<b>Old Value: </b>" + AuthorizedRepresentative.LastName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.LastName = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authcountry" && objFields.FieldValue != AuthorizedRepresentative.Country)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Authorize Representative Information<br/>" +
                            "<b>Field Name: </b>Country<br/>" +
                            "<b>Old Value: </b>" + AuthorizedRepresentative.Country + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.Country = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authaddress1" && objFields.FieldValue != AuthorizedRepresentative.Address1)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Authorize Representative Information<br/>" +
                            "<b>Field Name: </b>Address 1<br/>" +
                            "<b>Old Value: </b>" + AuthorizedRepresentative.Address1 + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Address1;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Address 1";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Address1;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Address 1";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    AuthorizedRepresentative.Address1 = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authaddress2" && objFields.FieldValue != AuthorizedRepresentative.Address2)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Section Name: </b>Authorize Representative Information<br/>" +
                           "<b>Field Name: </b>Address 2<br/>" +
                           "<b>Old Value: </b>" + AuthorizedRepresentative.Address2 + "<br/>" +
                           "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC,<br/>" +
                             "IT Team" +
                           "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Address2;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Address 2";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.Address2;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Address 2";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    AuthorizedRepresentative.Address2 = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authcity" && objFields.FieldValue != AuthorizedRepresentative.City)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                          $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                          "<b>Customer Name: </b>" + UserName + "<br/>" +
                          "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                          "<b>Section Name: </b>Authorize Representative Information<br/>" +
                          "<b>Field Name: </b>City <br/>" +
                          "<b>Old Value: </b>" + AuthorizedRepresentative.City + "<br/>" +
                          "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                          "Thanks<br/>" +
                            "FGC,<br/>" +
                            "IT Team" +
                          "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "City";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "City";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }



                    AuthorizedRepresentative.City = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authpostcode" && objFields.FieldValue != AuthorizedRepresentative.PostCode)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Authorize Representative Information<br/>" +
                         "<b>Field Name: </b>Post code<br/>" +
                         "<b>Old Value: </b>" + AuthorizedRepresentative.PostCode + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }



                    AuthorizedRepresentative.PostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authcounty" && objFields.FieldValue != AuthorizedRepresentative.County)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                      $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                      "<b>Customer Name: </b>" + UserName + "<br/>" +
                      "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                      "<b>Section Name: </b>Authorize Representative Information<br/>" +
                      "<b>Field Name: </b>County<br/>" +
                      "<b>Old Value: </b>" + AuthorizedRepresentative.County + "<br/>" +
                      "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                      "Thanks<br/>" +
                        "FGC,<br/>" +
                        "IT Team" +
                      "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.County;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "County";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.County;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "County";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.County = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else if (objFields.FieldName == "Authdob" && Convert.ToDateTime(objFields.FieldValue) != AuthorizedRepresentative.DOB)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                     $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                     "<b>Customer Name: </b>" + UserName + "<br/>" +
                     "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                     "<b>Section Name: </b>Authorize Representative Information<br/>" +
                     "<b>Field Name: </b>Date of birth<br/>" +
                     "<b>Old Value: </b>" + Convert.ToDateTime(AuthorizedRepresentative.DOB).ToString("dd-MMM-yyyy")+ "<br/>" +
                     "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy")  + "<br/><br/><br/><br/>" +
                     "Thanks<br/>" +
                       "FGC,<br/>" +
                       "IT Team" +
                     "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(AuthorizedRepresentative.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(AuthorizedRepresentative.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.DOB = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authphonenumber" && objFields.FieldValue != AuthorizedRepresentative.PhoneNumber)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                     $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                     "<b>Customer Name: </b>" + UserName + "<br/>" +
                     "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                     "<b>Section Name: </b>Authorize Representative Information<br/>" +
                     "<b>Field Name: </b>Phone Number<br/>" +
                     "<b>Old Value: </b>" + AuthorizedRepresentative.PhoneNumber + "<br/>" +
                     "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                     "Thanks<br/>" +
                       "FGC,<br/>" +
                       "IT Team" +
                     "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.PhoneNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authemail" && objFields.FieldValue != AuthorizedRepresentative.Email)
                {
                    try
                    {
                        var mail = new MailAddress(objFields.FieldValue);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            AuthorizedRepresentative.Email = objFields.FieldValue;
                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            foreach (var itm in Emails)
                            {
                                //EmailsString += itm + ",";
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
                      $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                      "<b>Customer Name: </b>" + UserName + "<br/>" +
                      "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                      "<b>Section Name: </b>Authorize Representative Information<br/>" +
                      "<b>Field Name: </b>Email<br/>" +
                      "<b>Old Value: </b>" + AuthorizedRepresentative.Email + "<br/>" +
                      "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                      "Thanks<br/>" +
                        "FGC,<br/>" +
                        "IT Team" +
                      "");
                            }
                            if (Role == "Introducer")
                            {
                                IntroducerLogs objnewLogs = new IntroducerLogs();
                                objnewLogs.IntroducerName = UserName;
                                objnewLogs.OldValue = AuthorizedRepresentative.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Authorize Representative Information";
                                objnewLogs.Action = "Authorize Representative Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.IntroducerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CountryName = Country;
                                _context.IntroducerLogs.Add(objnewLogs);
                            }
                            else
                            {
                                CustomerLogs objnewLogs = new CustomerLogs();
                                objnewLogs.CustomerName = UserName;
                                objnewLogs.OldValue = AuthorizedRepresentative.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Authorize Representative Information";
                                objnewLogs.Action = "Authorize Representative Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.CustomerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CountryName = Country;
                                _context.CustomerLogs.Add(objnewLogs);
                            }


                            AuthorizedRepresentative.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();
                            return Json(true);
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                    }
                }
                else if (objFields.FieldName == "Authpositioninbuisness" && objFields.FieldValue != AuthorizedRepresentative.PositionInBuisness)
                {


                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
             $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
             "<b>Customer Name: </b>" + UserName + "<br/>" +
             "<b>Customer Email: </b>" + UserEmail + "<br/>" +
             "<b>Section Name: </b>Authorize Representative Information<br/>" +
             "<b>Field Name: </b>Position In Buisness<br/>" +
             "<b>Old Value: </b>" + AuthorizedRepresentative.PositionInBuisness + "<br/>" +
             "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
             "Thanks<br/>" +
               "FGC,<br/>" +
               "IT Team" +
             "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PositionInBuisness;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Position In Buisness";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PositionInBuisness;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Position In Buisness";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    AuthorizedRepresentative.PositionInBuisness = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authroleincharity" && objFields.FieldValue != AuthorizedRepresentative.RoleIncharity)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
$"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
"<b>Customer Name: </b>" + UserName + "<br/>" +
"<b>Customer Email: </b>" + UserEmail + "<br/>" +
"<b>Section Name: </b>Authorize Representative Information<br/>" +
"<b>Field Name: </b>Role In charity<br/>" +
"<b>Old Value: </b>" + AuthorizedRepresentative.RoleIncharity + "<br/>" +
"<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
"Thanks<br/>" +
"FGC,<br/>" +
"IT Team" +
"");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.RoleIncharity;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Role In Charity";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.RoleIncharity;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Role In Charity";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    AuthorizedRepresentative.RoleIncharity = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else if (objFields.FieldName == "Authpositionincompany" && objFields.FieldValue != AuthorizedRepresentative.PositionInComany)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")<b>",
$"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
"<b>Customer Name: </b>" + UserName + "<br/>" +
"<b>Customer Email: </b>" + UserEmail + "<br/>" +
"<b>Section Name: </b>Authorize Representative Information<br/>" +
"<b>Field Name: </b>Position In Comany<br/>" +
"<b>Old Value: </b>" + AuthorizedRepresentative.PositionInComany + "<br/>" +
"<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
"Thanks<br/>" +
"FGC,<br/>" +
"IT Team" +
"");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PositionInComany;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Position In Company";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = AuthorizedRepresentative.PositionInComany;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Authorize Representative Information";
                        objnewLogs.FieldName = "Position In Company";
                        objnewLogs.Action = "Authorize Representative Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    AuthorizedRepresentative.PositionInComany = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTrusteesFields(Trustees objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();

            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                var Trustees = _context.Trustees.Where(x => x.TrusteeId == objFields.Formid).FirstOrDefault();

                if (objFields.FieldName == "Trusteefirstname" && objFields.FieldValue != Trustees.FirstName)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Trustee Information<br/>" +
                            "<b>Field Name: </b>First Name<br/>" +
                            "<b>Old Value: </b>" + Trustees.FirstName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.FirstName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteelastname" && objFields.FieldValue != Trustees.LastName)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Section Name: </b>Trustee Information<br/>" +
                           "<b>Field Name: </b>Last Name<br/>" +
                           "<b>Old Value: </b>" + Trustees.LastName + "<br/>" +
                           "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC,<br/>" +
                             "IT Team" +
                           "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.LastName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteeaddress" && objFields.FieldValue != Trustees.Address)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Trustee Information<br/>" +
                         "<b>Field Name: </b>Address<br/>" +
                         "<b>Old Value: </b>" + Trustees.Address + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.LastName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "Last Name";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.Address;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.FieldName = "Adsress";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    Trustees.Address = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "Trusteecity" && objFields.FieldValue != Trustees.City)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>City<br/>" +
                        "<b>Old Value: </b>" + Trustees.City + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.OldValue = Trustees.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "City";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.OldValue = Trustees.City;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "City";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.City = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteepostcode" && objFields.FieldValue != Trustees.PostCode)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Post Code<br/>" +
                        "<b>Old Value: </b>" + Trustees.PostCode + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.PostCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Post Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.PostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteecountry" && objFields.FieldValue != Trustees.Country)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Country<br/>" +
                        "<b>Old Value: </b>" + Trustees.Country + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.IPAdress = ip;
                        //  objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //  objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.Country;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Country";
                        objnewLogs.IPAdress = ip;
                        //  objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //  objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.Country = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteenationality" && objFields.FieldValue != Trustees.Nationality)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Trustee Information<br/>" +
                         "<b>Field Name: </b>Nationality<br/>" +
                         "<b>Old Value: </b>" + Trustees.Nationality + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();

                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        // objnewLogs.CustomerName = applicationUser?.UserName;
                        objnewLogs.OldValue = Trustees.Nationality;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Nationality";
                        objnewLogs.IPAdress = ip;
                        //  objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //  objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();

                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        // objnewLogs.CustomerName = applicationUser?.UserName;
                        objnewLogs.OldValue = Trustees.Nationality;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Nationality";
                        objnewLogs.IPAdress = ip;
                        //  objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //  objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.Nationality = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteecounty" && objFields.FieldValue != Trustees.County)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>County<br/>" +
                        "<b>Old Value: </b>" + Trustees.County + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();

                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        // objnewLogs.CustomerName = applicationUser?.UserName;
                        objnewLogs.OldValue = Trustees.County;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "County";
                        objnewLogs.IPAdress = ip;
                        //   objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //   objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();

                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        // objnewLogs.CustomerName = applicationUser?.UserName;
                        objnewLogs.OldValue = Trustees.County;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "County";
                        objnewLogs.IPAdress = ip;
                        //   objnewLogs.CustomerId = Convert.ToInt32(applicationUser?.Id);
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        //   objnewLogs.Email = applicationUser?.Email;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    Trustees.County = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteedob" && Convert.ToDateTime(objFields.FieldValue) != Trustees.DOB)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Date of birth<br/>" +
                        "<b>Old Value: </b>" + Convert.ToDateTime(Trustees.DOB).ToString("dd-MMM-yyyy") + "<br/>" +
                        "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Trustees.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Trustees.DOB).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Date Of Birth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }
                    Trustees.DOB = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteeappdate" && Convert.ToDateTime(objFields.FieldValue) != Trustees.AppointmentDate)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Section Name: </b>Trustee Information<br/>" +
                       "<b>Field Name: </b>Date of appointment<br/>" +
                       "<b>Old Value: </b>" + Convert.ToDateTime(Trustees.AppointmentDate).ToString("dd-MMM-yyyy") + "<br/>" +
                       "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC,<br/>" +
                         "IT Team" +
                       "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Trustees.AppointmentDate).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Appointment Date";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Convert.ToDateTime(Trustees.AppointmentDate).ToString("dd-MMM-yyyy");
                        objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Appointment Date";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.AppointmentDate = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Trusteephonenumber" && objFields.FieldValue != Trustees.PhoneNumber)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Phone Number<br/>" +
                        "<b>Old Value: </b>" + Trustees.PhoneNumber + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.PhoneNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Phone Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.PhoneNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }


                else if (objFields.FieldName == "Trusteeemail" && objFields.FieldValue != Trustees.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.FieldValue);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            Trustees.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            // Console.WriteLine($"The email is valid");
                            foreach (var itm in Emails)
                            {
                                //EmailsString += itm + ",";
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Email<br/>" +
                        "<b>Old Value: </b>" + Trustees.Email + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                            }
                            if (Role == "Introducer")
                            {
                                IntroducerLogs objnewLogs = new IntroducerLogs();
                                objnewLogs.IntroducerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.Email = UserEmail;
                                objnewLogs.IntroducerName = UserName;
                                objnewLogs.OldValue = Trustees.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Trustee Information";
                                objnewLogs.Action = "Trustee Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.CountryName = Country;
                                _context.IntroducerLogs.Add(objnewLogs);
                            }
                            else
                            {
                                CustomerLogs objnewLogs = new CustomerLogs();
                                objnewLogs.CustomerId = UserId;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.Email = UserEmail;
                                objnewLogs.CustomerName = UserName;
                                objnewLogs.OldValue = Trustees.Email;
                                objnewLogs.NewValue = objFields.FieldValue;
                                objnewLogs.FormName = "Trustee Information";
                                objnewLogs.Action = "Trustee Information Updated";
                                objnewLogs.FieldName = "Email";
                                objnewLogs.IPAdress = ip;
                                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                                objnewLogs.CountryName = Country;
                                _context.CustomerLogs.Add(objnewLogs);
                            }



                            Trustees.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();
                            return Json(true);
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }
                }

                else if (objFields.FieldName == "Trusteerole" && objFields.FieldValue != Trustees.Role)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Trustee Information<br/>" +
                        "<b>Field Name: </b>Role<br/>" +
                        "<b>Old Value: </b>" + Trustees.Role + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = Trustees.Role;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Role";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Trustees.Role;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Trustee Information";
                        objnewLogs.Action = "Trustee Information Updated";
                        objnewLogs.FieldName = "Role";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Trustees.Role = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveFinancialInformationFields(FinancialInformation objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                var FinancialInformation = _context.FinancialInformation.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();


                if (objFields.FieldName == "PerMonth" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.PerMonth)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Financial Information<br/>" +
                            "<b>Field Name: </b>Per Month<br/>" +
                            "<b>Old Value: </b>" + FinancialInformation.PerMonth.ToString() + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PerMonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "PerMonth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PerMonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "PerMonth";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    FinancialInformation.PerMonth = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "PerAnum" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.PerAnum)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                             $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                             "<b>Customer Name: </b>" + UserName + "<br/>" +
                             "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                             "<b>Section Name: </b>Financial Information<br/>" +
                             "<b>Field Name: </b>Per Anum<br/>" +
                             "<b>Old Value: </b>" + FinancialInformation.PerAnum.ToString() + "<br/>" +
                             "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                             "Thanks<br/>" +
                               "FGC,<br/>" +
                               "IT Team" +
                             "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PerAnum.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Per Anum";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PerAnum.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Per Anum";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    FinancialInformation.PerAnum = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "PaymentIncoming" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.PaymentIncoming)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Section Name: </b>Financial Information<br/>" +
                           "<b>Field Name: </b>Payment Incoming<br/>" +
                           "<b>Old Value: </b>" + FinancialInformation.PaymentIncoming.ToString() + "<br/>" +
                           "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC,<br/>" +
                             "IT Team" +
                           "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PaymentIncoming.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payment Incoming";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PaymentIncoming.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payment Incoming";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    FinancialInformation.PaymentIncoming = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "PaymentOutgoing" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.PaymentOutgoing)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Section Name: </b>Financial Information<br/>" +
                           "<b>Field Name: </b>Payment Outgoing<br/>" +
                           "<b>Old Value: </b>" + FinancialInformation.PaymentOutgoing.ToString() + "<br/>" +
                           "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC,<br/>" +
                             "IT Team" +
                           "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PaymentOutgoing.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payment Outgoing";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.PaymentOutgoing.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payment Outgoing";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }

                    FinancialInformation.PaymentOutgoing = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "TransactionIncoming" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.TransactionIncoming)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                             $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                             "<b>Customer Name: </b>" + UserName + "<br/>" +
                             "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                             "<b>Section Name: </b>Financial Information<br/>" +
                             "<b>Field Name: </b>Transaction Incoming<br/>" +
                             "<b>Old Value: </b>" + FinancialInformation.TransactionIncoming.ToString() + "<br/>" +
                             "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                             "Thanks<br/>" +
                               "FGC,<br/>" +
                               "IT Team" +
                             "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.TransactionIncoming.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Transaction Incoming";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.TransactionIncoming.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Transaction Incoming";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }



                    FinancialInformation.TransactionIncoming = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }



                else if (objFields.FieldName == "TransactionOutgoing" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.TransactionOutgoing)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                             $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                             "<b>Customer Name: </b>" + UserName + "<br/>" +
                             "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                             "<b>Section Name: </b>Financial Information<br/>" +
                             "<b>Field Name: </b>Transaction Outgoing<br/>" +
                             "<b>Old Value: </b>" + FinancialInformation.TransactionOutgoing.ToString() + "<br/>" +
                             "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                             "Thanks<br/>" +
                               "FGC,<br/>" +
                               "IT Team" +
                             "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.TransactionOutgoing.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Transaction Outgoing";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.TransactionOutgoing.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Transaction Outgoing";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }


                    FinancialInformation.TransactionOutgoing = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "NoOfPaymentsPerMonth" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.NoOfPaymentsPerMonth)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Financial Information<br/>" +
                            "<b>Field Name: </b>No Of Payments Per Month<br/>" +
                            "<b>Old Value: </b>" + FinancialInformation.NoOfPaymentsPerMonth.ToString() + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.NoOfPaymentsPerMonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payments Per Month";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.NoOfPaymentsPerMonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Payments Per Month";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }



                    FinancialInformation.NoOfPaymentsPerMonth = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "VolumePermonth" && Convert.ToDecimal(objFields.FieldValue) != FinancialInformation.VolumePermonth)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Financial Information<br/>" +
                            "<b>Field Name: </b>Volume Per month<br/>" +
                            "<b>Old Value: </b>" + FinancialInformation.VolumePermonth.ToString() + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.VolumePermonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Volume Per month";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.VolumePermonth.ToString();
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Volume Per month";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }

                    FinancialInformation.VolumePermonth = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "BankName" && objFields.FieldValue != FinancialInformation.BankName)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Financial Information<br/>" +
                         "<b>Field Name: </b>Bank Name<br/>" +
                         "<b>Old Value: </b>" + FinancialInformation.BankName + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.BankName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Bank Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.BankName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Bank Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);


                    }

                    FinancialInformation.BankName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }


                else if (objFields.FieldName == "BankAddress" && objFields.FieldValue != FinancialInformation.BankAddress)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Financial Information<br/>" +
                         "<b>Field Name: </b>Bank Address<br/>" +
                         "<b>Old Value: </b>" + FinancialInformation.BankAddress + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.BankAddress;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Bank Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.BankAddress;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Bank Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }



                    FinancialInformation.BankAddress = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "SortCode" && objFields.FieldValue != FinancialInformation.SortCode)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Financial Information<br/>" +
                         "<b>Field Name: </b>Sort Code<br/>" +
                         "<b>Old Value: </b>" + FinancialInformation.SortCode + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.SortCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Sort Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.SortCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Sort Code";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }


                    FinancialInformation.SortCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "AccountName" && objFields.FieldValue != FinancialInformation.AccountName)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Financial Information<br/>" +
                         "<b>Field Name: </b>Account Name<br/>" +
                         "<b>Old Value: </b>" + FinancialInformation.AccountName + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }




                    FinancialInformation.AccountName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "AccountNumber" && objFields.FieldValue != FinancialInformation.AccountNumber)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Financial Information<br/>" +
                        "<b>Field Name: </b>Account Number<br/>" +
                        "<b>Old Value: </b>" + FinancialInformation.AccountNumber + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);

                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountNumber;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Number";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);

                    }


                    FinancialInformation.AccountNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "IBAN" && objFields.FieldValue != FinancialInformation.IBAN)
                {
                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Section Name: </b>Financial Information<br/>" +
                       "<b>Field Name: </b>IBAN<br/>" +
                       "<b>Old Value: </b>" + FinancialInformation.IBAN + "<br/>" +
                       "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC,<br/>" +
                         "IT Team" +
                       "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.IBAN;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "IBAN";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.IBAN;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "IBAN";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }



                    FinancialInformation.IBAN = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "SwiftCode" && objFields.FieldValue != FinancialInformation.SwiftCode)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                      $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                      "<b>Customer Name: </b>" + UserName + "<br/>" +
                      "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                      "<b>Section Name: </b>Financial Information<br/>" +
                      "<b>Field Name: </b>SwiftCode<br/>" +
                      "<b>Old Value: </b>" + FinancialInformation.SwiftCode + "<br/>" +
                      "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                      "Thanks<br/>" +
                        "FGC,<br/>" +
                        "IT Team" +
                      "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.SwiftCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "SwiftCode";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.SwiftCode;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "SwiftCode";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    FinancialInformation.SwiftCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "AccountCurrency" && objFields.FieldValue != FinancialInformation.AccountCurrency)
                {

                    foreach (var itm in Emails)
                    {
                        //EmailsString += itm + ",";
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                     $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                     "<b>Customer Name: </b>" + UserName + "<br/>" +
                     "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                     "<b>Section Name: </b>Financial Information<br/>" +
                     "<b>Field Name: </b>Account Currency<br/>" +
                     "<b>Old Value: </b>" + FinancialInformation.AccountCurrency+ "<br/>" +
                     "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                     "Thanks<br/>" +
                       "FGC,<br/>" +
                       "IT Team" +
                     "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountCurrency;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Currency";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = FinancialInformation.AccountCurrency;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Financial Information";
                        objnewLogs.FieldName = "Account Currency";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    FinancialInformation.AccountCurrency = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else
                {
                    return Json(false);
                }

            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRepresentativeFields(ParentProfile objFields)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);

            if (UserId != null)
            {

                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.AuthorizedRepresentative.BuisnessProfileId).FirstOrDefault();
                //objFields.AuthorizedRepresentative.BuisnessProfileId = objFields.objBuisness.BuisnessProfileId;
                _context.AuthorizedRepresentative.Add(objFields.AuthorizedRepresentative);
                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        //"<b>Section Name: </b>Financial Information<br/>" +
                        "<b>New Authorize Representative Added </b><br/>" +
                        "<b>FullName: </b>" + objFields.AuthorizedRepresentative.FirstName + " " + objFields.AuthorizedRepresentative.LastName + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    objnewLogs.FormName = "Authorized Representative Information";
                    objnewLogs.Action = "Authorized Representative  Added";

                    objnewLogs.IPAdress = ip;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Authorized Representative Information";
                    objnewLogs.Action = "Authorized Representative  Added";

                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }



                // _context.Trustees.Add(objFields.Trustees);


                _context.SaveChanges();

                return RedirectToAction("EditApplication", "Home", new { BuisnessProfileId = objFields.AuthorizedRepresentative.BuisnessProfileId });
            }
            else
            {
                return Json(true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRepresentative(int RepresentativeId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);

            var Authorized = _context.AuthorizedRepresentative.Where(x => x.RepresentativeId == RepresentativeId).FirstOrDefault();
            var BuisnessProfileId = Authorized.BuisnessProfileId;
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();

            Authorized.IsDelete = true;

            foreach (var itm in Emails)
            {
                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")<b/> has been updated:<br/>" +
                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                    //"<b>Section Name: </b>Financial Information<br/>" +
                    "<b>Authorize Represntative Deleted </b><br/>" +
                    "<b>FullName: </b>" + Authorized.FirstName + " " + Authorized.LastName + "<br/><br/><br/><br/>" +
                    "Thanks<br/>" +
                      "FGC,<br/>" +
                      "IT Team" +
                    "");
            }
            if (Role == "Introducer")
            {
                IntroducerLogs objnewLogs = new IntroducerLogs();
                objnewLogs.IntroducerName = UserName;
                //objnewLogs.OldValue = Director.FirstName;
                //objnewLogs.NewValue = objFields.FieldValue;
                objnewLogs.FormName = "Authorize Representaive Information";
                objnewLogs.Action = "Authorize Representaive Deleted";

                objnewLogs.IPAdress = ip;
                objnewLogs.IntroducerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.IntroducerLogs.Add(objnewLogs);
            }
            else
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                //objnewLogs.OldValue = Director.FirstName;
                //objnewLogs.NewValue = objFields.FieldValue;
                objnewLogs.FormName = "Authorize Representaive Information";
                objnewLogs.Action = "Authorize Representaive Deleted";

                objnewLogs.IPAdress = ip;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);
            }



            _context.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDirectorsFields(DirectorAndShareHolders objFields)
        {
            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);


            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            if (UserId != null)
            {
                var Director = _context.DirectorAndShareHolders.Where(x => x.DirectorId == objFields.Formid).FirstOrDefault();
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.BuisnessProfileId).FirstOrDefault();
                if (objFields.FieldName == "Directorfirstname" && objFields.FieldValue != Director.FirstName)
                {

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")<b/> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                            "<b>Field Name: </b>First Name<br/>" +
                            "<b>Old Value: </b>" + Director.FirstName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Authorize Representaive Information";
                        objnewLogs.Action = "Authorize Representaive Deleted";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.OldValue = Director.FirstName;
                        objnewLogs.NewValue = objFields.FieldValue;
                        objnewLogs.FormName = "Director Information";
                        objnewLogs.Action = "Director Information Updated";
                        objnewLogs.FieldName = "First Name";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    Director.FirstName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directorlastname" && objFields.FieldValue != Director.LastName)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                            "<b>Field Name: </b>Last Name<br/>" +
                            "<b>Old Value: </b>" + Director.LastName + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.LastName;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Last Name";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.LastName = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directoraddress" && objFields.FieldValue != Director.Address)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                            "<b>Field Name: </b>Address<br/>" +
                            "<b>Old Value: </b>" + Director.Address + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.Address;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Address";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.Address = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directorcity" && objFields.FieldValue != Director.City)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                            "<b>Field Name: </b>City<br/>" +
                            "<b>Old Value: </b>" + Director.City + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.City;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "City";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.City = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directorpostcode" && objFields.FieldValue != Director.PostCode)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                            "<b>Field Name: </b>Post Code<br/>" +
                            "<b>Old Value: </b>" + Director.PostCode + "<br/>" +
                            "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.PostCode;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "PostCode";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.PostCode = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directorcountry" && objFields.FieldValue != Director.Country)
                {

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                           "<b>Field Name: </b>Country<br/>" +
                           "<b>Old Value: </b>" + Director.Country + "<br/>" +
                           "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC,<br/>" +
                             "IT Team" +
                           "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.Country;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Country";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.Country = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directornationality" && objFields.FieldValue != Director.Nationality)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                         "<b>Field Name: </b>Nationality<br/>" +
                         "<b>Old Value: </b>" + Director.Nationality + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.Nationality;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Nationality";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.Nationality = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directorcounty" && objFields.FieldValue != Director.County)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                        "<b>Field Name: </b>County<br/>" +
                        "<b>Old Value: </b>" + Director.County + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.County;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "County";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.County = objFields.FieldValue;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }

                else if (objFields.FieldName == "Directordob" && Convert.ToDateTime(objFields.FieldValue) != Director.DOB)
                {

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                        "<b>Field Name: </b>date of birth<br/>" +
                        "<b>Old Value: </b>" + Director.DOB.Value.ToString("dd-MMM-yyyy") + "<br/>" +
                        "<b>New Value: </b>" + Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy") + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.DOB.Value.ToString("dd-MMM-yyyy");
                    objnewLogs.NewValue = Convert.ToDateTime(objFields.FieldValue).ToString("dd-MMM-yyyy");
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Date of Birth";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.DOB = Convert.ToDateTime(objFields.FieldValue);
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                else if (objFields.FieldName == "Directorphonenumber" && objFields.FieldValue != Director.PhoneNumber)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                        "<b>Field Name: </b>Phone Number<br/>" +
                        "<b>Old Value: </b>" + Director.PhoneNumber + "<br/>" +
                        "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.PhoneNumber;
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Phone Number";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.PhoneNumber = objFields.FieldValue;
                    await _context.SaveChangesAsync();

                    return Json(true);
                }


                else if (objFields.FieldName == "Directoremail" && objFields.FieldValue != Director.Email)
                {
                    try
                    {
                        // string email = "ar@gmail.com";
                        //Console.WriteLine($"The email is {email}");
                        var mail = new MailAddress(objFields.FieldValue);
                        bool isValidEmail = mail.Host.Contains(".");
                        if (!isValidEmail)
                        {
                            // Console.WriteLine($"The email is invalid");
                            //Director.Email = objFields.FieldValue;
                            //await _context.SaveChangesAsync();

                            return Json(new { flag = true, validation = "Invalid Email Address" });
                        }
                        else
                        {
                            foreach (var itm in Emails)
                            {
                                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                         $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                         "<b>Customer Name: </b>" + UserName + "<br/>" +
                         "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                         "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                         "<b>Field Name: </b>Email<br/>" +
                         "<b>Old Value: </b>" + Director.Email + "<br/>" +
                         "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                         "Thanks<br/>" +
                           "FGC,<br/>" +
                           "IT Team" +
                         "");
                            }
                            CustomerLogs objnewLogs = new CustomerLogs();
                            objnewLogs.CustomerName = UserName;
                            objnewLogs.OldValue = Director.Email;
                            objnewLogs.NewValue = objFields.FieldValue;
                            objnewLogs.FormName = "Director Information";
                            objnewLogs.Action = "Director Information Updated";
                            objnewLogs.FieldName = "Email";
                            objnewLogs.IPAdress = ip;
                            objnewLogs.CustomerId = UserId;
                            objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                            objnewLogs.Email = UserEmail;
                            objnewLogs.CountryName = Country;
                            _context.CustomerLogs.Add(objnewLogs);
                            // Console.WriteLine($"The email is valid");
                            Director.Email = objFields.FieldValue;
                            await _context.SaveChangesAsync();

                            return Json(true);
                        }
                        //  Console.ReadLine();
                    }
                    catch (Exception)
                    {
                        return Json(new { flag = true, validation = "Invalid Email Address" });
                        //  Console.ReadLine();
                    }
                }
                else if (objFields.FieldName == "Directorshareholders" && Convert.ToDecimal(objFields.FieldValue) != Director.ShareHolders_percentage)
                {
                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                          $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                          "<b>Customer Name: </b>" + UserName + "<br/>" +
                          "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                          "<b>Section Name: </b>Directors & Shareholders Information<br/>" +
                          "<b>Field Name: </b>Email<br/>" +
                          "<b>Old Value: </b>" + Director.ShareHolders_percentage.ToString() + "<br/>" +
                          "<b>New Value: </b>" + objFields.FieldValue + "<br/><br/><br/><br/>" +
                          "Thanks<br/>" +
                            "FGC,<br/>" +
                            "IT Team" +
                          "");
                    }
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.OldValue = Director.ShareHolders_percentage.ToString();
                    objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Information Updated";
                    objnewLogs.FieldName = "Shareholders percentage";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);

                    Director.ShareHolders_percentage = Convert.ToDecimal(objFields.FieldValue);
                    await _context.SaveChangesAsync();

                    return Json(true);
                }

                else
                {
                    return Json(false);
                }

            }
            else
            {
                return Json(false);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddDirectorFields(ParentProfile objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email

            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);


            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.DirectorAndShareHolders.BuisnessProfileId).FirstOrDefault();


                //objFields.AuthorizedRepresentative.BuisnessProfileId = objFields.objBuisness.BuisnessProfileId;
                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        //"<b>Section Name: </b>Financial Information<br/>" +
                        "<b>New Director Added </b><br/>" +
                        "<b>FullName: </b>" + objFields.DirectorAndShareHolders.FirstName + " " + objFields.DirectorAndShareHolders.LastName + "<br/>" +
                        "<br/><br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    //objnewLogs.OldValue = Director.FirstName;
                    //objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Added";

                    objnewLogs.IPAdress = ip;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    //objnewLogs.OldValue = Director.FirstName;
                    //objnewLogs.NewValue = objFields.FieldValue;
                    objnewLogs.FormName = "Director Information";
                    objnewLogs.Action = "Director Added";

                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }


                _context.DirectorAndShareHolders.Add(objFields.DirectorAndShareHolders);
                _context.SaveChanges();

                return RedirectToAction("EditApplication", "Home", new { BuisnessProfileId = objFields.DirectorAndShareHolders.BuisnessProfileId, Link = "Link3" });
            }
            else
            {
                return Json(true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddTrusteeFields(ParentProfile objFields)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            if (UserId != null)
            {
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == objFields.Trustees.BuisnessProfileId).FirstOrDefault();

                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        //"<b>Section Name: </b>Financial Information<br/>" +
                        "<b>New Trustee Added </b><br/>" +
                        "<b>FullName: </b>" + objFields.Trustees.FirstName + " " + objFields.Trustees.LastName + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    objnewLogs.FormName = "Trustee Information";
                    objnewLogs.Action = "Trustee Added";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Trustee Information";
                    objnewLogs.Action = "Trustee Added";
                    objnewLogs.IPAdress = ip;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }



                //Trustees trustee = new Trustees();

                //trustee.FirstName = objFields.Trustees.FirstName;
                //trustee.LastName = objFields.Trustees.LastName;
                //trustee.Address = objFields.Trustees.Address;
                //trustee.City = objFields.Trustees.City;
                //trustee.PostCode = objFields.Trustees.PostCode;
                //trustee.County = objFields.Trustees.County;
                //trustee.Country = objFields.Trustees.Country;
                //trustee.Nationality = objFields.Trustees.Nationality;
                //trustee.DOB = objFields.Trustees.DOB;
                //trustee.PhoneNumber = objFields.Trustees.PhoneNumber;
                //trustee.Role = objFields.Trustees.Role;
                //trustee.AppointmentDate = objFields.Trustees.AppointmentDate;

                _context.Trustees.Add(objFields.Trustees);
                _context.SaveChanges();

                return RedirectToAction("EditApplication", "Home", new { BuisnessProfileId = objFields.Trustees.BuisnessProfileId, Link = "Link3" });
            }
            else
            {
                return Json(true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveDirector(int DirectorId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);

            var Authorized = _context.DirectorAndShareHolders.Where(x => x.DirectorId == DirectorId).FirstOrDefault();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == Authorized.BuisnessProfileId).FirstOrDefault();

            Authorized.IsDelete = true;


            foreach (var itm in Emails)
            {
                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                    //"<b>Section Name: </b>Financial Information<br/>" +
                    "<b>Director Deleted </b><br/>" +
                    "<b>FullName: </b>" + Authorized.FirstName + " " + Authorized.LastName + "<br/>" +
                    "<br/><br/><br/>" +
                    "Thanks<br/>" +
                      "FGC,<br/>" +
                      "IT Team" +
                    "");
            }
            if (Role == "Introducer")
            {
                IntroducerLogs objnewLogs = new IntroducerLogs();
                objnewLogs.IntroducerName = UserName;
                objnewLogs.FormName = "Director Information";
                objnewLogs.Action = "Director Deleted";
                objnewLogs.IPAdress = ip;
                objnewLogs.IntroducerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.IntroducerLogs.Add(objnewLogs);
            }
            else
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                objnewLogs.FormName = "Director Information";
                objnewLogs.Action = "Director Deleted";
                objnewLogs.IPAdress = ip;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);
            }

            _context.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTrsutee(int TrusteeId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);

            var Authorized = _context.Trustees.Where(x => x.TrusteeId == TrusteeId).FirstOrDefault();
            var BuisnessProfileId = Authorized.BuisnessProfileId;
            Authorized.IsDelete = true;

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessProfileId).FirstOrDefault();


            foreach (var itm in Emails)
            {
                await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                    $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                    "<b>Customer Name: </b>" + UserName + "<br/>" +
                    "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                    //"<b>Section Name: </b>Financial Information<br/>" +
                    "<b>Trustee Deleted </b><br/>" +
                    "<b>FullName: </b>" + Authorized.FirstName + " " + Authorized.LastName + "<br/>" +
                    "<br/><br/><br/>" +
                    "Thanks<br/>" +
                      "FGC,<br/>" +
                      "IT Team" +
                    "");
            }
            if (Role == "Introducer")
            {
                IntroducerLogs objnewLogs = new IntroducerLogs();
                objnewLogs.IntroducerName = UserName;
                //objnewLogs.OldValue = Director.FirstName;
                //objnewLogs.NewValue = objFields.FieldValue;
                objnewLogs.FormName = "Trustee Information";
                objnewLogs.Action = "Trustee Deleted";

                objnewLogs.IPAdress = ip;
                objnewLogs.IntroducerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.IntroducerLogs.Add(objnewLogs);
            }
            else
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                //objnewLogs.OldValue = Director.FirstName;
                //objnewLogs.NewValue = objFields.FieldValue;
                objnewLogs.FormName = "Trustee Information";
                objnewLogs.Action = "Trustee Deleted";

                objnewLogs.IPAdress = ip;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);
            }

            _context.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> BuisnessFile1(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);


            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // string UserId = applicationUser?.Id; // will give the user's Email
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Buisness File Uploaded</b><br/>" +
                            "<b>Field Name: </b> Proof of Business Address<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Business Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Business Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }

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

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            List<int> AttachementIds = new List<int>();

            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Buisness File Uploaded</b><br/>" +
                            "<b>Field Name: </b> Regulatory License<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    //var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Buisness3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Buisness File Uploaded</b><br/>" +
                            "<b>Field Name: </b> Annual Accounts<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Annual Accounts";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Annual Accounts";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


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
        public async Task<IActionResult> DeleteFile(int Documentid)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            var BuisnessAttachment = _context.BuisnessAttachemtns.Where(x => x.DocumentId == Documentid).FirstOrDefault();
            string FieldName = "";
            BuisnessAttachment.IsDelete = true;

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessAttachment.BuisnessProfileId).FirstOrDefault();
            //   _context.BuisnessAttachemtns.Remove(BuisnessAttachment);

            if (BuisnessAttachment.BuisnessTypeId == 1)
            {


                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    objnewLogs.FormName = "Buisness Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Buisness1")
                    {
                        objnewLogs.FieldName = "Proof of Business Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Buisness2")
                    {
                        objnewLogs.FieldName = "Regulatory License";
                    }
                    else if (BuisnessAttachment.DocumentType == "Buisness3")
                    {
                        objnewLogs.FieldName = "Annual Accounts";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Buisness Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Buisness1")
                    {
                        objnewLogs.FieldName = "Proof of Business Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Buisness2")
                    {
                        objnewLogs.FieldName = "Regulatory License";
                    }
                    else if (BuisnessAttachment.DocumentType == "Buisness3")
                    {
                        objnewLogs.FieldName = "Annual Accounts";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }



                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Buisness File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }

            }
            else if (BuisnessAttachment.BuisnessTypeId == 2)
            {
                if (Role == "Introducer")
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Charity Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Charity1")
                    {
                        objnewLogs.FieldName = "Proof of Charity Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Charity2")
                    {
                        objnewLogs.FieldName = "Regulatory License";
                    }
                    else if (BuisnessAttachment.DocumentType == "Charity3")
                    {
                        objnewLogs.FieldName = "Registration certification, CIO registration, Annual Accounts, Organisational chart & Policies";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Charity Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Charity1")
                    {
                        objnewLogs.FieldName = "Proof of Charity Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Charity2")
                    {
                        objnewLogs.FieldName = "Regulatory License";
                    }
                    else if (BuisnessAttachment.DocumentType == "Charity3")
                    {
                        objnewLogs.FieldName = "Registration certification, CIO registration, Annual Accounts, Organisational chart & Policies";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }



                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Charity File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
            }
            else if (BuisnessAttachment.BuisnessTypeId == 3)
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                objnewLogs.FormName = "Company Documents";
                objnewLogs.Action = "File Deleted";
                if (BuisnessAttachment.DocumentType == "Company1")
                {
                    objnewLogs.FieldName = "Proof of Business Address";
                }
                else if (BuisnessAttachment.DocumentType == "Company2")
                {
                    objnewLogs.FieldName = "Regulatory License";
                }
                else if (BuisnessAttachment.DocumentType == "Company3")
                {
                    objnewLogs.FieldName = "Incorporation, Memorandum , Articles of Association, Annual Accounts & Organisational chart";
                }
                FieldName = objnewLogs.FieldName;
                objnewLogs.IPAdress = ip;
                objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);

                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Company File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }

            }
            _context.SaveChanges();
            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePersonalFiles(int Documentid)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var BuisnessAttachment = _context.PersonalDocuments.Where(x => x.DocumentId == Documentid).FirstOrDefault();
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == BuisnessAttachment.BuisnessProfileId).FirstOrDefault();
            BuisnessAttachment.IsDelete = true;
            string FieldName = "";
            if (BuisnessAttachment.BuisnessTypeId == 1)
            {
                if (Role == "Introducer")
                {
                    IntroducerLogs objnewLogs = new IntroducerLogs();
                    objnewLogs.IntroducerName = UserName;
                    objnewLogs.FormName = "Buisness Personal Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Sole1")
                    {
                        objnewLogs.FieldName = "Proof of ID";
                    }
                    else if (BuisnessAttachment.DocumentType == "Sole2")
                    {
                        objnewLogs.FieldName = "Proof of Personal Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Sole3")
                    {
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.IntroducerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.IntroducerLogs.Add(objnewLogs);
                }
                else
                {
                    CustomerLogs objnewLogs = new CustomerLogs();
                    objnewLogs.CustomerName = UserName;
                    objnewLogs.FormName = "Buisness Personal Documents";
                    objnewLogs.Action = "File Deleted";
                    if (BuisnessAttachment.DocumentType == "Sole1")
                    {
                        objnewLogs.FieldName = "Proof of ID";
                    }
                    else if (BuisnessAttachment.DocumentType == "Sole2")
                    {
                        objnewLogs.FieldName = "Proof of Personal Address";
                    }
                    else if (BuisnessAttachment.DocumentType == "Sole3")
                    {
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                    }
                    FieldName = objnewLogs.FieldName;
                    objnewLogs.IPAdress = ip;
                    objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                    objnewLogs.CustomerId = UserId;
                    objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                    objnewLogs.Email = UserEmail;
                    objnewLogs.CountryName = Country;
                    _context.CustomerLogs.Add(objnewLogs);
                }


                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Buisness Personal File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }

            }
            else if (BuisnessAttachment.BuisnessTypeId == 2)
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                objnewLogs.FormName = "Charity Documents";
                objnewLogs.Action = "File Deleted";
                if (BuisnessAttachment.DocumentType == "Charity1")
                {
                    objnewLogs.FieldName = "Proof of ID of each trustee";
                }
                else if (BuisnessAttachment.DocumentType == "Charity2")
                {
                    objnewLogs.FieldName = "Proof of Personal Address";
                }
                else if (BuisnessAttachment.DocumentType == "Charity3")
                {
                    objnewLogs.FieldName = "Proof of ID & Address of Each Authorized representative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                }
                FieldName = objnewLogs.FieldName;
                objnewLogs.IPAdress = ip;
                objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);

                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Charity Personal File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
            }
            else if (BuisnessAttachment.BuisnessTypeId == 3)
            {
                CustomerLogs objnewLogs = new CustomerLogs();
                objnewLogs.CustomerName = UserName;
                objnewLogs.FormName = "Company Documents";
                objnewLogs.Action = "File Deleted";
                if (BuisnessAttachment.DocumentType == "Buisness1")
                {
                    objnewLogs.FieldName = "Proof of ID of each director and shareholder (Incase of European Nationals ID should be notarized)";
                }
                else if (BuisnessAttachment.DocumentType == "Buinsess2")
                {
                    objnewLogs.FieldName = "Proof of Personal Address";
                }
                else if (BuisnessAttachment.DocumentType == "Buinsess3")
                {
                    objnewLogs.FieldName = "Proof of ID & Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                }
                objnewLogs.IPAdress = ip;
                objnewLogs.Remarks = BuisnessAttachment.DisplayFilename;
                objnewLogs.CustomerId = UserId;
                objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                objnewLogs.Email = UserEmail;
                objnewLogs.CountryName = Country;
                _context.CustomerLogs.Add(objnewLogs);

                foreach (var itm in Emails)
                {
                    await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Company Personal File Deleted</b><br/>" +
                        "<b>Field Name: </b> " + objnewLogs.FieldName + "<br/>" +
                        "<b>File Name: </b>" + BuisnessAttachment.DisplayFilename + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC,<br/>" +
                          "IT Team" +
                        "");
                }
            }

            _context.SaveChanges();
            var fulPath = Path.Combine(_env.ContentRootPath, "wwwroot\\UploadedDocs", BuisnessAttachment.Filename);
            FileInfo file = new FileInfo(fulPath);
            if (file.Exists)//check file exsit or not  
            {
                file.MoveTo(fulPath + "(Deleted)");
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> SoloPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            var FieldName = "";
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                // string UserId = applicationUser?.Id; // will give the user's Email

                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();


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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);



                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID";
                        FieldName = "Proof of ID";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID";
                        FieldName = "Proof of ID";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                    }


                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Sole Personal File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            var FieldName = "";
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //string UserId = applicationUser?.Id; // will give the user's Email

                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();



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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;

                        FieldName = objnewLogs.FieldName;
                        _context.IntroducerLogs.Add(objnewLogs);
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Sole Personal File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                // string UserId = applicationUser?.Id; // will give the user's Email

                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var SoleDocument = _context.SoleDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();

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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = SoleDocument.Id;
                    _context.PersonalDocuments.Add(obj);
                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID &amp; Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Sole Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID &amp; Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }




                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Sole Personal File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

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
        public async Task<IActionResult> CharityFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            var FieldName = "";
            string Country = GetUserCountryByIp(ip);
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // string UserId = applicationUser?.Id; // will give the user's Email
                    //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Charity Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Charity Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Charity File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }
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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);

            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);

            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);


                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application <b>(" + BuisnessProfile.BuisnessName + ")</b> has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Charity File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC,<br/>" +
                              "IT Team" +
                            "");
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            var FieldName = "";
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // string UserId = applicationUser?.Id; // will give the user's Email
                    //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Charity3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Registration certification, CIO registration, Annual Accounts, Organisational chart & Policies";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Registration certification, CIO registration, Annual Accounts, Organisational chart & Policies";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                            $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                            "<b>Customer Name: </b>" + UserName + "<br/>" +
                            "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                            "<b>Charity File Uploaded</b><br/>" +
                            "<b>Field Name: </b> " + FieldName + "<br/>" +
                            "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                            "<br/><br/><br/>" +
                            "Thanks<br/>" +
                              "FGC," +
                              "IT Team" +
                            "");
                    }

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
        public async Task<IActionResult> CharityPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;
            List<int> AttachementIds = new List<int>();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            try
            {
                //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                // string UserId = applicationUser?.Id; // will give the user's Email
                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();


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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID of each trustee";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID of each trustee";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                           $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                           "<b>Customer Name: </b>" + UserName + "<br/>" +
                           "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                           "<b>Charity Personal File Uploaded</b><br/>" +
                           "<b>Field Name: </b> " + FieldName + "<br/>" +
                           "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                           "<br/><br/><br/>" +
                           "Thanks<br/>" +
                             "FGC," +
                             "IT Team" +
                           "");

                    }
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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();

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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Charity Personal File Uploaded</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC," +
                          "IT Team" +
                        "");
                      
                    }
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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            try
            {
                //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email

                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var CharityDocument = _context.CharityDocument.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();

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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = CharityDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized representative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Charity Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized representative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }



                    _context.SaveChanges();


                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                          $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                          "<b>Customer Name: </b>" + UserName + "<br/>" +
                          "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                          "<b>Charity Personal File Uploaded</b><br/>" +
                          "<b>Field Name: </b> " + FieldName + "<br/>" +
                          "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                          "<br/><br/><br/>" +
                          "Thanks<br/>" +
                            "FGC," +
                            "IT Team" +
                          "");
                    }
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
        public async Task<IActionResult> CompanyFile1(IList<IFormFile> filearray)
        {
            var UploadedFiles = Request.Form.Files;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);


            var FieldName = "";
            List<int> AttachementIds = new List<int>();
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company1";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);


                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Business Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Business Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);


                        FieldName = objnewLogs.FieldName;
                    }

                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Company File Uploaded</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC," +
                          "IT Team" +
                        "");
                    
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //   string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            var FieldName = "";
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    // string UserId = applicationUser?.Id; // will give the user's Email
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company2";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Regulatory License";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }



                    _context.SaveChanges();


                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Company File Uploaded</b><br/>" +
                       "<b>Field Name: </b> " + FieldName + "<br/>" +
                       "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                       "<br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC," +
                         "IT Team" +
                       "");
                     
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
            int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
            var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();

            var FieldName = "";
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
                    //    IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                    //  string UserId = applicationUser?.Id; // will give the user's Email
                    // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();

                    BuisnessAttachemtns obj = new BuisnessAttachemtns();
                    obj.DocumentType = "Company3";
                    obj.Filename = FileName;
                    obj.DisplayFilename = formFile.FileName;
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    _context.BuisnessAttachemtns.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Incorporation, Memorandum , Articles of Association, Annual Accounts & Organisational chart";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);


                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Company Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Incorporation, Memorandum , Articles of Association, Annual Accounts & Organisational chart";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Company File Uploaded</b><br/>" +
                       "<b>Field Name: </b> " + FieldName + "<br/>" +
                       "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                       "<br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC," +
                         "IT Team" +
                       "");
                       
                    }

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
        public async Task<IActionResult> BuisnessPersonalFile1(IList<IFormFile> filearray)
        {

            var UploadedFiles = Request.Form.Files;

            List<int> AttachementIds = new List<int>();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            var FieldName = "";
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            try
            {
                //  IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                // var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID of each director and shareholder (Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID of each director and shareholder (Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Buisness Personal File Uploaded</b><br/>" +
                       "<b>Field Name: </b> " + FieldName + "<br/>" +
                       "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                       "<br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC," +
                         "IT Team" +
                       "");
                       
                    }

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
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            // string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);

            var FieldName = "";
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                //  string UserId = applicationUser?.Id; // will give the user's Email
                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);


                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of Personal Address";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }

                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                       $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                       "<b>Customer Name: </b>" + UserName + "<br/>" +
                       "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                       "<b>Buisness Personal File Uploaded</b><br/>" +
                       "<b>Field Name: </b> " + FieldName + "<br/>" +
                       "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                       "<br/><br/><br/>" +
                       "Thanks<br/>" +
                         "FGC," +
                         "IT Team" +
                       "");
                    }


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
            var FieldName = "";

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string UserId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            string UserEmail = claims.Where(x => x.Type == "Email").Select(x => x.Value).FirstOrDefault();
            string UserName = claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault();
            var Emails = _context.EventsEmails.Where(x => x.EmailEventsId == 2).Select(x => x.Email).ToList();
            string Role = claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault();
            //   IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            //  string UserId = applicationUser?.Id; // will give the user's Email
            string ip = GetClientIPAddress(HttpContext);
            string Country = GetUserCountryByIp(ip);
            try
            {
                // IdentityUser applicationUser = await _userManager.GetUserAsync(User);
                // string UserId = applicationUser?.Id; // will give the user's Email
                int BuisnessTypeId = Convert.ToInt32(Request.Form.Where(x => x.Key == "BuisnessTypeId").FirstOrDefault().Value);
                int buisnessprofileid = Convert.ToInt32(Request.Form.Where(x => x.Key == "buisnessprofileid").FirstOrDefault().Value);
                //  var BuisnessProfile = _context.BuisnessProfile.Include(s => s.BuisnessTypes).Include(s => s.BuisnessSector).Where(x => x.UserId == UserId && x.IsComplete == false).OrderByDescending(x => x.BuisnessProfileId).FirstOrDefault();
                var BuisnessDocument = _context.BuisnessDocuments.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
                var BuisnessProfile = _context.BuisnessProfile.Where(x => x.BuisnessProfileId == buisnessprofileid).FirstOrDefault();
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
                    obj.BuisnessProfileId = buisnessprofileid;
                    obj.BuisnessTypeId = BuisnessTypeId;
                    obj.DocumentTypeId = BuisnessDocument.Id;
                    _context.PersonalDocuments.Add(obj);

                    if (Role == "Introducer")
                    {
                        IntroducerLogs objnewLogs = new IntroducerLogs();
                        objnewLogs.IntroducerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.IntroducerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.IntroducerLogs.Add(objnewLogs);

                        FieldName = objnewLogs.FieldName;
                    }
                    else
                    {
                        CustomerLogs objnewLogs = new CustomerLogs();
                        objnewLogs.CustomerName = UserName;
                        objnewLogs.FormName = "Buisness Personal Documents";
                        objnewLogs.Action = "File Uploaded";
                        objnewLogs.FieldName = "Proof of ID & Address of Each Authorized Represesntative (passport, utility bill, driving license, state issued ID, Incase of European Nationals ID should be notarized)";
                        objnewLogs.IPAdress = ip;
                        objnewLogs.Remarks = formFile.FileName;
                        objnewLogs.CustomerId = UserId;
                        objnewLogs.ActionTime = DateTime.Now.DateTime_UK();
                        objnewLogs.Email = UserEmail;
                        objnewLogs.CountryName = Country;
                        _context.CustomerLogs.Add(objnewLogs);
                        FieldName = objnewLogs.FieldName;
                    }


                    _context.SaveChanges();
                    AttachementIds.Add(obj.DocumentId);

                    _context.SaveChanges();

                    foreach (var itm in Emails)
                    {
                        await _emailSender.SendEmailAsync(itm, "Application form updated <b>(" + BuisnessProfile.BuisnessName + ")</b>",
                        $"Hi onboarding Team, <br/><br/> Following information of the application (" + BuisnessProfile.BuisnessName + ") has been updated:<br/>" +
                        "<b>Customer Name: </b>" + UserName + "<br/>" +
                        "<b>Customer Email: </b>" + UserEmail + "<br/>" +
                        "<b>Buisness Personal File Uploaded</b><br/>" +
                        "<b>Field Name: </b> " + FieldName + "<br/>" +
                        "<b>File Name: </b>" + formFile.FileName + "<br/>" +
                        "<br/><br/><br/>" +
                        "Thanks<br/>" +
                          "FGC," +
                          "IT Team" +
                        "");
                    }

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
    }
}
