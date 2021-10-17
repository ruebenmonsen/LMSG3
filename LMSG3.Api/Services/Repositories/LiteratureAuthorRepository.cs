using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSG3.Api.Services.Repositories;
using LMSG3.Api.ResourceParameters;

namespace LMSG3.Api.Repositories
{
    public class LiteratureAuthorRepository : ILiteratureAuthorRepository
    {
        private readonly ApiDbContext _context;
        private readonly ILogger logger;

        public LiteratureAuthorRepository(ApiDbContext context, ILogger logger)
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
            var author = _context.LiteratureAuthors.AsQueryable();
            if (includeAllInfo)
            {
                author = author.Include(a => a.Literatures);
            }


            return await author.FirstOrDefaultAsync(a => a.Id == id);

        }


        public async Task<IEnumerable<LiteratureAuthor>> FindAsync(ResourceParameters.AuthorResourceParameters authorResourceParameters)
        {
            var author = _context.LiteratureAuthors.AsQueryable();



            if (authorResourceParameters == null)
            {
                return await author.ToListAsync();
            }

            if (!string.IsNullOrWhiteSpace(authorResourceParameters.nameStr))
            {
                var searchParam = authorResourceParameters.nameStr.ToLower();
                //return author.Where(a => a.FullName.ToLower().Contains(authorResourceParameters.nameStr.ToLower()));  // Todo: Trim()

                if (authorResourceParameters.includeAllInfo)
                {
                    await author.Where(a => a.FirstName.Contains(searchParam) || a.LastName.Contains(searchParam)).ToListAsync();
                    author = author.Include(a => a.Literatures);
                }
                return await author.Where(a => a.FirstName.Contains(searchParam) || a.LastName.Contains(searchParam)).ToListAsync(); ;

            }


            return await author.ToListAsync();

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