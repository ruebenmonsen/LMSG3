using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMSG3.Core.Repositories;

namespace LMSG3.Data.Repositories
{
    public class LiteratureRepository : GenericRepository<Literature>
    {
        public LiteratureRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Literature>> GetAsync(bool includeAllInfo) //bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        {

            return includeAllInfo ? await context.Literatures
                .Include(e => e.Authors)
                .Include(e => e.Subject)
                .Include(e => e.LiteratureType)
                .Include(e => e.LiteratureLevel)
                .ToListAsync() :
                await context.Literatures
                .ToListAsync();
        }

        public async Task<Literature> GetAsync(int id, bool includeAllInfo)
        {

            var literature = context.Literatures.AsQueryable();

            if (includeAllInfo)
            {
                literature = literature.Include(e => e.Authors)
                 .Include(e => e.Subject)
                 .Include(e => e.LiteratureType)
                 .Include(e => e.LiteratureLevel);
            }


            return await literature.FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<Literature> FindAsync(string searchStr)
        {
            var literature = context.Literatures.AsQueryable();
            
            return await context.Literatures.FirstOrDefaultAsync(e => e.Title.Contains(searchStr));


        }
    }
}
