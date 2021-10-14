using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface ILiteratureAuthorRepository : IRepository<LiteratureAuthor>
    {
        Task<IEnumerable<LiteratureAuthor>> GetAsync(bool includeAllInfo); // bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        Task<IEnumerable<LiteratureAuthor>> FindAsync(string searchStr);
        Task<LiteratureAuthor> GetAsync(int id, bool includeAllInfo);
        Task<bool> AnyAsync(int id);
    }
}
