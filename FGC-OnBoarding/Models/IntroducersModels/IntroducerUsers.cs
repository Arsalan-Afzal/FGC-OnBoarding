using FGC_OnBoarding.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace FGC_OnBoarding.Models.IntroducersModels
{
    public class IntroducerUsers
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public Introducers Introducers { get; set; }
        public int IntroducerId { get; set; }
        public IntroducerUserRole UserRole { get; set; }
        public int UserRoleId { get; set; }
        public bool IsDelete { get; set; }
    }
}
