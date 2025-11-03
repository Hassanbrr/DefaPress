using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class NewsletterSubscriberProfile : Profile
    {
        public NewsletterSubscriberProfile()
        {
            // Entity -> DTO
            CreateMap<NewsletterSubscriber, NewsletterSubscriberDto>();

            // Create DTO -> Entity
            CreateMap<NewsletterSubscriberCreateDto, NewsletterSubscriber>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubscribedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<NewsletterSubscriberUpdateDto, NewsletterSubscriber>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.SubscribedAt, opt => opt.Ignore());
        }
    }
}