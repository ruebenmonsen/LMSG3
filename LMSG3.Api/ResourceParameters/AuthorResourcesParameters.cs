using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.ResourceParameters
{
    public class AuthorResourcesParameters
    {
      
        public string searchString { get; set; }
        public string ageStr { get; set; }
        public bool includeAllInfo { get; set; }
        public string sortOrder { get; set; }


    }
}
