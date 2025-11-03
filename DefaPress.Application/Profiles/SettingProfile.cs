using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            // Entity -> DTO
            CreateMap<Setting, SettingDto>();

            // Create DTO -> Entity
            CreateMap<SettingCreateDto, Setting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<SettingUpdateDto, Setting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Key, opt => opt.Ignore());
        }
    }
}