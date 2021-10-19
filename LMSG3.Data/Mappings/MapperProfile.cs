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
