using AutoMapper;
using OrchestraApi.Mappings;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Mappings
{
    public class SectionMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public SectionMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SectionMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Section maps correctly to SectionDto
        /// </summary>
        [Fact]
        public void SectionToSectionDto_MapsCorrectly_ReturnsSection()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Name = "Test Section",
            };

            // Act
            var sectionDto = _mapper.Map<SectionDto>(section);
            // Assert
            Assert.NotNull(sectionDto);
            Assert.Equal(section.Id, sectionDto.Id);
            Assert.Equal(section.Name, sectionDto.Name);
        }

        /// <summary>
        /// Test that SectionDto maps incorrectly to Section
        /// </summary>
        [Fact]
        public void SectionToSectionDto_NullInput_ReturnsNull()
        {
            // Arrange
            Section section = null;

            // Act
            var sectionDto = _mapper.Map<SectionDto>(section);

            // Assert
            Assert.Null(sectionDto);
        }
    }
}
