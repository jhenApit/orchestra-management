using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Models;

namespace OrchestraAPI.Mappings
{
    public class ConcertMapping : Profile
    {
        public ConcertMapping()
        {
            CreateMap<Concert, ConcertDto>();
            CreateMap<Concert, ConcertCreationDto>();
        }
    }
}
