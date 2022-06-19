using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class ExternalSearches
    {
        [Key]
        public int ExternalsearchId { get; set; }

        public string Name { get; set; }

    }
}
