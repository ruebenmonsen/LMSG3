using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class LiteratureAuthorRepository : GenericRepository<LiteratureAuthor>, ILiteratureAuthorRepository
    {
        public LiteratureAuthorRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<LiteratureAuthor>> GetAsync(bool includeAllInfo)
        {
            var authors = await context.LiteratureAuthors.ToListAsync();
            return includeAllInfo ? await context.LiteratureAuthors  // await _context.LiteratureAuthors.ToListAsync()
               .Include(e => e.Literatures)
               .ToListAsync() :
               await context.LiteratureAuthors
               .ToListAsync();
        }

        public async Task<LiteratureAuthor> GetAsync(int id, bool includeAllInfo)
        {
            var author =   context.LiteratureAuthors.AsQueryable();
            if (includeAllInfo)
            {
                author = author.Include(a => a.Literatures);
            }


            return await author.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<LiteratureAuthor>> FindAsync(string searchStr)
        {
            var author = await context.LiteratureAuthors.AsQueryable().ToListAsync();
           
            return author.Where(a => a.FirstName.ToLower().Contains(searchStr)).ToList();
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
