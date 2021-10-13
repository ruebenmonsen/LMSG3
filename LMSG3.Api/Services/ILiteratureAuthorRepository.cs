using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Api.Services.Repositories
{
    public interface ILiteratureAuthorRepository
    {
        void Add(LiteratureAuthor literatureAuthor);
        Task<IEnumerable<LiteratureAuthor>> GetAsync(bool includeAllInfo); // bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        Task<IEnumerable<LiteratureAuthor>> FindAsync(string searchStr, bool includeAllInfo);
        Task<LiteratureAuthor> GetAsync(int id, bool includeAllInfo);
        void Update(LiteratureAuthor literature);
        void Remove(LiteratureAuthor literature);
        Task<bool> AnyAsync(int id);
    }
}
