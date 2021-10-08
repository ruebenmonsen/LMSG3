﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Entities
{
    public class User : IdentityUser
    {
        
        public string FName { get; set; }
        public string LName { get; set; }
        public ICollection<Document> Documents  { get; set; }

    }
}
