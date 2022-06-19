using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FGC_OnBoarding.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FGC_OnBoardingUser class
    public class FGC_OnBoardingUser : IdentityUser
    {
    
        public string Name { get; set; }
        public string BuisnessName { get; set; }


    }
}
