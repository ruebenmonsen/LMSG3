using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMSG3.Data;
using LMSG3.Core.Models.Entities;
using LMSG3.Api.ResourceParameters;

namespace LMSG3.Api.Services.Repositories
{
    public class LitertureRepository : ILiteratureRepository
    {
        private readonly ApiDbContext _context;

        public LitertureRepository(ApiDbContext context, ILogger logger)
        {
            _context = context;
        }

        public async Task<IEnumerable<Literature>> GetAsync(bool includeAllInfo) //bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        {

            return includeAllInfo ? await _context.Literatures
                .Include(e => e.Authors)
              //  .Include(e => e.Subject)
               // .Include(e => e.LiteratureType)
                //.Include(e => e.LiteratureLevel)
                .ToListAsync() :
                await _context.Literatures
                .ToListAsync();

        }

        public async Task<Literature> GetAsync(int id, bool includeAllInfo)
        {

            var literature = _context.Literatures.AsQueryable();
            var literLevel = _context.literatureLevels.AsQueryable();

            if (includeAllInfo)
            {
                literature = literature.Include(e => e.Authors);

                 //.Include(a => a.li)
                 //.Where(t => t.LiteraLevelId == literLevel.i).ToList();
                 //.Include(e => e.LiteratureType)
                 //.Include(e => e.LiteratureLevel);

              
            }

            

            return await literature.FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<IEnumerable<Literature>> FindAsync(LiteraturesResourceParameters literaturesResourceParameters)
        {
            var literature = _context.Literatures.AsQueryable();
            if (literaturesResourceParameters == null)
            {
                return await literature.ToListAsync();
            }

            if (!string.IsNullOrWhiteSpace(literaturesResourceParameters.titleStr))
            {
                //return await _context.Literatures.AsQueryable().ToListAsync();
                // var literature = GetLiteratures(literaturesResourceParameters);
                literature = literature.Where(l => l.Title.ToLower().Contains(literaturesResourceParameters.titleStr.ToLower()));
            }

            if (literaturesResourceParameters.includeAllInfo)
            {
                literature = literature.Include(e => e.Authors);
                            //.Include(e => e.Subject);
                           // .Include(e => e.LiteratureType)
                            //.Include(e => e.LiteratureLevel);
            }

            if (!string.IsNullOrWhiteSpace(literaturesResourceParameters.subjectStr))
            {
                //literature = literature.Where(l => l.Subject.Name.Contains(literaturesResourceParameters.subjectStr.ToLower())).Include(e => e.Subject);
            }

            return await literature.ToListAsync();


        }


        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void AddLiterature(Literature literature)
        {
            _context.Literatures.Add(literature);


        }


        public void DeliteLiterature(Literature literature)
        {
            if (literature == null)
            {
                throw new ArgumentNullException(nameof(literature));
            }
            _context.Literatures.Remove(literature);
        }

        public void Update(Literature literature)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Remove(Literature literature)
        {
            throw new NotImplementedException();
        }
    }
}