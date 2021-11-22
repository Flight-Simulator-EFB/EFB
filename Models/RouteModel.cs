using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFB.Models.Route;

namespace EFB.Models
{
    public class RouteModel
    {
        /*
            Route Model - This model contains implementations for different points along the Route

            Route only becomes populated after route is recieved from autorouter API
        */
        public string RouteID { get; init; }
        
        public WaypointModel Departure { get; set; } = null;
        public WaypointModel Arrival { get; set; } = null;
        public IWaypoint Current { get; set; } = null;
        public uint Cruise { get; set; } = 0;
        
    }
}