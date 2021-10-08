using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Entities
{
    public class Teacher : User
    {
       public ICollection<Module> Modules { get; set; }
    }
}
