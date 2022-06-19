using FGC_OnBoarding.Models.Buisness;
using FGC_OnBoarding.Models.ModelVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models
{
    public class ParentProfile
    {
        public BuisnessProfile objBuisness { get; set; }

        public List<AuthorizedRepresentative> Authorzels { get; set; } = new List<AuthorizedRepresentative>();

        public AuthorizedRepresentative AuthorizedRepresentative { get; set; }

        public DirectorAndShareHolders DirectorAndShareHolders { get; set; }

        public Trustees Trustees { get; set; }

       // public TrusteesVm Trustees { get; set; }

        


        public BuisnessInformation BuisnessInformation { get; set; }

        public FinancialInformation FinancialInformation { get; set; }

        public OwnerShip OwnerShip { get; set; }

        public List<Trustees> Trusteesls { get; set; } = new List<Trustees>();

        public List<DirectorAndShareHolders> DirectorAndShareHoldersls { get; set; } = new List<DirectorAndShareHolders>();

        public List<BuisnessAttachemtns> Buisness1 { get; set; } = new List<BuisnessAttachemtns>();

        public List<BuisnessAttachemtns> Buisness2 { get; set; } = new List<BuisnessAttachemtns>();

        public List<BuisnessAttachemtns> Buisness3 { get; set; } = new List<BuisnessAttachemtns>();


        public List<PersonalDocuments> Personal1 { get; set; } = new List<PersonalDocuments>();

        public List<PersonalDocuments> Personal2 { get; set; } = new List<PersonalDocuments>();

        public List<PersonalDocuments> Personal3 { get; set; } = new List<PersonalDocuments>();


        public string FieldName { set; get; }

    }
}
