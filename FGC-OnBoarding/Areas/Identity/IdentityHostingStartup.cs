using System;
using FGC_OnBoarding.Areas.Identity.Data;
using FGC_OnBoarding.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FGC_OnBoarding.Areas.Identity.IdentityHostingStartup))]
namespace FGC_OnBoarding.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FGC_OnBoardingContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FGC_OnBoardingContextConnection")));

                services.AddDefaultIdentity<FGC_OnBoardingUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FGC_OnBoardingContext>();
            });
        }
    }
}