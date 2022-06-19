using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class FinancialInformation
    {
        [Key]
        public int FIId { get; set; }

        public decimal? PerMonth { get; set; }

        public decimal? PerAnum { get; set; }

        public decimal? PaymentIncoming { get; set; }

        public decimal? PaymentOutgoing { get; set; }

        public decimal? TransactionIncoming { get; set; }

        public decimal? TransactionOutgoing { get; set; }

        public decimal? NoOfPaymentsPerMonth { get; set; }

        public decimal? VolumePermonth { get; set; }

        public bool AccountDetails { get; set; }

        public string BankName { get; set; }

        public string BankAddress { get; set; }

        public string SortCode { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string IBAN { get; set; }

        public string SwiftCode { get; set; }

        public string AccountCurrency { get; set; }

        public BuisnessProfile BuisnessProfile { get; set; }

        public int BuisnessProfileId { get; set; }

        [NotMapped]
        public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }

    }
}
