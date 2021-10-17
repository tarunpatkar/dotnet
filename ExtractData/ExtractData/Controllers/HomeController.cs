using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExtractData.Models;

namespace ExtractData.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Extract()
        {
            DBService dbs = new DBService();
            dbs.TransferData();
            TempData["Message"] = "Records Inserted";
            //ViewBag.Message = "Records Inserted";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RemoveRecords()
        {
            DBService dbs = new DBService();
            dbs.DeleteRecords();
            TempData["Message"] = "Records Removed";
            

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
