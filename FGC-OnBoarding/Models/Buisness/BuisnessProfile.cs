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
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? RegistrationDate { get; set; } 
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? TradeStartingDate { get; set; } 
        public bool RegisteredAdress { get; set; }
        public string RegisteredAdresss { get; set; }
        public string RegisteredCity { get; set; }
        public string RegisteredCounty { get; set; }
        public string RegisteredPostCode { get; set; }
        public string RegisteredCountry { get; set; }
       // public Currency Currency { get; set; }
        public string CurrencyId { get; set; }
        public virtual BuisnessTypes BuisnessTypes { get;set;}
        public int BuisnessTypeId { get; set; }
        public  BuisnessSector BuisnessSector { get; set; }
        public int? BuisnessSectorId { get; set; }
        public bool IsComplete { get; set; }
        public string UserId { get; set; }
        public bool Ispep { get; set; }
        public string Peprelationship { get; set; }
        public string SendBackReason { get; set; }
        public bool IsDelete { get; set; }
        public bool IsClient { get; set; }
        public DateTime? ClientDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public string DeclinedReason { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool  IsDiscarded { get; set; }
        public bool IsApproved { get; set; }
        public string CurrentStatus { get; set; } 
        public bool IsOnboarded { get; set; }
        public bool IsMlro { get; set; }
        public bool IsCompliance { get; set; }
        public bool IsAccount { get; set; }
        public bool IsComplianceCommitee { get; set; }
        public bool OfferLetter { get; set; }
        public bool SignOfferLetter { get; set; }
        public bool OnboardingFee { get; set; }
        public bool AutomatedKycsent { get; set; }
        public bool AutomatedKycrecieved { get; set; }
        public bool IsDeclined { get; set; }
        public string RiskIndicator { get; set; }
        public decimal? ClientRiskScore { get; set; }
        public int CurrentForm { get; set; }
        [NotMapped]
        public string BuisnessTypeName { get; set; }
        [NotMapped]
        public string FieldName { get; set; }
        [NotMapped]
        public string FieldValue { get; set; }
        [NotMapped]
        public int ActionCount { get; set; }
    }
}
