using AutoMapper;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Models;

namespace OrchestraAPI.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Role, opt => opt.MapFrom(usr => MapUserRole(usr.Role).ToString()));
            CreateMap<UserDto, UserCreationDto>()
                .ForMember(dto => dto.Role, opt => opt.MapFrom(usr => MapRoleUser(usr.Role)));
        }   

        public string MapUserRole(int role)
        {
            return role switch
            {
                1 => "Conductor",
                2 => "Player",
                _ => "Unknown",
            };
        }

        public int MapRoleUser(string role)
        {
            return role switch
            {
                "Conductor" => 1,
                "Player" => 2,
                _ => -1
            };
        }
    }
}
