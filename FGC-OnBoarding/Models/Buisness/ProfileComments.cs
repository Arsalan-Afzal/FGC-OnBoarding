using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class ProfileComments
    {
        [Key]
        public int Id { get; set; }

        public string ActionBy { get; set; }


        public string Comments { get; set; }

        public int ActorsId {get;set;}
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? ActionDate { get; set; }

        public int BuisneesProfileId { get; set; }

        public bool IsDelete { get; set; }

        [NotMapped]
        public string ActionDateStr { get; set; }

    }
}
