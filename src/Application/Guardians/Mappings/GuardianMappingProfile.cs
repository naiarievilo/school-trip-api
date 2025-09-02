using AutoMapper;
using SchoolTripApi.Application.Guardians.Commands.UpdateGuardianInfo;
using SchoolTripApi.Domain.GuardianAggregate;

namespace SchoolTripApi.Application.Guardians.Mappings;

public sealed class GuardianMappingProfile : Profile
{
    public GuardianMappingProfile()
    {
        CreateMap<UpdateGuardianInfoCommand, Guardian>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.EmergencyContact, opt => opt.MapFrom(src => src.EmergencyContact));
    }
}