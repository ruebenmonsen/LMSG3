using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Api.ResourceParameters
{
    public class LiteraturesResourceParameters
    {
        public string titleStr { get; set; }
        public string subjectStr { get; set; }
        public string discriptionStr { get; set; }
        public bool includeAllInfo { get; internal set; }
        public bool includeAuthor { get; set; }
        public bool includeSubject { get; set; }
        public bool includeLevel { get; set; }
        public bool includeType { get; set; }
         
    }
}
