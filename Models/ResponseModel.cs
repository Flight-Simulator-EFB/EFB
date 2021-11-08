using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models
{
    public class ResponseModel
    {
        //Object should be of known type from sender
        public object Result { get; set; } = null;
        
        public string Error { get; set; } = null;
        
    }
}