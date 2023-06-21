using AutoMapper;
using OrchestraAPI.Dtos.Orchestra;
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
    public class OrchestraMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public OrchestraMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrchestraMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Orchestra maps correctly to OrchestraDto
        /// </summary>
        [Fact]
        public void OrchestraToOrchestraDto_MapsCorrectly_ReturnsOrchestra()
        {
            // Arrange
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Conductor = new Conductor
                {
                    Id = 1,
                    Name = "Test Conductor"
                }
            };

            // Act
            var orchestraDto = _mapper.Map<OrchestraDto>(orchestra);

            // Assert
            Assert.NotNull(orchestraDto);
            Assert.Equal(orchestra.Id, orchestraDto.Id);
            Assert.Equal(orchestra.Name, orchestraDto.Name);
            Assert.Equal(orchestra.Conductor.Name, orchestraDto.Conductor);
        }
        /// <summary>
        /// Test that OrchestraDto maps incorrectly to Orchestra
        /// </summary>
        [Fact]
        public void OrchestraToOrchestraDto_NullInput_ReturnsNull()
        {
            // Arrange
            Orchestra orchestra = null;

            // Act
            var orchestraDto = _mapper.Map<OrchestraDto>(orchestra);

            // Assert
            Assert.Null(orchestraDto);
        }
    }
}
