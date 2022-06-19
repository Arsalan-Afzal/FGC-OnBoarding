using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }

        public string CountryName { get; set; }
    }
}
