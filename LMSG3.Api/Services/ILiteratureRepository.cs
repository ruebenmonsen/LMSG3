using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LMSG3.Api.Services.Repositories
{
    public interface ILiteratureRepository
    {
        Task<IEnumerable<Literature>> GetAsync(bool includeAllInfo); // bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        Task<IEnumerable<LiteratureDto>> FindAsync(LiteraturesResourceParameters literatureResourceParameters);
        Task<Literature> GetAsync(int id, bool includeAllInfo);
        bool CompleteAsync();
        void Update(Literature literature);
        //void Remove(Literature literature);
        Task<bool> AnyAsync(int id);
        void AddLiterature(Literature literature);
        void DeliteLiterature(Literature literature);
        bool LiteratureExist(LiteraturesResourceParameters literatureResourceParameters);
    }
}