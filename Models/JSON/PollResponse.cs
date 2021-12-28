using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EFB.Models.JSON
{
    [JsonArray]
    public class PollResponse
    {
        [JsonProperty(PropertyName = "cmdname")]
        public string Command { get; set; }

        [JsonProperty(PropertyName = "fpl")]
        public string FlightPlan { get; set; }
    }

}