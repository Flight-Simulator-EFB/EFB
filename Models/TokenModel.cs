using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models
{
    public class TokenModel
    {
        /*
            Auto Router API Token Model
        */
        public string Token { get; init; }
        public DateTime Expiration { get; init; }

        public bool IsExpired(){
            //Check if the current time is beyond expiration
            if (DateTime.UtcNow > Expiration)
            {
                return true;
            }
            return false;
        }
        
    }
}