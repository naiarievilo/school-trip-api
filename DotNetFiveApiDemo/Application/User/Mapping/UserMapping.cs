using AutoMapper;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.WebApi.User.DTOs;

namespace DotNetFiveApiDemo.Application.User.Mapping
{
    internal class UserMapping : Profile
    {
        public UserMapping()
        {
            // SignUpUserRequest -> ApplicationUser
            CreateMap<SignUpUserRequest, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            // UserLoginRequest -> ApplicationUser
            CreateMap<SignInUserRequest, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            // UserUpdateRequest -> ApplicationUser
            CreateMap<UpdateUserInfoRequest, AppUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(src => src.Number))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.Country, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.PostalCode, opt => opt.MapFrom(src => src.PostalCode));

            // ApplicationUser -> UserDto
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForPath(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number))
                .ForPath(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForPath(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
                .ForPath(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForPath(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country));
        }
    }
}