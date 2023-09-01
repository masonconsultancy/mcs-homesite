﻿using AutoMapper;
using MCS.HomeSite.Areas.Models.Users;

namespace MCS.HomeSite.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
