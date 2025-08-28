using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();

            CreateMap<ApplicationUserUpdateDto, ApplicationUser>()
                .ForMember(d => d.UserName, opt => opt.Ignore()) // username handled by Identity
                .ForMember(d => d.Email, opt => opt.Ignore());   // email handled elsewhere if needed

        }
    }
}
