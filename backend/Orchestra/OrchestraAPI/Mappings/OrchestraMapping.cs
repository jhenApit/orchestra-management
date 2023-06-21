using AutoMapper;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Models;
using System.Xml;

namespace OrchestraAPI.Mappings
{
    public class OrchestraMapping : Profile
    {
        public OrchestraMapping()
        {
            CreateMap<Orchestra, OrchestraDto>()
                .ForMember(dto => dto.Conductor, opt => opt.MapFrom(o => o.Conductor != null ? o.Conductor.Name : ""));
        }
    }
}
