using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Dtos;
using AutoMapper;
using LMSG3.Api.Services;

namespace LMSG3.Api.Service.Repositories
{
    public class LiteratureAuthorRepository : ILiteratureAuthorRepository
    {
        private readonly ApiDbContext _context;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public LiteratureAuthorRepository(ApiDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            this.logger = logger;
            this.mapper = mapper;
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


        public async Task<IEnumerable<LiteratureAuthorDto>> FindAsync(AuthorResourcesParameters authorResourceParameters)
        {
            var sortOrder = authorResourceParameters.sortOrder;

            var author = _context.LiteratureAuthors.AsQueryable();
            if (authorResourceParameters.includeAllInfo)
            {
                //await author.Where(a => a.FirstName.Contains(searchParam) || a.LastName.Contains(searchParam)).ToListAsync();
                author = author.Include(a => a.Literatures);
            }
            var authorDto = mapper.Map<IEnumerable<LiteratureAuthorDto>>(author);
            foreach (var item in authorDto)
            {
               
                item.Age = ModelsJoinHelper.GetAythorAge(item.DateOfBirth);
                item.AmoutWorks = item.Literatures.Count;
                item.LatestWork = item.Literatures.OrderBy(a => a.ReleaseDate).ElementAt(0).Title.ToString();

            }
            if (authorResourceParameters == null)
            {
                return authorDto;
            }

            if (!String.IsNullOrEmpty(authorResourceParameters.searchString))
            {
                var searchParam = authorResourceParameters.searchString.ToLower(); 
                authorDto = authorDto.Where(s => s.FirstName.ToLower().Contains(searchParam.ToLower().Trim())
                                                    || s.LastName.ToLower().Contains(searchParam.ToLower().Trim())
                                                    || s.Age.Equals(searchParam.Trim())
                                                    || s.DateOfBirth.ToString().Contains(searchParam.ToLower().Trim()));


                //Todo Add countfor each creteria result
            }

            
            switch (sortOrder)
            {
                case "fullName_desc":
                    authorDto = authorDto.OrderByDescending(s => s.FullName);
                    break;
                case "Age":
                    authorDto = authorDto.OrderBy(s => s.Age);
                    break;
                case "age_desc":
                    authorDto = authorDto.OrderByDescending(s => s.Age);
                    break;
                case "LatestWork":
                    authorDto = authorDto.OrderBy(s => s.LatestWork);
                    break;
                case "latestWork_desc":
                    authorDto = authorDto.OrderByDescending(s => s.LatestWork);
                    break;
                case "CountLiteraturesSortParm":
                    authorDto = authorDto.OrderBy(s => s.AmoutWorks);
                    break;
                case "countLiteraturesSortParm_desc":
                    authorDto = authorDto.OrderByDescending(s => s.AmoutWorks);
                    break;
                default:
                    authorDto = authorDto.OrderBy(s => s.FullName);
                    break;

            }

            return authorDto;

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