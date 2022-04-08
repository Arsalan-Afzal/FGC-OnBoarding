using FGC_OnBoarding.Models.ServiceRequirment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class BuisnessProfile
    {
        [Key]
        public int BuisnessProfileId { get; set; }
        public string BuisnessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string BuisnessWebsite { get; set; }
        public string BuisnessEmail { get; set; }
        public string UTR { get; set; }
        public string CharityNumber { get; set; }
        public string IncorporationNumber { get; set; }
        public int? NoOfDirectors_Partners { get; set; }
        public int? NoOfTrustees { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TradeStartingDate { get; set; } = DateTime.Now;
        public bool RegisteredAdress { get; set; }
        public string RegisteredAdresss { get; set; }
        public string RegisteredCity { get; set; }
        public string RegisteredCounty { get; set; }
        public string RegisteredPostCode { get; set; }
        public string RegisteredCountry { get; set; }
       // public Currency Currency { get; set; }
        public string CurrencyId { get; set; }
        public BuisnessTypes BuisnessTypes { get;set;}
        public int BuisnessTypeId { get; set; }
        public BuisnessSector BuisnessSector { get; set; }
        public int? BuisnessSectorId { get; set; }
        public bool IsComplete { get; set; }
        public string UserId { get; set; }

        public bool IsDelete { get; set; }


        public DateTime SubmitDate { get; set; }

        public bool  IsDiscarded { get; set; }

        public int CurrentForm { get; set; }

        [NotMapped]
        public string FieldName { get; set; }
    }
}
