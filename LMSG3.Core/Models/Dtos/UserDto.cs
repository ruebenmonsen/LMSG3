using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Dtos
{
    public class UserDto : IdentityUser

    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return FName + " " + LName; } }


        public ICollection<Entities.Document> Documents { get; set; }

        public string Role { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }

    }
}
