using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.IntroducersModels
{
    public class IntroducersUsersApplications
    {
        [Key]
        public int IntroducersApplicationId { get; set; }
        public int IntroducerUserId {get;set;}


        public int ApplicationId { get; set; }

    }
}
