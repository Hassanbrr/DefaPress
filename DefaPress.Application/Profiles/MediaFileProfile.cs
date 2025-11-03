using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class MediaFileProfile : Profile
    {
        public MediaFileProfile()
        {
            // Entity -> DTO
            CreateMap<MediaFile, MediaFileDto>();

            // Create DTO -> Entity (برای آپلود فایل)
            CreateMap<MediaFileCreateDto, MediaFile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FileName, opt => opt.Ignore())
                .ForMember(dest => dest.FileUrl, opt => opt.Ignore())
                .ForMember(dest => dest.FileType, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Article, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<MediaFileUpdateDto, MediaFile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FileName, opt => opt.Ignore())
                .ForMember(dest => dest.FileUrl, opt => opt.Ignore())
                .ForMember(dest => dest.FileType, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Article, opt => opt.Ignore());
        }
    }
}