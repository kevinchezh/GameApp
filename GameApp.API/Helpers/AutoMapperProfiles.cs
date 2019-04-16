using System;
using System.Linq;
using AutoMapper;
using GameApp.API.Dtos;
using GameApp.API.Models;

namespace GameApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // create mappings here
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.GameUrl, opt =>
                {
                    opt.MapFrom(src => src.Games.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForDetailDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                })
                .ForMember(dest => dest.GameUrl, opt =>
                {
                    opt.MapFrom(src => src.Games.FirstOrDefault(p => p.IsMain).Url);
                });
            CreateMap<Game, GameForDetailDto>();
            CreateMap<UserForUpdateDto, User>();

            // auto mapper would map together two properties which have the same name without 
            // any configuration
        }
    }
}
