using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Users
{
    public class CustomerLogs
    {
        [Key]
        public int CustomerLogId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string IPAdress { get; set; }
        public DateTime? LoginTime { get; set; }
        public string LoginTimeStr { get; set; }
        public string OldValue { get; set; }
        public string FormName { get; set; }

        public string Remarks { get; set; }

        public string Action { get; set; }
        public string FieldName { get; set; }
        public string CountryName { get; set; }
        public string  NewValue { get; set; }
        public DateTime? LogOutTime { get; set; }

        public DateTime? ActionTime { get; set; }
        public string LogOutTimeStr { get; set; }
        public string Activity { get; set; }
        public string CustomerId { get; set; }
        public bool IsDelete { get; set; }
    }
}
