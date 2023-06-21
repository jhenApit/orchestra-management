using AutoMapper;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Instruments;

namespace OrchestraAPI.Services.Instruments
{
    public class InstrumentService : IInstrumentService
    {

        private readonly IInstrumentRepository _repository;
        private readonly IMapper _mapper;

        public InstrumentService(IInstrumentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InstrumentDto> GetInstrumentById(int id)
        {
            var instrumentModel = await _repository.GetInstrumentById(id);
            return _mapper.Map<InstrumentDto>(instrumentModel);
        }

        public async Task<InstrumentDto> GetInstrumentByName(string name)
        {
            var instrumentModel = await _repository.GetInstrumentByName(name);
            return _mapper.Map<InstrumentDto>(instrumentModel);
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
