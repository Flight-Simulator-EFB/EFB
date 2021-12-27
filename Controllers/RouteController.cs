using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using EFB.Models;
using EFB.Models.JSON;
using EFB.Sessions;
using EFB.Controllers.Form;
using EFB.Controllers.API;

namespace EFB.Controllers
{
    public class RouteController : Controller
    {
        private readonly ILogger<RouteController> _logger;

        public RouteController(ILogger<RouteController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Check the user has a valid login
            UserModel user = HttpContext.Session.GetObject<UserModel>("User");
            if (user == null || user.Route != null || user.Token.IsExpired())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public async  Task<IActionResult> New(string departure, string arrival, string cruise){
            UserModel user = HttpContext.Session.GetObject<UserModel>("User");
            if (!(user == null || user.Token.IsExpired()))
            {//If the user is still authenticated
                if (FormAuthenticator.ValidateICAOCode(departure) && FormAuthenticator.ValidateICAOCode(arrival))
                {//If the user has entered valid ICAOs
                    
                    uint cruiseAlt;

                    if (uint.TryParse(cruise, out cruiseAlt) && FormAuthenticator.ValidateCruiseAlt(cruiseAlt))
                    {//If the cruise altitude if within limits.
                        
                        //Submit route request...
                        APIInterface API = new APIInterface();

                        //Prepare data to be send off with request (route)
                        Dictionary<string, string> headerData = new Dictionary<string, string>();
                        headerData.Add("Authorization", $"Bearer {user.Token.TokenValue}");

                        RouteRequest routeRequest = new RouteRequest(){
                            departure = departure,
                            destination = arrival,
                            preferredminlevel = cruiseAlt / 1000,
                            preferredmaxlevel = cruiseAlt / 1000,
                        };
                        StringContent content = new StringContent(JsonConvert.SerializeObject(routeRequest), Encoding.UTF8, "application/json");
                        
                        //Make initial Route Request
                        var requestRoute = API.Post<string>("https://api.autorouter.aero/v1.0/router", headerData, content);

                        ResponseModel responseRoute = await requestRoute;

                        if (responseRoute.Error == null)
                        {//Update User session and add route ID
                            RouteModel route = new RouteModel(){
                                RouteID = responseRoute.Result.ToString()
                            };

                            user.Route = route;
                            HttpContext.Session.SetObject("User", user);

                        }

                        TempData["Error"] = responseRoute.Error;
                        return RedirectToAction("Index", "Route");

                    }
                    TempData["Error"] = "Invalid Cruise Altitude";
                    TempData["Departure"] = departure;
                    TempData["Arrival"] = arrival;
                    return RedirectToAction("Index", "Route");

                }
                TempData["Error"] = "Invalid Departure or Arrival ICAO";
                return RedirectToAction("Index", "Route");
            }
            return RedirectToAction("Index", "Home");
        }


        public async  Task<IActionResult> Poll(){
            if (HttpContext.Session.GetString("User") != null)
            {//If the user is currently logged in
                UserModel User = HttpContext.Session.GetObject<UserModel>("User");

                if (User.Route != null)
                {//If the user has a route object (e.g, they have been to the route page)
                    
                    //Make calls to the server to fetch route
                    return RedirectToAction("Index", "Route");

                }else{
                    return RedirectToAction("Index", "Route");
                }
                
            }else{
                return RedirectToAction("Index", "Route");
            }
        }
    }
}