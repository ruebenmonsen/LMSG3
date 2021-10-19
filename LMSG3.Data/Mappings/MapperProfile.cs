using AutoMapper;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
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
            CreateMap<Literature, LiteratureDto>().ReverseMap()
                .ForMember(dest => dest.SubId, opt => opt.Ignore());
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

            CreateMap<Literature, LiteratureDto>().ReverseMap();
            CreateMap<LiteratureAuthor, LiteratureAuthorDto>().ReverseMap();
            CreateMap<LiteratureLevel, LiteratureLevelDto>().ReverseMap();
            CreateMap<LiteratureType, LiteratureTypeDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();

            
           
            CreateMap<Course, CourseIndexViewModel>()
                .ForMember(dest=>dest.Modelslist, frm=>frm.MapFrom(m=>m.Modules.ToList()))
                .ReverseMap();
            CreateMap<IEnumerable<Course>, IndexCourseViewModel>()
                .ForMember(dest => dest.CoursesList, from => from.MapFrom(c => c.ToList()))
                .ReverseMap();

            CreateMap<Module, ModelListViewModel>()
                .ForMember(dest=>dest.ActivitiesList, frm=>frm.MapFrom(a=>a.Activities.ToList()))
                .ReverseMap();
            CreateMap<IEnumerable<Module>, CourseIndexViewModel>()
                .ForMember(dest => dest.Modelslist, from => from.MapFrom(c => c.ToList()))
                .ReverseMap();

            CreateMap<Activity, ActivityListViewModel>()
                .ReverseMap();

            CreateMap<Course, CreateCourseViewModel>()
                .ReverseMap();
            CreateMap<Module, CreateModelListViewModel>().ReverseMap();
        }


    }
}
