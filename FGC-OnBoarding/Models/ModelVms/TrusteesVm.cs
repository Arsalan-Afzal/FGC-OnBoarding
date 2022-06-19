using FGC_OnBoarding.Models.Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.ModelVms
{
    public class TrusteesVm
    {
        public int TrusteeId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostCode { get; set; }
        public string County { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? DOB { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? AppointmentDate { get; set; }
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

        [NotMapped]
        public string DateofAppointment { get; set; }

        public bool IsDelete { get; set; }
    }
}
