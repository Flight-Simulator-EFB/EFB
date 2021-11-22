using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EFB.Models.JSON;
using Microsoft.Extensions.Logging;
using EFB.Models;
using EFB.Sessions;

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
                ResponseModel response = await request;

                if (response.Error != null)
                {
                    TempData["Error"] = response.Error;
                    TempData["email"] = email;
                    return RedirectToAction("Index", "Home");
                }else{

                    //Type cast required but we know response will be of known type
                    LoginResponse login = (LoginResponse)response.Result;

                    //Generate User Session
                    if (login.error == null)
                    {
                        UserModel user = new UserModel{
                            EMail = email,
                            Token = new TokenModel{
                                TokenValue = login.access_token,
                                Expiration = DateTime.UtcNow.AddSeconds(login.expires_in)
                            }
                        };

                        //Using Session Extensions (Store the user session)
                        HttpContext.Session.SetObject("User", user);
                        return RedirectToAction("Index", "App");
                    }else{
                        TempData["Error"] = login.error_description;
                        TempData["email"] = email;
                        return RedirectToAction("Index", "Home");
                    }

                    
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