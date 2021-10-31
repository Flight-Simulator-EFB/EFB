using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EFB.Models.JSON
{
    public class Login
    {
        [JsonProperty]
        public string grant_type { get; set; }
        [JsonProperty]
        public string client_id { get; set; }
        [JsonProperty]
        public string client_secret { get; set; }
        
    }
}