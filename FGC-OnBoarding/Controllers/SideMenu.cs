using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FGC_OnBoarding.Controllers
{
    public class SideMenu : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SideMenu1()
        {

            return View();
        }
    }
}
