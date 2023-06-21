using AutoMapper;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Models;

namespace OrchestraAPI.Mappings
{
    public class EnrollmentMapping : Profile
    {
        public EnrollmentMapping()
        {
            CreateMap<Enrollment, EnrolleesDto>();
        }
    }
}
