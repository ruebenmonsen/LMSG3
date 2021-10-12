using LMSG3.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public class UserRoleSelectListService : IUserRoleSelectListService
    {
        private readonly ApplicationDbContext db;

        public UserRoleSelectListService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SelectListItem> GetUserRole()
        {
            return new List<SelectListItem>{
                                              new SelectListItem{ Text="Teacher", Value = "Teacher" },
                                              new SelectListItem{ Text="Student", Value = "Student" } };
        }
    }
}
