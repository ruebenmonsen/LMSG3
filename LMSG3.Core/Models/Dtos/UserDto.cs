using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Dtos
{
    public class UserDto
    {
            public IList<ApplicationUser> users { get; set; }
            public IList<string> roles { get; set; }

    }
}
