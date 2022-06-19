using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class BuisnessInformation
    {
        [Key]
        public int BuisnessInformationId { get; set; }

        public string  Answer1 { get; set; }

        public string Answer2 { get; set; }
        public string Answer3 { get; set; }

        public string Answer4 { get; set; }

        public string Answer5 { get; set; }

        public string Answer6 { get; set; }

        public string Answer7 { get; set; }

        public string Answer8 { get; set; }

        public string Answer9 { get; set; }

        public BuisnessProfile BuisnessProfile { get; set; }

        public int BuisnessProfileId { get; set; }

        public int BuisnessTypeId { get; set; }

        [NotMapped]
        public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }

        [NotMapped]
        public string Question { get; set; }

        public bool IsDelete { get; set; }

    }
}
