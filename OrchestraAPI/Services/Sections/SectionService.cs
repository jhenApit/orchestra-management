using AutoMapper;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Sections;

namespace OrchestraAPI.Services.Sections
{
    public class SectionService : ISectionService
    {

        private readonly ISectionRepository _sectionRepository;
        private readonly IMapper _mapper;

        public SectionService(ISectionRepository sectionRepository, IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SectionDto>> GetAllSections()
        {
            var sectionModel = await _sectionRepository.GetAllSections();

            return _mapper.Map<IEnumerable<SectionDto>>(sectionModel);
        }

        public async Task<IEnumerable<PlayerDto>> GetLeaderboardsBySection(int id)
        {
            var players = await _sectionRepository.GetLeaderboardsBySection(id);

            return _mapper.Map<IEnumerable<PlayerDto>>(players);
        }

        public async Task<SectionDto?> GetSectionById(int id)
        {
            var sectionModel = await _sectionRepository.GetSectionById(id);
            if (sectionModel == null) return null;

            return _mapper.Map<SectionDto>(sectionModel);
        }

        public async Task<SectionDto?> GetSectionByName(string sectionName)
        {
            var sectionModel = await _sectionRepository.GetSectionByName(sectionName);
            if (sectionModel == null) return null;

            return _mapper.Map<SectionDto>(sectionModel);
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
