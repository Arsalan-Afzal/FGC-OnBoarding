using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Helpers
{
    public static class OnboardingExtentions
    {
        private static Dictionary<string, string> contentTypes;
        public static DateTime DateTime_UK(this DateTime serverDate)
        {
            var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var UKDateTime = TimeZoneInfo.ConvertTime(serverDate, TimeZoneInfo.Local, britishZone);
            return UKDateTime;
        }
    }
}
