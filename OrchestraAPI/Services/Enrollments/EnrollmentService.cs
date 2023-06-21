using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Concerts;
using OrchestraAPI.Repositories.Enrollments;
using OrchestraAPI.Repositories.Instruments;
using OrchestraAPI.Repositories.Orchestras;
using OrchestraAPI.Repositories.Players;
using OrchestraAPI.Repositories.Sections;
using System.Numerics;

namespace OrchestraAPI.Services.Enrollments
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IConcertRepository _concertRepository;
        private readonly IOrchestraRepository _orchestraRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;

        public EnrollmentService(IPlayerRepository playerRepository, IConcertRepository concertRepository, IOrchestraRepository orchestraRepository, ISectionRepository sectionRepository, IInstrumentRepository instrumentRepository, IEnrollmentRepository enrollmentRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _concertRepository = concertRepository;
            _orchestraRepository = orchestraRepository;
            _sectionRepository = sectionRepository;
            _instrumentRepository = instrumentRepository;
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
        }

        public async Task<ConcertDto> AddOrchestra(int concertId, int orchestraId)
        {
            var orchestra = await _orchestraRepository.GetOrchestraById(orchestraId);
            var concert = await _concertRepository.GetConcertById(concertId);
            concert.OrchestraId = orchestraId;
            
            await _concertRepository.UpdateConcert(concertId, concert);

            return _mapper.Map<ConcertDto>(concert);
        }

        public async Task<bool> EnrollPlayerToOrchestra(int playerId, int orchestraId, int sectionId, int instrumentId, int experience)
        {
            var player = await _playerRepository.GetPlayerById(playerId);
            var orchestra = await _orchestraRepository.GetOrchestraById(orchestraId);
            var section = await _sectionRepository.GetSectionById(sectionId);
            var instrument = await _instrumentRepository.GetInstrumentById(instrumentId);
            if (player == null || orchestra == null || section == null || instrument == null)
            {
                return false;
            }

            var result = await _enrollmentRepository.EnrollToAnOrchestra(playerId, orchestraId, sectionId, instrumentId, experience);
            return result;
        }

        public async Task<IEnumerable<EnrolleesDto>> GetAllEnrolleesByOrchestraId(int id)
        {
            var enrollees = await _enrollmentRepository.GetAllEnrolleesByOrchestaId(id);
            return _mapper.Map<IEnumerable<EnrolleesDto>>(enrollees);
        }

        public async Task<PlayerDto> AcceptEnrollee(int orchestraID, int playerId)
        {
            var player = await _playerRepository.GetPlayerById(playerId);
            var orchestra = await _orchestraRepository.GetOrchestraById(orchestraID);
            if (player == null || orchestra == null)
            {
                return null;
            }

            var result = await _enrollmentRepository.AcceptEnrollee(orchestraID, playerId);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<PlayerDto>(result);
        }
    }
}