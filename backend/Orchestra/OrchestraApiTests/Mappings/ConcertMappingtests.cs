using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Mappings;
using OrchestraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Mappings
{
    public class ConcertMappingtests
    {
            private readonly IMapper _mapper;
            private readonly MapperConfiguration _mapperConfiguration;

        public ConcertMappingtests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ConcertMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Concert maps correctly to ConcertDto
        /// </summary>
        [Fact]
        public void ConcertToConcertDto_MapsCorrectly_ReturnsConcert()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Test Concert",
                Description = "Test Description",
                PerformanceDate = DateTime.Now,
                OrchestraId = 1
            };

            // Act
            var concertDto = _mapper.Map<ConcertDto>(concert);

            // Assert
            Assert.NotNull(concertDto);
            Assert.Equal(concert.Id, concertDto.Id);
            Assert.Equal(concert.Name, concertDto.Name);
            Assert.Equal(concert.Description, concertDto.Description);
            Assert.Equal(concert.PerformanceDate, concertDto.PerformanceDate);
            Assert.Equal(concert.OrchestraId, concertDto.OrchestraId);
        }
        /// <summary>
        /// Test that ConcertDto maps incorrectly to Concert
        /// </summary>
        [Fact]
        public void ConcertToConcertDto_NullInput_ReturnsNull()
        {
            // Arrange
            Concert concert = null;

            // Act
            var concertDto = _mapper.Map<ConcertDto>(concert);

            // Assert
            Assert.Null(concertDto);
        }

        /// <summary>
        /// Test ConcertCreationDto maps correctly to Concert
        /// </summary>
        [Fact]
        public void ConcertToConcertCreationDto_MapsCorrectly_ReturnsConcertCreationDto()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Test Concert",
                Description = "Test Description",
                PerformanceDate = DateTime.Now,
                OrchestraId = 1
            };
            // Act
            var concertCreationDto = _mapper.Map<ConcertCreationDto>(concert);
            // Assert
            Assert.NotNull(concertCreationDto);
            Assert.Equal(concert.Name, concertCreationDto.Name);
            Assert.Equal(concert.Description, concertCreationDto.Description);
        }

        /// <summary>
        /// Test that ConcertCreationDto maps incorrectly to Concert
        /// </summary>
        [Fact]
        public void ConcertToConcertCreationDto_NullInput_ReturnsNull()
        {
            // Arrange
            Concert concert = null;

            // Act
            var concertCreationDto = _mapper.Map<ConcertCreationDto>(concert);

            // Assert
            Assert.Null(concertCreationDto);
        }

    }
}
