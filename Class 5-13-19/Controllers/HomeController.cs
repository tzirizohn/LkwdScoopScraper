using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;  
using Class_5_13_19.Models;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Mvc;
using LakewoodScoop.Data;

namespace Class_5_13_19.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            LkwdScoop ls = new LkwdScoop();
            IEnumerable<LakewoodScoopPost> posts = ls.ScrapeLS();
            return View(posts);
        }
       
    }
}
