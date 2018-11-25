using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webApp.Models;
using webApp.Services;

namespace webApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
//            var runner = new Doer();
//            var repo = await runner.CreateAndRewind();
//            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Go(string login, string pwd)
        {
            var runner = new Doer();
            var repo = await runner.CreateAndRewind(login, pwd);
            ViewData["Message"] = "ТЕПЕРЬ СЫН МАМИНОЙ ПОДРУГИ - ЭТО ТЫ!!!";

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}