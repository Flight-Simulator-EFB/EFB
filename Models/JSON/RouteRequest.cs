using Newtonsoft.Json;

namespace EFB.Models.JSON
{
    public class RouteRequest
    {
        [JsonProperty]
        public string departure { get; set; }
        [JsonProperty]
        public string destination { get; set; }
        [JsonProperty]
        public uint preferredminlevel { get; set; }
        [JsonProperty]
        public uint preferredmaxlevel { get; set; }
        [JsonProperty]
        public string optimise { get; } = "preferred";
    }
}