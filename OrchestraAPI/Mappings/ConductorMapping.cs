using AutoMapper;
using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Models;

namespace OrchestraAPI.Mappings
{
    public class ConductorMapping : Profile
    {
        public ConductorMapping()
        {
            CreateMap<Conductor, ConductorDto>()
                .ForMember(dto => dto.Orchestra, opt => opt.MapFrom(c => c.Orchestra != null ? c.Orchestra.Name : ""));
            CreateMap<ConductorCreationDto, Conductor>();
            CreateMap<Conductor, ConductorCreationDto>();
        }
    }
}