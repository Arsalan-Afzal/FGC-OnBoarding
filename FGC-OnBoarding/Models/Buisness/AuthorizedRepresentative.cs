using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class AuthorizedRepresentative
    {
        [Key]
        public int RepresentativeId { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string County { get; set; }

        public string Country { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? DOB { get; set; } 

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string PositionInBuisness { get; set; }

        public string RoleIncharity { get; set; }

        public string PositionInComany { get; set; }

        public BuisnessProfile BuisnessProfile { get; set; }

        public int BuisnessProfileId { get; set; }

        public bool Isdefault { get; set; }

        [NotMapped]
        public int Formid { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime MYdob { get; set; }

        [NotMapped]
        public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }


        [NotMapped]
        public string DateOfBirth { get; set; }



        public bool IsDelete { get; set; }
    }
}
