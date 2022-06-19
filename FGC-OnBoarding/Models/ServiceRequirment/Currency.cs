using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.ServiceRequirment
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Name { get; set; }

        public bool IsDelete { get; set; }
    }
}
