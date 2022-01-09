using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFB.Models
{
    public class ResponseModel<T>
    {
        //Object should be of known type from sender
        public T Result { get; set; } = default(T);
        public string Error { get; set; } = null;
        
    }
}