using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFB.Models.Route;
using EFB.Controllers.Form;
using System.Net.Http;

namespace EFB.Models
{
    public class RouteModel
    {
        /*
            Route Model - This model contains implementations for different points along the Route

            Route only becomes populated after route is recieved from autorouter API
        */
        public WaypointModel Departure { get; set; } = null;
        public WaypointModel Arrival { get; set; } = null;
        public IWaypoint Current { get; set; } = null;
        public uint Cruise { get; set; } = 0;
        public RouteModel(string departure, string departureRoute, string arrival, string arrivalRoute, uint cruise){
            if (FormAuthenticator.ValidateICAOCode(departure))
            {
                Departure = new WaypointModel(departure, departureRoute);
            }

            if (FormAuthenticator.ValidateICAOCode(arrival))
            {
                Arrival = new WaypointModel(arrival, arrivalRoute);
            }

            if (FormAuthenticator.ValidateCruiseAlt(cruise))
            {
                Cruise = cruise;
            }
        }

        //Generate a route Object
        public static RouteModel StringToRoute(string departure, string arrival, uint cruise, string routeString){
            string[] routeTemp = routeString.Split(" ");

            //Set departure and arrival route
            string departureRoute = routeTemp[0];
            string arrivalRoute = routeTemp[routeTemp.Length - 2];

            RouteModel route = new RouteModel(departure, departureRoute, arrival, arrivalRoute, cruise);
            route.Departure.Airway = routeTemp[0];

            route.Current = route.Departure;

            for (var i = 1; i < routeTemp.Length-1; i+=2)
            {//Already used first item, continue itterating over every other item
                IWaypoint next;

                //Populate 'next' waypoint
                if (routeTemp[i].Length > 3)
                {//waypoint Type
                    next = new WaypointModel(routeTemp[i], routeTemp[i+1]); 
                }else
                {//Navaid Type
                    next = new NavaidModel(routeTemp[i], routeTemp[i+1]); 
                }

                next.Previous = route.Current;
                route.Current.Next = next;
                route.Current = next;
            }
            
            //Connect end of route (linked list)
            route.Current.Airway = null;
            route.Current.Next = route.Arrival;
            route.Arrival.Previous = route.Current;

            route.Current = null;

            return route;
        }



        //Generate a route String
        public static string ParseRoute(string route){
            route.Replace('/', ' ');
            var routeArr = route.Split(' ');

            string finalRoute = "";

            foreach (var item in routeArr)
            {
                var waypoint = item.Split('/')[0];
                if (waypoint.Length <= 7 && waypoint.Length >= 3 && !waypoint.Contains('-'))
                {
                    finalRoute += $"{waypoint} ";

                    if (waypoint.Length == 7 && finalRoute.Length > 8)
                        break;
                    
                }
            }

            return finalRoute;
        }
        
    }

}