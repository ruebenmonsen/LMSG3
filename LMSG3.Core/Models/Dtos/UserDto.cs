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
    public class UserDto : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string FullName { get; set; }
        public ICollection<Entities.Document> Documents { get; set; }
        public string Role { get; set; }

    }
}
