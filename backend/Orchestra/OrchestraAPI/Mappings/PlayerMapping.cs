using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;

namespace OrchestraAPI.Mappings
{
    public class PlayerMapping : Profile
    {
        public PlayerMapping()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(dto => dto.Section, opt => opt.MapFrom(pl => pl.Section!));
            CreateMap<PlayerCreationDto, Player>();
        }

    }
}
