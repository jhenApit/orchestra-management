using AutoMapper;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Models;

namespace OrchestraApi.Mappings
{
    public class InstrumentMapping : Profile
    {
        public InstrumentMapping()
        {
            CreateMap<Instrument, InstrumentDto>();
        }
    }
}
