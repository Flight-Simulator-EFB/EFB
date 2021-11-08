using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models
{
    public class UserModel
    {
        /*
            User - Contains relevant user information as well as current session data such
                
                #API Token
                #Route
                #Last Recorded Sim Position

                This is the model that is implemented into MongoDB
        */
        public object Id { get; init; }
        public string EMail { get; init; }
        public TokenModel Token { get; set; } = null;
        
        //Contains the most recent route generated by the user through the App
        public object Route { get; set; } = null;

        //Contains the most recently stored position of the user in the simulator
        public object SimPosition { get; set; } = null;
        
    }
}