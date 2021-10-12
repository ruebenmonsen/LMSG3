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
    public class LiteratureAuthorRepository : ILiteratureAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;

        public LiteratureAuthorRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<IEnumerable<LiteratureAuthor>> GetAsync(bool includeAllInfo)
        {
            var authors = await _context.LiteratureAuthors.ToListAsync();
            return includeAllInfo ? await _context.LiteratureAuthors  // await _context.LiteratureAuthors.ToListAsync()
               .Include(e => e.Literatures)
               .ToListAsync() :
               await _context.LiteratureAuthors
               .ToListAsync();
        }

        public async Task<LiteratureAuthor> GetAsync(int id, bool includeAllInfo)
        {
            var author =   _context.LiteratureAuthors.AsQueryable();
            if (includeAllInfo)
            {
                author = author.Include(a => a.Literatures);
            }


            return await author.FirstOrDefaultAsync(a => a.Id == id);

        }

        public async Task<IEnumerable<LiteratureAuthor>> FindAsync(string searchStr, bool includeAllInfo)
        {
            var author = await _context.LiteratureAuthors.AsQueryable().ToListAsync();
            if (includeAllInfo)
            {
                //author = author.Include(a => a.Literatures);
            }

            return  author.Where(a => a.FullName.ToLower().Contains(searchStr));
        }
        public void Add(LiteratureAuthor literatureAuthor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

       

       

        public void Remove(LiteratureAuthor literature)
        {
            throw new NotImplementedException();
        }

        public void Update(LiteratureAuthor literature)
        {
            throw new NotImplementedException();
        }

       
    }
}
