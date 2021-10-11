using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.Repositories
{
    public interface ILiteratureRepository 
    {
        void Add(Literature literature);
        Task<IEnumerable<Literature>> GetAsync(bool includeAllInfo); // bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        Task<Literature> FindAsync(string searchStr);
        Task<Literature> GetAsync(int id, bool includeAllInfo);
        void Update(Literature literature);
        void Remove(Literature literature);
        Task<bool> AnyAsync(int id);
       
    }
}
