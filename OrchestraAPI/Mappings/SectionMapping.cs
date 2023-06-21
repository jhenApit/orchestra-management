using AutoMapper;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Models;

namespace OrchestraApi.Mappings
{
    public class SectionMapping : Profile
    {
        public SectionMapping()
        {
            CreateMap<Section, SectionDto>();
        }
    }
}