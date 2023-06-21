using AutoMapper;
using OrchestraAPI.Dtos.Conductor;
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
    public class ConductorMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public ConductorMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ConductorMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Conductor maps correctly to ConductorDto
        /// </summary>
        [Fact]
        public void ConductorToConductorDto_MapsCorrectly_ReturnsConductor()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor",
                Orchestra = new Orchestra
                {
                    Id = 1,
                    Name = "Test Orchestra"
                }
            };

            // Act
            var conductorDto = _mapper.Map<ConductorDto>(conductor);

            // Assert
            Assert.NotNull(conductorDto);
            Assert.Equal(conductor.Id, conductorDto.Id);
            Assert.Equal(conductor.Name, conductorDto.Name);
            Assert.Equal(conductor.Orchestra.Name, conductorDto.Orchestra);
        }

        /// <summary>
        /// Test that ConductorDto maps incorrectly to Conductor
        /// </summary>
        [Fact]
        public void ConductorToConductorDto_NullInput_ReturnsNull()
        {
            // Arrange
            Conductor conductor = null;

            // Act
            var conductorDto = _mapper.Map<ConductorDto>(conductor);

            // Assert
            Assert.Null(conductorDto);
        }
    }
}
