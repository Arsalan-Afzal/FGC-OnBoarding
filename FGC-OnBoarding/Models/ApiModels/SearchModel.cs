using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.ApiModels
{
    public class SearchModel
    {
        public string countries { set; get; }
        public string id { set; get; }
        public string safeNo { set; get; }
        public int page { set; get; }
        public int pageSize { set; get; }
        public string language { set; get; }
        public string regNo { set; get; }
        public string vatNo { set; get; }
        public string name { set; get; }
        public string tradeName { set; get; }
        public string acronym { set; get; }
        public string address { set; get; }
        public string street { set; get; }
        public string houseNo { set; get; }
        public string city { set; get; }
        public string postCode { set; get; }
        public string province { set; get; }
        public string callRef { set; get; }
        public string officeType { set; get; }
        public string phoneNo { set; get; }
        public string status { set; get; }
        public string type { set; get; }
        public string website { set; get; }
        public string customData { set; get; }
        public bool exact { set; get; }
    }
}
