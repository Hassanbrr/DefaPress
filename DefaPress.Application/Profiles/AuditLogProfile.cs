using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile()
        {
            // Entity -> DTO
            CreateMap<AuditLog, AuditLogDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
        }
    }
}