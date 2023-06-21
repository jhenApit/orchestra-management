using AutoMapper;
using OrchestraApi.Mappings;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Models;
using Xunit;

namespace OrchestraApiTests.Mappings
{
    public class InstrumentMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public InstrumentMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InstrumentMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Instrument maps correctly to InstrumentDto
        /// </summary>
        [Fact]
        public void InstrumentToInstrumentDto_MapsCorrectly_ReturnsInstrument()
        {
            // Arrange
            var instrument = new Instrument
            {
                Id = 1,
                Name = "Test Instrument"
            };
            // Act
            var instrumentDto = _mapper.Map<InstrumentDto>(instrument);
            // Assert
            Assert.NotNull(instrumentDto);
            Assert.Equal(instrument.Id, instrumentDto.Id);
            Assert.Equal(instrument.Name, instrumentDto.Name);
        }

        /// <summary>
        /// Test that InstrumentDto maps incorrectly to Instrument
        /// </summary>
        [Fact]
        public void InstrumentToInstrumentDto_NullInput_ReturnsNull()
        {
            // Arrange
            Instrument instrument = null!;

            // Act
            var instrumentDto = _mapper.Map<InstrumentDto>(instrument);

            // Assert
            Assert.Null(instrumentDto);
        }
    }
}
