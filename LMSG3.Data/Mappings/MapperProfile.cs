using AutoMapper;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Literature, LiteratureDto>().ReverseMap();
            CreateMap<LiteratureAuthor, LiteratureAuthorDto>().ReverseMap();
            CreateMap<LiteratureLevel, LiteratureLevelDto>().ReverseMap();
            CreateMap<LiteratureType, LiteratureTypeDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();

            CreateMap<Course, CreateCourseViewModel>().ReverseMap();
        }
        
    }
}
