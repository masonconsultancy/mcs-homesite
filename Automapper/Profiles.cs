using AutoMapper;
using mcs_homesite.Areas.Models.Users;

namespace mcs_homesite.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
