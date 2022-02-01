using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EFB.Models;
using EFB.Sessions;

namespace EFB.Controllers
{
    //[Route("[controller]")]
    public class NavdataController : Controller
    {
        private readonly ILogger<NavdataController> _logger;

        public NavdataController(ILogger<NavdataController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string identifier)
        {
            if (identifier == null)
            {//In the event we are just going to the base page1
                return View();
            }

            NavdataModel[] data = null;

            if (HttpContext.Session.GetObject<NavdataModel[]>("Navdata") == null)
            {//If the navdata needs re-caching
                data = await NavdataModel.Populate();
                HttpContext.Session.SetObject("Navdata", NavdataModel.MergeSort(ref data, 0, data.Length-1));
            }

            //get the data out of tempdata and cast it into an array
            data = HttpContext.Session.GetObject<NavdataModel[]>("Navdata");
            NavdataModel navaid = NavdataModel.BinarySearch(ref data, 0, data.Length-1, identifier);

            if (navaid == null)
            {
                TempData["Error"] = $"Sorry, no Navaid found with the name {identifier}";
            }
            return View(navaid);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}