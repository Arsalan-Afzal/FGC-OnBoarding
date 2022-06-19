using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class OwnerShip
    {
        [Key]
        public int OwnerShipID { get; set; }

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

        [NotMapped]
        public string Mydob { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public BuisnessProfile BuisnessProfile { get; set; }

        public int BuisnessProfileId { get; set; }

        public bool Isdefault { get; set; }
        [NotMapped]
        public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }

        public bool IsDelete { get; set; }
    }
}
