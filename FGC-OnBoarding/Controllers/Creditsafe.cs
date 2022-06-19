using FGC_OnBoarding.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static FGC_OnBoarding.Models.ApiModels.CompaniesModels;

namespace FGC_OnBoarding.Controllers
{
    public class Creditsafe : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public Creditsafe(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;
        public async Task<IActionResult> CreditSafeData()
        {

            ViewBag.msg = TempData["msg"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreditSafeData(SearchModel objSearchModel)
        {
            var definition = new { token = "" };

            string TokenEndPoint = "https://connect.sandbox.creditsafe.com/v1/authenticate";
            string CompanyEndPoint = "https://connect.sandbox.creditsafe.com/v1/companies";
            AuthenticationModel objModel = new AuthenticationModel();
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(objModel), Encoding.UTF8, "application/json");
                using (var Response = await client.PostAsync(TokenEndPoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                         definition = JsonConvert.DeserializeAnonymousType(Response.Content.ReadAsStringAsync().Result, definition);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        TempData["msg"] = "Authentication Failed";
                        return RedirectToAction("CreditSafeData");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            if (definition.token != "")
            {
                var request = new HttpRequestMessage(HttpMethod.Get, CompanyEndPoint +"/?countries="+ objSearchModel.countries+"&regNo="+ objSearchModel.regNo);
                var clientt = _httpClientFactory.CreateClient();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", definition.token);

                HttpResponseMessage response = await clientt.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiString = response.Content.ReadAsStringAsync().Result;
                    CompaniesModels.Root objCompany = JsonConvert.DeserializeObject<CompaniesModels.Root>(apiString);
                    if (objCompany != null)
                    {
                        var Companies = objCompany.companies;
                        var ConnectId = Companies[0].id;

                        if (ConnectId != "")
                        {
                            var CompanyDetailsRequest = new HttpRequestMessage(HttpMethod.Get, "https://connect.sandbox.creditsafe.com/v1/companies/" +ConnectId);
                            var CompanyClient = _httpClientFactory.CreateClient();
                            CompanyDetailsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", definition.token);
                            HttpResponseMessage Companyresponse = await clientt.SendAsync(CompanyDetailsRequest, HttpCompletionOption.ResponseHeadersRead);
                            if (Companyresponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var CopmayapiString = Companyresponse.Content.ReadAsStringAsync().Result;
                                CompanyDetailModel.Root objDetails = JsonConvert.DeserializeObject<CompanyDetailModel.Root>(CopmayapiString);

                            }
                        }
                    }
                }
                else
                {
                    TempData["msg"] = "No Company Found..";
                    return RedirectToAction("CreditSafeData");
                }
            }
            else
            {
                TempData["msg"] = "Unable To Access Api";
                return RedirectToAction("CreditSafeData");
            }
            return RedirectToAction("CreditSafeData");
        }
    }
}
