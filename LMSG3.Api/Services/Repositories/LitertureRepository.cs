using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMSG3.Data;
using LMSG3.Core.Models.Entities;
using LMSG3.Api.ResourceParameters;
using AutoMapper;
using LMSG3.Core.Models.Dtos;

namespace LMSG3.Api.Services.Repositories
{
    public class LitertureRepository : ILiteratureRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper mapper;

        public LitertureRepository(ApiDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
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


        public async Task<IEnumerable<LiteratureDto>> FindAsync(LiteraturesResourceParameters literaturesResourceParameters)
        {
            var literature =  _context.Literatures.AsQueryable();
            var currentFilter = literaturesResourceParameters.levelFilter;
            var sortOrder = literaturesResourceParameters.sortOrder;
            if (literaturesResourceParameters.includeAllInfo)
            {
                literature = literature.Include(e => e.Authors);

            }
            var literaDto =  mapper.Map<IEnumerable<LiteratureDto>>(literature);
            foreach (var item in literaDto)
            {
                item.LevelName = ModelsJoinHelper.GetLevelName(item.LiteraLevelId, _context);
                item.LiteraTypeName = ModelsJoinHelper.GetTypeName(item.LiteraTypeId, _context);
                item.SubjectName = ModelsJoinHelper.GetSubjectName(item.SubId, _context);
            }
            if (literaturesResourceParameters == null)
            {
                return literaDto;
            }
            if (currentFilter > 0)
            {
                literaDto = literaDto.Where(s => s.LiteraLevelId.Equals(currentFilter));

            }
           
            if (!string.IsNullOrWhiteSpace(literaturesResourceParameters.searchString))
            {

                literaDto = literaDto.Where(l => l.Title.ToLower()
                                 .Contains(literaturesResourceParameters.searchString.ToLower())
                                  || l.SubjectName.ToLower().Contains(literaturesResourceParameters.searchString.ToLower())
                                  || l.Description.ToLower().Contains(literaturesResourceParameters.searchString.ToLower())
                                  || l.Authors.Any(a => a.FirstName.Contains(literaturesResourceParameters.searchString))
                                  || l.Authors.Any(a => a.LastName.Contains(literaturesResourceParameters.searchString)));

            }

            switch (sortOrder)
            {
                case "title_desc":
                    literaDto = literaDto.OrderByDescending(s => s.Title);
                    break;
                case "Description":
                    literaDto = literaDto.OrderBy(s => s.Description);
                    break;
                case "description_desc":
                    literaDto = literaDto.OrderByDescending(s => s.Description);
                    break;
                case "ReleaseDate":
                    literaDto = literaDto.OrderBy(s => s.ReleaseDate);
                    break;
                case "releaseDate_desc":
                    literaDto = literaDto.OrderByDescending(s => s.ReleaseDate);
                    break;
                case "Subject":
                    literaDto = literaDto.OrderBy(s => s.SubjectName);
                    break;
                case "subject_desc":
                    literaDto = literaDto.OrderByDescending(s => s.SubjectName);
                    break;
                case "Level":
                    literaDto = literaDto.OrderBy(s => s.LevelName);
                    break;
                case "level_desc":
                    literaDto = literaDto.OrderByDescending(s => s.LevelName);
                    break;
                case "Type":
                    literaDto = literaDto.OrderBy(s => s.LiteraTypeName);
                    break;
                case "type_desc":
                    literaDto = literaDto.OrderByDescending(s => s.LiteraTypeName);
                    break;
                default:
                    literaDto = literaDto.OrderBy(s => s.Title);
                    break;
            }

            return literaDto;


        }

        public bool LiteratureExist(LiteraturesResourceParameters literaturesResourceParameters)
        {
            var literature = _context.Literatures.AsQueryable();
            var searchStr = literaturesResourceParameters.searchString;
            bool exist = false;
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                //literature = literature.Where(l => l.Title.ToLower().Equals(literaturesResourceParameters.titleStr.ToLower()));
                foreach (var item in literature)
                {
                   
                    exist = String.Equals(item.Title.ToString(), searchStr, StringComparison.OrdinalIgnoreCase);
                    if (exist)
                    {
                        break;
                    }
                }
              
            }

            return exist;

        }

        public bool LiteratureExist(int id)
        {
            return _context.Literatures.Any(l => l.Id == id);
          
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

        public bool CompleteAsync()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Remove(Literature literature)
        {
            throw new NotImplementedException();
        }

        
    }
}