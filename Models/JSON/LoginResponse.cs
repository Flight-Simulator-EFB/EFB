using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EFB.Models.JSON
{
    public class LoginResponse
    {
        [JsonProperty]
        public string access_token { get; set; }
        [JsonProperty]
        public int expires_in { get; set; }
        [JsonProperty]
        public string token_type { get; set; }
        [JsonProperty]
        public string scope { get; set; }
        

        [JsonProperty]
        public string error { get; set; } = null;
        [JsonProperty]
        public string error_description { get; set; } = null;
        
        
    }
}