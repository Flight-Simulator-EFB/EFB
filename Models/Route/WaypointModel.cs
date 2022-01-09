using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models.Route
{
    public class WaypointModel:IWaypoint
    {
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        
        public string Airway { get; set; }
        public IWaypoint Next { get; set; } = null;
        public IWaypoint Previous { get; set; } = null;
        public bool Visited { get; set; }

        public WaypointModel(string name, string airway){
            Name = name;
            Airway = airway;
        }
        
    }
}