using AutoMapper;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Helpers;
using System.Linq.Expressions;
using System;

namespace LMSG3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Literature, LiteratureDto>().ReverseMap();
            CreateMap<LiteratureAuthor, LiteratureAuthorDto>().ReverseMap()
               .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore());
            CreateMap<LiteratureLevel, LiteratureLevelDto>().ReverseMap();
            CreateMap<LiteratureType, LiteratureTypeDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();


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
