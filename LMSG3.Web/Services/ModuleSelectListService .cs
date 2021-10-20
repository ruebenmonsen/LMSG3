using LMSG3.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public class ModuleSelectListService : IModuleSelectListService
    {

        private readonly ApplicationDbContext db;

        public ModuleSelectListService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetModuleAsync()
        {
            return await db.Modules
                       .Distinct()
                       .Select(c => new SelectListItem
                       {
                           Text = c.Name,
                           Value = c.Id.ToString()
                       })
                       .ToListAsync();
        }
    }
}

