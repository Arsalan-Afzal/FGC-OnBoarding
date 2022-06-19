using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class PersonalDocuments
    {
        [Key]
        public int DocumentId { get; set; }
        public string Filename { get; set; }
        public string DisplayFilename { get; set; }
        public int BuisnessProfileId { get; set; }
        public int BuisnessTypeId { get; set; }
        public string DocumentType { get; set; }
        public bool ISpep { get; set; }
        public string RelationShip { set; get; }
        public int DocumentTypeId {get;set;}

        public bool IsDelete { get; set; }
    }
}
