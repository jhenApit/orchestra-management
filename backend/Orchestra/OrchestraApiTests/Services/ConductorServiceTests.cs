using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Conductors;
using OrchestraAPI.Services.Conductors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class ConductorServiceTests
    {
        private readonly Mock<IConductorRepository> _mockConductorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IConductorService _conductorService;

        public ConductorServiceTests()
        {
            _mockConductorRepository = new Mock<IConductorRepository>();
            _mockMapper = new Mock<IMapper>();
            _conductorService = new ConductorService(_mockConductorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllConductors_WhenCalled_ShouldReturnAllConductors()
        {
            // Arrange
            var conductors = new List<Conductor>
            {
                new Conductor
                {
                    Id = 1,
                    Name = "Conductor 1"
                },
                new Conductor
                {
                    Id = 2,
                    Name = "Conductor 2"
                }
            };
            var conductorDtos = new List<ConductorDto>
            {
                new ConductorDto
                {
                    Id = 1,
                    Name = "Conductor 1"
                },
                new ConductorDto
                {
                    Id = 2,
                    Name = "Conductor 2"
                }
            };
            _mockConductorRepository.Setup(x => x.GetAllConductors()).ReturnsAsync(conductors);
            _mockMapper.Setup(x => x.Map<IEnumerable<ConductorDto>>(conductors)).Returns(conductorDtos);

            // Act
            var result = await _conductorService.GetAllConductors();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ConductorDto>>(result);
            Assert.Equal(2, result.Count());
        }

        /// <summary>
        /// Test to check if GetAllConductors returns null when no values are present in the database
        /// </summary>
        [Fact]
        public async Task GetAllConductors_WhenCalledButNoValues_ReturnsNull()
        {
            // Arrange
            var conductors = new List<Conductor>();
            var conductorDtos = new List<ConductorDto>();

            _mockConductorRepository.Setup(x => x.GetAllConductors()).ReturnsAsync(conductors);
            _mockMapper.Setup(x => x.Map<IEnumerable<ConductorDto>>(conductors)).Returns(conductorDtos);

            // Act
            var result = await _conductorService.GetAllConductors();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ConductorDto>>(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Test to check if GetAllConductors returns an error when the repository fails
        /// </summary>
        [Fact]
        public async Task GetAllConductors_WheRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockConductorRepository.Setup(x => x.GetAllConductors()).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _conductorService.GetAllConductors());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Conductor by Id with existing id returns conductor
        /// </summary>
        [Fact]
        public async Task GetConductorById_WithExistingId_ReturnsConductor()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Conductor 1"
            };
            var conductorDto = new ConductorDto
            {
                Id = 1,
                Name = "Conductor 1"
            };

            _mockConductorRepository.Setup(x => x.GetConductorById(1)).ReturnsAsync(conductor);
            _mockMapper.Setup(x => x.Map<ConductorDto>(conductor)).Returns(conductorDto);

            // Act
            var result = await _conductorService.GetConductorById(1);

            Assert.NotNull(result);
            Assert.IsType<ConductorDto>(result);
            Assert.Equal(1, result.Id);
        }

        /// <summary>
        /// Test Get Conductor by Id with non existing id returns null
        /// </summary>
        [Fact]
        public async Task GetConductorById_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var conductorId = 1;
            _mockConductorRepository.Setup(x => x.GetConductorById(conductorId)).ReturnsAsync(() => null!);

            // Act
            var result = await _conductorService.GetConductorById(conductorId);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Conductor by Id returns error when repository fails
        /// </summary>
        [Fact]
        public async Task GetConductorById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockConductorRepository.Setup(x => x.GetConductorById(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _conductorService.GetConductorById(1));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Conductor by Name with existing name returns conductor
        /// </summary>
        [Fact]
        public async Task GetConductorByName_WithExistingName_ReturnsConductor()
        {
            var conductorId = 1;
            var conductor = new Conductor
            {
                Id = conductorId,
                Name = "Conductor 1"
            };
            var conductorDto = new ConductorDto
            {
                Id = conductorId,
                Name = "Conductor 1"
            };

            _mockConductorRepository.Setup(x => x.GetConductorByName("Conductor 1")).ReturnsAsync(conductor);
            _mockMapper.Setup(x => x.Map<ConductorDto>(conductor)).Returns(conductorDto);

            // Act
            var result = await _conductorService.GetConductorByName("Conductor 1");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ConductorDto>(result);
            Assert.Equal(conductorId, result.Id);
        }

        /// <summary>
        /// Test Get Conductor by Name with non existing name returns null
        /// </summary>
        [Fact]
        public async Task GetConductorByName_WithNonExistingName_ReturnsNull()
        {
            // Arrange
            _mockConductorRepository.Setup(x => x.GetConductorByName("Conductor 1")).ReturnsAsync(() => null!);
            
            // Act
            var result = await _conductorService.GetConductorByName("Conductor 1");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Conductor by Name returns error when repository fails
        /// </summary>
        [Fact]
        public async Task GetConductorByName_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockConductorRepository.Setup(x => x.GetConductorByName("Conductor 1")).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _conductorService.GetConductorByName("Conductor 1"));
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
