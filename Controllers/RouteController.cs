using System;
using System.Threading.Tasks;
using System.Threading;
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
            if (user == null || user.UserToken.IsExpired())
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

        public async Task<IActionResult> New(string departure, string arrival, string cruise)
        {
            UserModel user = HttpContext.Session.GetObject<UserModel>("User");
            if (!(user == null || user.UserToken.IsExpired()))
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
                        headerData.Add("Authorization", $"Bearer {user.UserToken.TokenValue}");

                        RouteRequest routeRequest = new RouteRequest()
                        {
                            departure = departure,
                            destination = arrival,
                            preferredminlevel = cruiseAlt / 1000,
                            preferredmaxlevel = cruiseAlt / 1000,
                        };
                        StringContent content = new StringContent(JsonConvert.SerializeObject(routeRequest), Encoding.UTF8, "application/json");

                        //Make initial Route Request
                        var requestRoute = API.Post<string>("https://api.autorouter.aero/v1.0/router", headerData, content);

                        ResponseModel<string> responseRoute = await requestRoute;

                        if (responseRoute.Error == null)
                        {//Update User session and add route ID
                            TokenModel routeToken = new TokenModel()
                            {
                                TokenValue = responseRoute.Result.ToString()
                            };

                            user.RouteToken = routeToken;
                            HttpContext.Session.SetObject("User", user);

                            return await Poll(departure, arrival, cruiseAlt);

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


        public async Task<IActionResult> Poll(string departure, string arrival, uint cruise)
        {
            if (HttpContext.Session.GetString("User") != null)
            {//If the user is currently logged in
                UserModel user = HttpContext.Session.GetObject<UserModel>("User");

                if (user.RouteToken != null)
                {//If the user has a route object (e.g, they have been to the route page)

                    //Make calls to the server to fetch route
                    bool collected = false;
                    int pollCount = 0;
                    string routeString = "";

                    APIInterface API = new APIInterface();

                    Dictionary<string, string> headerData = new Dictionary<string, string>();
                    headerData.Add("Authorization", $"Bearer {user.UserToken.TokenValue}");

                    while (collected == false && pollCount < 3)
                    {
                        //Make Polling Request
                        var pollingRequest = API.Put<List<PollResponse>>($"https://api.autorouter.aero/v1.0/router/{user.RouteToken.TokenValue}/longpoll", headerData, null);

                        ResponseModel<List<PollResponse>> responsePoll = await pollingRequest;


                        int routePos = responsePoll.Result.Count - 1;
                        if (responsePoll.Result[routePos].Command == "solution")
                        {
                            collected = true;
                            routeString = responsePoll.Result[routePos].FlightPlan;
                            break;
                        }

                        Thread.Sleep(3000);
                        pollCount++;

                    }

                    if (collected)
                    {
                        //fill in route
                        string finalRoute = RouteModel.ParseRoute(routeString);

                        RouteModel route = RouteModel.StringToRoute(departure, arrival, cruise, finalRoute);
                        user.Route = route;
                        HttpContext.Session.SetObject("User", user);
                        
                        return RedirectToAction("Index", "Route");
                    }

                    TempData["Error"] = $"Unable to get route after {pollCount} Attempts!";
                    return RedirectToAction("Index", "Route");

                }
                else
                {
                    return RedirectToAction("Index", "Route");
                }

            }
            else
            {
                return RedirectToAction("Index", "Route");
            }
        }

        
    }
}