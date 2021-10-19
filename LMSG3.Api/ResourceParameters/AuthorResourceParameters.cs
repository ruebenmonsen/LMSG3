using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.ResourceParameters
{
    public class AuthorResourceParameters
    {
        public string nameStr { get; set; }
        public string titleStr { get; set; }
        public string ageStr { get; set; }
        public bool includeAllInfo { get; set; }
        
    }
}
