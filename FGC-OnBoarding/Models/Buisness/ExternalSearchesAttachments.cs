using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class ExternalSearchesAttachments
    {
        [Key]
        public int AttachementsId { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public int? ExternalsearchId { get; set; }
        public string Comments { get; set; }
        public int ApplicationId { set; get; }
        public int? CommentsId { get; set; }

    }
}
