using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.IntroducersModels
{
    public class Introducers
    {
        [Key]
        public int IntroducerId { get; set; }

        public string IntroducerName { get; set; }

        public bool IsDelete { get; set; }
    }
}
