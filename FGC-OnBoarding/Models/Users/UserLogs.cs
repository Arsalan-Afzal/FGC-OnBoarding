using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Users
{
    public class UserLogs
    {
        [Key]
        public int LogId { get; set; }
        public string Username { get; set; }
        public string CountryName { get; set; }
        public string Email { get; set; }
        public string IPAdress { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime LogDate { get; set; }
        public string LogDateStr { get; set; }
        public string Action { get; set; }

        public bool IsDelete { get; set; }
    }
}
