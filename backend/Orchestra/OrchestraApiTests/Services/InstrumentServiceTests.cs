using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Instruments;
using OrchestraAPI.Services.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class InstrumentServiceTests
    {
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IInstrumentService _instrumentService;

        public InstrumentServiceTests()
        {
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _mockMapper = new Mock<IMapper>();
            _instrumentService = new InstrumentService(_mockInstrumentRepository.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Test to verify that the GetInstrumentById method returns an instrument with the correct id
        /// </summary>
        [Fact]
        public async Task GetInstrumentById_WithExistingId_ReturnsInstrument()
        {
            // Arrange
            var instrumentId = 1;
            var instrument = new Instrument
            {
                Id = instrumentId,
                Name = "Violin"
            };
            var instrumentDto = new InstrumentDto
            {
                Id = instrumentId,
                Name = "Violin"
            };
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ReturnsAsync(instrument);
            _mockMapper.Setup(x => x.Map<InstrumentDto>(instrument)).Returns(instrumentDto);

            // Act
            var result = await _instrumentService.GetInstrumentById(instrumentId);

            // Assert
            Assert.Equal(instrumentId, result.Id);
            Assert.Equal("Violin", result.Name);
        }

        /// <summary>
        /// Test Get Instrument By Id with a non existing id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInstrumentById_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var instrumentId = 1;
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ReturnsAsync(() => null!);

            // Act
            var result = await _instrumentService.GetInstrumentById(instrumentId);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Instrument By Id when the repository fails
        /// </summary>
        [Fact]
        public async Task GetInstrumentById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var instrumentId = 1;
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _instrumentService.GetInstrumentById(instrumentId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Instrument By Name with an existing name
        /// </summary>
        [Fact]
        public async Task GetInstrumentByName_WithExistingName_ReturnsInstrument()
        {
            // Arrange
            var instrumentName = "Violin";
            var instrument = new Instrument
            {
                Id = 1,
                Name = instrumentName
            };
            var instrumentDto = new InstrumentDto
            {
                Id = 1,
                Name = instrumentName
            };
            _mockInstrumentRepository.Setup(x => x.GetInstrumentByName(instrumentName)).ReturnsAsync(instrument);
            _mockMapper.Setup(x => x.Map<InstrumentDto>(instrument)).Returns(instrumentDto);

            // Act
            var result = await _instrumentService.GetInstrumentByName(instrumentName);

            // Assert
            Assert.Equal(instrumentName, result.Name);
            Assert.True(result.Id == 1);
        }

        /// <summary>
        /// Test Get Instrument By Name with a non existing name
        /// </summary>
        [Fact]
        public async Task GetInstrumentByName_WithNonExistingName_ReturnsNull()
        {
            // Arrange
            var instrumentName = "Violin";
            _mockInstrumentRepository.Setup(x => x.GetInstrumentByName(instrumentName)).ReturnsAsync(() => null!);

            // Act
            var result = await _instrumentService.GetInstrumentByName(instrumentName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetInstrumentByName_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var instrumentName = "Violin";
            _mockInstrumentRepository.Setup(x => x.GetInstrumentByName(instrumentName)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _instrumentService.GetInstrumentByName(instrumentName));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

    }
}
