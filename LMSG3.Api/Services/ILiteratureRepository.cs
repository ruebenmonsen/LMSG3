using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LMSG3.Api.Services.Repositories
{
    public interface ILiteratureRepository 
    {
        void Add(Literature literature);
        Task<IEnumerable<Literature>> GetAsync(bool includeAllInfo); // bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        Task<IEnumerable<Literature>> FindAsync(LiteraturesResourceParameters literatureResourceParameters);
        Task<Literature> GetAsync(int id, bool includeAllInfo);
        bool Save();
        void Update(Literature literature);
        void Remove(Literature literature);
        Task<bool> AnyAsync(int id);
        
    }
}
