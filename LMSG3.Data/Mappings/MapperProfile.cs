using AutoMapper;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Helpers;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace LMSG3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Literature, LiteratureDto>().ReverseMap();
                //.ForMember(dest => dest.LiteraLevelId,
                     // from => from.MapFrom(s => s.LevelName));
            CreateMap<LiteratureAuthor, LiteratureAuthorDto>().ReverseMap()
               .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore());
            //CreateMap<LiteratureLevel, LiteratureLevelDto>().ReverseMap();
            //CreateMap<LiteratureType, LiteratureTypeDto>().ReverseMap();
            //CreateMap<Subject, SubjectDto>().ReverseMap();


            //.ForMember(
            //        dest => dest.Courses,
            //        from => from.MapFrom(s => s.Enrollments.Select(e => e.Course).ToList()));

            //  CreateMap<LiteratureAuthor, LiteratureAuthorDto>();
            //.ForMember(
            //    dest => dest.FullName,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            // .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore());
            //.ForMember(
            //    dest => dest.Age,
            //    opt => opt.MapFrom(src => src.DateOfBirth));  //.GetCurrentAge()

        }


    }
}
