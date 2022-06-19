using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class DirectorAndShareHolders
    {
        [Key]
        public int DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? DOB { get; set; } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal? ShareHolders_percentage {get;set;}
        public BuisnessProfile BuisnessProfile { get; set; }
        public int BuisnessProfileId { get; set; }
        public bool Isdefault { get; set; }
        [NotMapped]
        public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }
        [NotMapped]
        public int Formid { get; set; }

        [NotMapped]
        public string Dateofbirth { get; set; }

        public bool IsDelete { get; set; }


    }
}
