using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.ApiModels
{
    public class CompaniesModels
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Activity
        {
            public string code { get; set; }
            public string industrySector { get; set; }
            public string description { get; set; }
            public string classification { get; set; }
        }
        public class AdditionalData
        {
        }
        public class Address
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }
        public class Company
        {
            public string country { get; set; }
            public string id { get; set; }
            public string safeNo { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string officeType { get; set; }
            public string status { get; set; }
            public string regNo { get; set; }
            public string vatNo { get; set; }
            public Address address { get; set; }
            public Activity activity { get; set; }
            public string legalForm { get; set; }
            public AdditionalData additionalData { get; set; }
            public DateTime dateOfLatestAccounts { get; set; }
            public DateTime dateOfLatestChange { get; set; }
            public string activityCode { get; set; }
        }
        public class Message
        {
            public string type { get; set; }
            public string code { get; set; }
            public string text { get; set; }
        }
        public class Root
        {
            public string chargeReference { get; set; }
            public List<Message> messages { get; set; }
            public List<Company> companies { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public int totalSize { get; set; }
        }
    }
}
