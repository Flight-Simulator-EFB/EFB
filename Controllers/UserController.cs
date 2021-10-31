using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EFB.Models.JSON;
using Microsoft.Extensions.Logging;

namespace EFB.Controllers
{
    //[Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Login(string email, string password){

            if (Form.FormAuthenticator.ValidateEMail(email))
            {
                //API Helper
                API.APIInterface API = new API.APIInterface();

                //Dictionary of Formdata to be encoded
                Dictionary<string, string> formData = new Dictionary<string, string>();

                formData.Add("grant_type", "client_credentials");
                formData.Add("client_id", email);
                formData.Add("client_secret", password);

                HttpContent content = new FormUrlEncodedContent(formData);
                
                var request = API.Post<Models.JSON.LoginResponse>("https://api.autorouter.aero/v1.0/oauth2/token", null, content);

                //Wait for the response to come through
                var response = await request;

                if (response.error != null)
                {

                    TempData["Error"] = response.error_description;
                    return RedirectToAction("Index", "Home");

                }else{
                    //Create a user session and continue
                   return RedirectToAction("Index", "Home");
                }

            }else{
                TempData["Error"] = "Please enter a valid E-Mail";
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