using AutoMapper;
using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Conductors;

namespace OrchestraAPI.Services.Conductors
{
    public class ConductorService : IConductorService
    {
        private readonly IConductorRepository _conductorRepository;
        private readonly IMapper _mapper;

        public ConductorService(IConductorRepository conductorRepository, IMapper mapper)
        {
            _conductorRepository = conductorRepository;
            _mapper = mapper;
        }

        public async Task<ConductorDto> AddConductor(ConductorCreationDto conductor)
        {
            var conductorModel = new Conductor
            {
                Name = conductor.Name,
                UserId = conductor.UserId
            };

            conductorModel.Id = await _conductorRepository.AddConductor(conductorModel);
            return _mapper.Map<ConductorDto>(conductorModel);
        }

        public async Task<bool> DeleteConductor(int id)
        {
            return await _conductorRepository.DeleteConductor(id);
        }

        public async Task<IEnumerable<ConductorDto>> GetAllConductors()
        {
            var conductors = await _conductorRepository.GetAllConductors();
            return _mapper.Map<IEnumerable<ConductorDto>>(conductors);
        }

        public async Task<ConductorDto> GetConductorById(int id)
        {
            var conductors = await _conductorRepository.GetConductorById(id);
            return _mapper.Map<ConductorDto>(conductors);
        }

        public async Task<ConductorDto> GetConductorByName(string name)
        {
            var conductors = await _conductorRepository.GetConductorByName(name);
            return _mapper.Map<ConductorDto>(conductors);
        }

        public async Task<bool> UpdateConductor(int id, ConductorUpdationDto conductor)
        {
            var updateConductor = new Conductor
            {
                Id = id,
                Name = conductor.Name
            };
            
            return await _conductorRepository.UpdateConductor(id, updateConductor);
        }

        public bool IsIdValid(int id)
        {
            return id > 0 && IsNumeric(id.ToString());
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}
