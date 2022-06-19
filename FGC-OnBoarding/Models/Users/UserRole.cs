using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Users
{
    public class IntroducerUserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        public string RoleName { get; set; }
        public bool IsDelete { get; set; }
    }
}
