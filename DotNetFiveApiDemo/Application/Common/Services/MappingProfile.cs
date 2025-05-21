using AutoMapper;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.WebApi.DTOs;

namespace DotNetFiveApiDemo.Application.Common.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // UserCreationRequest -> ApplicationUser
            CreateMap<UserCreationCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(src => src.Number))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForPath(dest => dest.Address.Country, opt => opt.MapFrom(src => src.Country));

            // UserLoginRequest -> ApplicationUser
            CreateMap<UserLoginCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            // UserUpdateRequest -> ApplicationUser
            CreateMap<UserUpdateCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(src => src.Number))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.Country, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.ZipCode, opt => opt.MapFrom(src => src.ZipCode));

            // ApplicationUser -> UserDto
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}