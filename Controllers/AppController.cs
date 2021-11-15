using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EFB.Models;
using Microsoft.AspNetCore.Http;
using EFB.Sessions;

namespace EFB.Controllers
{
    public class AppController : Controller
    {
        private readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Check to see what point on the application the user is at and where they should be sent
            UserModel User = HttpContext.Session.GetObject<UserModel>("User");
            if (User != null)
            {
                if (User.Route == null)
                {
                    return RedirectToAction("Index", "Route");
                }else{
                    return RedirectToAction("Index", "App");
                }
            }else{
                return RedirectToAction("Index", "Home");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}