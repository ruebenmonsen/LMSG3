using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Entities
{
    public class ModuleTeacher
    {
        public string TeacherId { get; set; }

        public int? ModuleId { get; set; }

        public Teacher Teacher { get; set; }

        public Module Module { get; set; }
    }
}
