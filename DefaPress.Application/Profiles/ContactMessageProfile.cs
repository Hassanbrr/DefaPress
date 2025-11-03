using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class ContactMessageProfile : Profile
    {
        public ContactMessageProfile()
        {
            // Entity -> DTO
            CreateMap<ContactMessage, ContactMessageDto>();

            // Create DTO -> Entity
            CreateMap<ContactMessageCreateDto, ContactMessage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SentAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsRead, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<ContactMessageUpdateDto, ContactMessage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.SentAt, opt => opt.Ignore());
        }
    }
}