using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Controllers.FormAuthenticator
{
    public static class FormAuthenticator
    {
        
        public static bool ValidateEMail(string EMail){
            if (EMail.Contains("@") && EMail.Contains(".") && !EMail.Contains(" "))
            {
                if (EMail.Count(x => x == '@') == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ValidateEndpoint(string Endpoint){
            //If it contains http & :// it can be either https or http
            if (Endpoint.Contains("http") && Endpoint.Contains("://") && Endpoint.Length > 7)
            {
                return true;
            }
            return false;
        }

        public static bool ValidateICAOCode(string ICAO){
            if (ICAO.Length == 4)
            {
                //If the value contains a Number, then the value will return false
                return !ICAO.Any(x => char.IsDigit(x));
            }
            return false;
        }

        public static bool ValidateCruiseAlt(int CruiseAlt){
            if (CruiseAlt > 0 && CruiseAlt < 50000)
            {
                return true;
            }
            return false;
        }


    }
}