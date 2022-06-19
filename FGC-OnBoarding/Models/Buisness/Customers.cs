using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.Buisness
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string Ip { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }

        public string PasswordString { get; set; }
        public virtual string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public virtual string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string BuisnessName { get; set; }
        public string UserId { get; set; }

    }
}
