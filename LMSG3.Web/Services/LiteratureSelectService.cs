using LMSG3.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public class LiteratureSelectService : ILiteratureSelectService
    {
        private readonly ApiDbContext context;

        public LiteratureSelectService(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetLevelAsync()
        {
            return await context.literatureLevels.OrderBy(l => l.Id)
                     
                      .Select(g => new SelectListItem
                      {
                          Text = g.Name.ToString(),
                          Value = g.Id.ToString()
                      })
                      .ToListAsync();
        }
        
        public async Task<IEnumerable<SelectListItem>> GetLteraturesTypeAsync()
        {
            return await context.literatureTypes.OrderBy(t => t.Name)
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name.ToString(),
                        Value = r.Id.ToString()
                    }).ToListAsync();
        }
    }
}
