using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Api.ResourceParameters
{
    public class LiteraturesResourceParameters
    {
        public string searchString { get; set; }
       
        [Range(0, 4, ErrorMessage = "Can only be between 0 .. 4")]
        public int levelFilter { get; set; }
        public string sortOrder { get; set; }
        public bool includeAllInfo { get; set; }
        public bool includeAuthor { get; set; }
               
    }
}
