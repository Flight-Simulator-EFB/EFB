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


    }
}