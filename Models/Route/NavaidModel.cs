using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models.Route
{
    public class NavaidModel:IWaypoint
    {
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int Frequency { get; set; }
        
        public string Airway { get; set; }
        public IWaypoint Next { get; set; } = null;
        public IWaypoint Previous { get; set; } = null;
        public bool Visited { get; set; } = false;
        
        public NavaidModel(string name, string airway, int frequency){
            Name = name;
            Airway = Airway;
            Frequency = frequency;
        }
    }
}