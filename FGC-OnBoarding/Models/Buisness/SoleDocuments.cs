using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class SoleDocuments
    {
        [Key]
        public int Id { get; set; }
        public bool isPop { get; set; }
        public string RelationShip { set; get; }
        public int BuisnessProfileId {get;set;}
    }
}
