using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Conductors;
using OrchestraAPI.Repositories.Orchestras;
using OrchestraAPI.Services.Orchestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class OrchestraServiceTests
    {
        private readonly Mock<IOrchestraRepository> _mockOrchestraRepository;
        private readonly Mock<IConductorRepository> _mockConductorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrchestraService _orchestraService;

        public OrchestraServiceTests()
        {
            _mockOrchestraRepository = new Mock<IOrchestraRepository>();
            _mockConductorRepository = new Mock<IConductorRepository>();
            _mockMapper = new Mock<IMapper>();
            _orchestraService = new OrchestraService(_mockOrchestraRepository.Object, _mockConductorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddOrchestra_WithValidOrchestra_ReturnsOrchestraDto()
        {
            // Arrange
            var orchestra = new OrchestraCreationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                ConductorId = 1
            };
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestraModel = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            _mockConductorRepository.Setup(x => x.GetConductorById(orchestra.ConductorId)).ReturnsAsync(conductor);
            _mockOrchestraRepository.Setup(x => x.AddOrchestra(It.IsAny<Orchestra>())).ReturnsAsync(1);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns(orchestraDto);
            
            // Act
            var result = await _orchestraService.AddOrchestra(orchestra);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrchestraDto>(result);
            Assert.Equal(orchestraDto.Id, result.Id);
            Assert.Equal(orchestraDto.Name, result.Name);
            Assert.Equal(orchestraDto.Image, result.Image);
            Assert.Equal(orchestraDto.Date, result.Date);
            Assert.Equal(orchestraDto.Conductor, result.Conductor);
        }

        /// <summary>
        /// Test Add Orchestra with invalid conductor id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddOrchestra_NotExistingId_ReturnsNull()
        {
            // Arrange
            var orchestra = new OrchestraCreationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                ConductorId = 1
            };
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestraModel = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            _mockConductorRepository.Setup(x => x.GetConductorById(orchestra.ConductorId)).ReturnsAsync((Conductor)null!);
            _mockOrchestraRepository.Setup(x => x.AddOrchestra(It.IsAny<Orchestra>())).ReturnsAsync(0);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns((OrchestraDto)null!);

            // Act
            var result = await _orchestraService.AddOrchestra(orchestra);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Add Orchestra when repository fails
        /// </summary>
        [Fact]
        public async Task AddOrchestra_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var orchestra = new OrchestraDto();
            var conductor = new Conductor();
            var orchestraModel = new Orchestra();
            var orchestraDto = new OrchestraCreationDto();

            _mockConductorRepository.Setup(x => x.GetConductorById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockOrchestraRepository.Setup(x => x.AddOrchestra(It.IsAny<Orchestra>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.AddOrchestra(orchestraDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Delete Orchestra with existing id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteOrchestra_ExistingId_SuccessfulDeletion()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestra.Id)).ReturnsAsync(orchestra);
            _mockOrchestraRepository.Setup(x => x.DeleteOrchestra(orchestra.Id)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns(orchestraDto);
            
            // Act
            var result = await _orchestraService.DeleteOrchestra(orchestra.Id);
            
            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Delete Orchestra with non-existing id
        /// </summary>
        [Fact]
        public async Task DeleteOrchestra_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestra = new Orchestra();
            var orchestraDto = new OrchestraDto();

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestra.Id)).ReturnsAsync((Orchestra)null!);
            _mockOrchestraRepository.Setup(x => x.DeleteOrchestra(orchestra.Id)).ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns((OrchestraDto)null!);
            
            // Act
            var result = await _orchestraService.DeleteOrchestra(orchestra.Id);
            
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Delete Orchestra when repository fails
        /// </summary>
        [Fact]
        public async Task DeleteOrchestra_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestra = new Orchestra();
            var orchestraDto = new OrchestraDto();
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockOrchestraRepository.Setup(x => x.DeleteOrchestra(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.DeleteOrchestra(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get All Orchestras returns all orchestras
        /// </summary>
        [Fact]
        public async Task GetAllOrchestras_WhenCalled_ReturnsOrchestras()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            var orchestras = new List<Orchestra> { orchestra };
            var orchestraDtos = new List<OrchestraDto> { orchestraDto };
            _mockOrchestraRepository.Setup(x => x.GetAllOrchestras()).ReturnsAsync(orchestras);
            _mockMapper.Setup(x => x.Map<IEnumerable<OrchestraDto>>(It.IsAny<IEnumerable<Orchestra>>())).Returns(orchestraDtos);
            
            // Act
            var result = await _orchestraService.GetAllOrchestras();
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<OrchestraDto>>(result);
            Assert.Equal(orchestraDtos.Count, result.Count());
        }

        /// <summary>
        /// Test Get All Orchestras returns empty list
        /// </summary>
        [Fact]
        public async Task GetAllOrchestras_WhenCalled_ReturnsEmptyList()
        {
            // Arrange
            var orchestras = new List<Orchestra>();
            var orchestraDtos = new List<OrchestraDto>();
            _mockOrchestraRepository.Setup(x => x.GetAllOrchestras()).ReturnsAsync(orchestras);
            _mockMapper.Setup(x => x.Map<IEnumerable<OrchestraDto>>(It.IsAny<IEnumerable<Orchestra>>())).Returns(orchestraDtos);
            
            // Act
            var result = await _orchestraService.GetAllOrchestras();
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<OrchestraDto>>(result);
            Assert.Empty(result);
        }


        /// <summary>
        /// Test Get All Orchestras when repository fails
        /// </summary>
        [Fact]
        public async Task GetAllOrchestras_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockOrchestraRepository.Setup(x => x.GetAllOrchestras()).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.GetAllOrchestras());
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Orchestra By Id with existing id
        /// </summary>
        [Fact]
        public async Task GetOrchestraById_ExistingId_ReturnsOrchestra()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestra.Id)).ReturnsAsync(orchestra);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns(orchestraDto);
            
            // Act
            var result = await _orchestraService.GetOrchestraById(orchestra.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrchestraDto>(result);
            Assert.Equal(orchestraDto.Id, result.Id);
            Assert.Equal(orchestraDto.Name, result.Name);
            Assert.Equal(orchestraDto.Image, result.Image);
            Assert.Equal(orchestraDto.Date, result.Date);
            Assert.Equal(orchestraDto.Conductor, result.Conductor);
        }


        /// <summary>
        /// Test Get Orchestra By Id with non-existing id
        /// </summary>
        [Fact]
        public async Task GetOrchestraById_NonExistingID_RetunsNull()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestra.Id)).ReturnsAsync((Orchestra)null!);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns((OrchestraDto)null!);
            
            // Act
            var result = await _orchestraService.GetOrchestraById(orchestra.Id);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Orchestra By Id when repository fails
        /// </summary>
        [Fact]
        public async Task GetOrchestraById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.GetOrchestraById(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Orchestra By Name with existing name
        /// </summary>
        [Fact]
        public async Task GetOrchestraByName_ExistingName_ReturnsOrchestra()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };
            _mockOrchestraRepository.Setup(x => x.GetOrchestraByName(orchestra.Name)).ReturnsAsync(orchestra);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns(orchestraDto);
            
            // Act
            var result = await _orchestraService.GetOrchestraByName(orchestra.Name);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrchestraDto>(result);
            Assert.Equal(orchestraDto.Id, result.Id);
            Assert.Equal(orchestraDto.Name, result.Name);
            Assert.Equal(orchestraDto.Image, result.Image);
            Assert.Equal(orchestraDto.Date, result.Date);
            Assert.Equal(orchestraDto.Conductor, result.Conductor);
        }

        /// <summary>
        /// Test Get Orchestra By Name with non-existing name
        /// </summary>
        [Fact]
        public async Task GetOrchestraByName_NonExistingName_ReturnsNull()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestra = new Orchestra();
            var orchestraDto = new OrchestraDto();
            _mockOrchestraRepository.Setup(x => x.GetOrchestraByName("Test")).ReturnsAsync((Orchestra)null!);
            _mockMapper.Setup(x => x.Map<OrchestraDto>(It.IsAny<Orchestra>())).Returns((OrchestraDto)null!);
            
            // Act
            var result = await _orchestraService.GetOrchestraByName(orchestra.Name);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Orchestra By Name when repository fails
        /// </summary>
        [Fact]
        public async Task GetOrchestraByName_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockOrchestraRepository.Setup(x => x.GetOrchestraByName("Test")).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.GetOrchestraByName("Test"));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Orcchestras By Player Id with existing id
        /// </summary>
        [Fact]
        public async Task GetOrchestrasByPlayerId_ExistingId_ReturnsOrchestras()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestras = new List<Orchestra>
            {
                new Orchestra
                {
                    Id = 1,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Date = DateTime.Now,
                    Conductor = conductor
                },
                new Orchestra
                {
                    Id = 2,
                    Name = "Test Orchestra 2",
                    Image = "Test Image 2",
                    Date = DateTime.Now,
                    Conductor = conductor
                }
                
            };
            var orchestrasDto = new List<OrchestraDto>
            {
                new OrchestraDto
                {
                    Id = 1,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Date = DateTime.Now,
                    Conductor = "Test Conductor"
                },
                new OrchestraDto
                {
                    Id = 2,
                    Name = "Test Orchestra 2",
                    Image = "Test Image 2",
                    Date = DateTime.Now,
                    Conductor = "Test Conductor"
                }

            };
            _mockOrchestraRepository.Setup(x => x.GetOrchestrasByPlayerId(1)).ReturnsAsync(orchestras);
            _mockMapper.Setup(x => x.Map<IEnumerable<OrchestraDto>>(It.IsAny<IEnumerable<Orchestra>>())).Returns(orchestrasDto);
            
            // Act
            var result = await _orchestraService.GetOrchestrasByPlayerId(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<OrchestraDto>>(result);
            Assert.Equal(orchestras.Count, result.Count());
        }

        /// <summary>
        /// Test Get Orchestras by player id with non-existing id
        /// </summary>
        [Fact]
        public async Task GetOrchestrasByPlayerId_NonExistingId_ReturnsNull()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestras = new List<Orchestra>();
            var orchestrasDto = new List<OrchestraDto>();
            _mockOrchestraRepository.Setup(x => x.GetOrchestrasByPlayerId(1)).ReturnsAsync((List<Orchestra>)null!);
            _mockMapper.Setup(x => x.Map<IEnumerable<OrchestraDto>>(It.IsAny<IEnumerable<Orchestra>>())).Returns((List<OrchestraDto>)null!);
            
            // Act
            var result = await _orchestraService.GetOrchestrasByPlayerId(1);
            
            // Assert
            Assert.Null(result);
        }
        
        /// <summary>
        /// Test Get Orchestras by player id when repository fails
        /// </summary>
        [Fact]
        public async Task GetOrchestrasByPlayerId_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockOrchestraRepository.Setup(x => x.GetOrchestrasByPlayerId(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.GetOrchestrasByPlayerId(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Update Orchestra with existing id
        /// </summary>
        [Fact]
        public async Task UpdateOrchestra_ExistingId_ReturnsTrue()
        {
            // Arrange
            var conductor = new Conductor
            {
                Id = 1,
                Name = "Test Conductor"
            };
            var orchestra = new Orchestra
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor
            };
            var orchestraDto = new OrchestraDto
            {
                Id = 1,
                Name = "Test Orchestra",
                Image = "Test Image",
                Date = DateTime.Now,
                Conductor = conductor.Name
            };
            var orchestraUpdationDto = new OrchestraUpdationDto
            {
                Name = "Updated Orchestra",
                Image = "Updated Image"
            };
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestra.Id)).ReturnsAsync(orchestra);
            _mockConductorRepository.Setup(x => x.GetConductorById(It.IsAny<int>())).ReturnsAsync(conductor);
            _mockOrchestraRepository.Setup(x => x.UpdateOrchestra(orchestra.Id, It.IsAny<Orchestra>())).ReturnsAsync(true);

            // Act
            var result = await _orchestraService.UpdateOrchestra(orchestra.Id, orchestraUpdationDto);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Update Orchestra with non-existing id
        /// </summary>
        [Fact]
        public async Task UpdateOrchestra_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestra = new Orchestra();
            var orchestraDto = new OrchestraDto();
            var orchestraUpdationDto = new OrchestraUpdationDto();
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(1)).ReturnsAsync((Orchestra)null!);
            _mockConductorRepository.Setup(x => x.GetConductorById(It.IsAny<int>())).ReturnsAsync((Conductor)null!);
            _mockOrchestraRepository.Setup(x => x.UpdateOrchestra(1, It.IsAny<Orchestra>())).ReturnsAsync(false);
            
            // Act
            var result = await _orchestraService.UpdateOrchestra(1, orchestraUpdationDto);
            
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Update Orchestra when repository fails
        /// </summary>
        [Fact]
        public async Task UpdateOrchestra_WhenRepositoryFail_ReturnsError()
        {
            // Arrange
            var conductor = new Conductor();
            var orchestra = new Orchestra();
            var orchestraDto = new OrchestraDto();
            var orchestraUpdationDto = new OrchestraUpdationDto();

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockConductorRepository.Setup(x => x.GetConductorById(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection error"));
            _mockOrchestraRepository.Setup(x => x.UpdateOrchestra(1, It.IsAny<Orchestra>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _orchestraService.UpdateOrchestra(1, orchestraUpdationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
