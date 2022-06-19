using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class ExternalAttachmentComments
    {
        [Key]
        public int Id { get; set; }
        public string Comments  { get; set; }
        public int ExternalSearchId { get; set; }
        public int BuisnessProfileID { get; set; }
        public int AttachementsId { get; set; }
        public string ActionBy { get; set; }
        public DateTime Date { get; set; }

        public bool Isdelete { get; set; }
    }
}
