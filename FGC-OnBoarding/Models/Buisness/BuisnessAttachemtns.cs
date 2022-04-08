using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class BuisnessAttachemtns
    {
        [Key]
        public int DocumentId { get; set; }
        public string Filename { get; set; }
        public string DisplayFilename { get; set; }
        public int BuisnessProfileId { get; set; }
        public int BuisnessTypeId { get; set; }
        public string DocumentType { get; set; }
    }
}
