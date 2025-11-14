using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain.PollModels;

namespace DefaPress.Application.Profiles
{
    public class PollProfile : Profile
    {
        public PollProfile()
        {
            // Mapping from Poll to PollDto
            CreateMap<Poll, PollDto>()
                .ForMember(dest => dest.TotalVotes, opt => opt.MapFrom(src => src.Votes.Count))
                .ForMember(dest => dest.HasVoted, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            // Mapping from PollOption to PollOptionDto
            CreateMap<PollOption, PollOptionDto>()
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Count))
                .ForMember(dest => dest.Percentage, opt => opt.Ignore())
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());
        }
    }
}