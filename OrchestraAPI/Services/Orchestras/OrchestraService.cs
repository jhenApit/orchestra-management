using AutoMapper;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Conductors;
using OrchestraAPI.Repositories.Orchestras;

namespace OrchestraAPI.Services.Orchestras
{
    public class OrchestraService : IOrchestraService
    {
        private readonly IOrchestraRepository _orchestraRepository;
        private readonly IConductorRepository _conductorRepository;
        private readonly IMapper _mapper;
        
        public OrchestraService(IOrchestraRepository orchestraRepository, IConductorRepository conductorRepository, IMapper mapper)
        {
            _orchestraRepository = orchestraRepository;
            _conductorRepository = conductorRepository;
            _mapper = mapper;
        }

        public async Task<OrchestraDto> AddOrchestra(OrchestraCreationDto orchestra)
        {
            var newOrchestra = new Orchestra
            {
                Name = orchestra.Name,
                Image = orchestra.Image,
                Date = DateTime.Now,
                Conductor = await _conductorRepository.GetConductorById(orchestra.ConductorId),
                Description = orchestra.Description
            };

            newOrchestra.Id = await _orchestraRepository.AddOrchestra(newOrchestra);
            return _mapper.Map<OrchestraDto>(newOrchestra);
        }

        public async Task<bool> DeleteOrchestra(int id)
        {
            return await _orchestraRepository.DeleteOrchestra(id);
        }

        public async Task<IEnumerable<OrchestraDto>> GetAllOrchestras()
        {
            var orchestraModel = await _orchestraRepository.GetAllOrchestras();
            return _mapper.Map<IEnumerable<OrchestraDto>>(orchestraModel);
        }

        public async Task<OrchestraDto> GetOrchestraById(int id)
        {
            var orchestraModel = await _orchestraRepository.GetOrchestraById(id);
            return _mapper.Map<OrchestraDto>(orchestraModel);
        }

        public async Task<OrchestraDto> GetOrchestraByName(string name)
        {
            var orchestraModel = await _orchestraRepository.GetOrchestraByName(name);
            return _mapper.Map<OrchestraDto>(orchestraModel);
        }

        public async Task<IEnumerable<OrchestraDto>> GetOrchestrasByPlayerId(int id)
        {
            var orchestras = await _orchestraRepository.GetOrchestrasByPlayerId(id);
            return _mapper.Map<IEnumerable<OrchestraDto>>(orchestras);
        }

        public async Task<bool> UpdateOrchestra(int id, OrchestraUpdationDto orchestra)
        {
            var newOrchestra = new Orchestra
            {
                Id = id,
                Name = orchestra.Name,
                Image = orchestra.Image,
                Conductor = await _conductorRepository.GetConductorById(orchestra.ConductorId),
                Description = orchestra.Description
            };
            
            return await _orchestraRepository.UpdateOrchestra(id, newOrchestra);
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
