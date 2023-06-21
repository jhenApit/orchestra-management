using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Concerts;
using OrchestraAPI.Repositories.Orchestras;
using System;

namespace OrchestraAPI.Services.Concerts
{
    public class ConcertService : IConcertService
    {

        private readonly IConcertRepository _repository;
        private readonly IOrchestraRepository _orchestraRepository;
        private readonly IMapper _mapper;

        public ConcertService(IConcertRepository repository, IOrchestraRepository orchestraRepository, IMapper mapper)
        {
            _repository = repository;
            _orchestraRepository = orchestraRepository;
            _mapper = mapper;
        }

        public async Task<ConcertDto> AddConcert(ConcertCreationDto concertToAdd)
        {
            if (concertToAdd == null)
                return null;

            var concertModel = new Concert
            {
                Name = concertToAdd.Name,
                Description = concertToAdd.Description,
                PerformanceDate = concertToAdd.PerformanceDate,
                Image = concertToAdd.Image
            };

            var resultCode = await _repository.AddConcert(concertModel);

            if (resultCode <= 0)
                return null;

            var concertResult = await _repository.GetConcertById(resultCode);

            if (concertResult == null)
                return null;

            return new ConcertDto
            {
                Id = concertResult.Id,
                Name = concertResult.Name,
                Description = concertResult.Description,
                PerformanceDate = concertResult.PerformanceDate,
                Image = concertResult.Image
            };
        }


        public async Task<bool> DeleteConcert(int id)
        {
            return await _repository.DeleteConcert(id);
        }

        public async Task<IEnumerable<ConcertDto>> GetAllConcerts()
        {
            var concertModel = await _repository.GetAllConcerts();
            return concertModel.Select(concert => new ConcertDto
            {
                Id = concert.Id,
                Name = concert.Name,
                Description = concert.Description,
                PerformanceDate = concert.PerformanceDate,
                Players = concert.Players
            });
        }

        public async Task<ConcertDto> GetConcertById(int id)
        {
            var concertModel = await _repository.GetConcertById(id);
            if (concertModel == null) return null;

            return new ConcertDto
            {
                Id = concertModel.Id,
                Name = concertModel.Name,
                Description = concertModel.Description,
                PerformanceDate = concertModel.PerformanceDate,
                Players = concertModel.Players
            };
        }

        public async Task<bool> UpdateConcert(int concertId, ConcertUpdateDto concertUpdateDto)
        {
            try
            {
                var existingConcert = await _repository.GetConcertById(concertId);
                if (existingConcert == null)
                {
                    return false;
                }

                // Update the properties of the existing concert
                existingConcert.Name = concertUpdateDto.Name;
                existingConcert.Description = concertUpdateDto.Description;
                existingConcert.PerformanceDate = concertUpdateDto.PerformanceDate;
                existingConcert.PerformanceDate = concertUpdateDto.PerformanceDate;
                existingConcert.OrchestraId = concertUpdateDto.OrchestraId;

                return await _repository.UpdateConcert(concertId, existingConcert);
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection error", ex);
            }
        }

        public async Task<IEnumerable<ConcertDto>> GetConcertsByPlayerId(int id)
        {
            var concerts = await _repository.GetConcertsByPlayerId(id);
            return _mapper.Map<IEnumerable<ConcertDto>>(concerts);
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
