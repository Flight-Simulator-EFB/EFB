using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models.Route
{
    public interface IWaypoint
    {
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        
        public string Airway { get; set; }
        public IWaypoint Next { get; set; }
        public IWaypoint Previous { get; set; }
        public bool Visited { get; set; }
        
    }
}