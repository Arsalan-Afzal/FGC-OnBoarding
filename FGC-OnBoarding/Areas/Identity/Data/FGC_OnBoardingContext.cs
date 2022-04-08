using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FGC_OnBoarding.Areas.Identity.Data;
using FGC_OnBoarding.Models.Buisness;
using FGC_OnBoarding.Models.ServiceRequirment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FGC_OnBoarding.Data
{
    public class FGC_OnBoardingContext : IdentityDbContext<FGC_OnBoardingUser>
    {
        public FGC_OnBoardingContext(DbContextOptions<FGC_OnBoardingContext> options)
        : base(options){}
        public DbSet<AuthorizedRepresentative> AuthorizedRepresentative { get; set; }
        public DbSet<BuisnessInformation> BuisnessInformation { get; set; }
        public DbSet<BuisnessProfile> BuisnessProfile { get; set; }
        public DbSet<BuisnessAttachemtns> BuisnessAttachemtns { get; set; }
        public DbSet<DirectorAndShareHolders> DirectorAndShareHolders { get; set; }
        public DbSet<FinancialInformation> FinancialInformation { get; set; }
        public DbSet<OwnerShip> OwnerShip { get; set; }
        public DbSet<Trustees> Trustees { get; set; }
        public DbSet<BuisnessSector> BuisnessSector { get; set; }
        public DbSet<BuisnessTypes> BuisnessTypes { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<SoleDocuments> SoleDocuments { get; set; }
        public DbSet<BuisnessDocuments> BuisnessDocuments { get; set; }
        public DbSet<CharityDocument> CharityDocument { get; set; }
        public DbSet<PersonalDocuments> PersonalDocuments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dbo");
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);



        }
    }
}
