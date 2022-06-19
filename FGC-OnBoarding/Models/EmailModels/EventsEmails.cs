using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.EmailModels
{
    public class EventsEmails
    {
        [Key]
        public int EventsEmailsId { get; set; }
        public string  Email { get; set; }
        public string Username  { get; set; }

        public EmailEvents EmailEvents { set; get; }
        public int EmailEventsId { get; set; }
    }
}
