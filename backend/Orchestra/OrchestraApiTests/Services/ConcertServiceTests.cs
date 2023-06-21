using OrchestraAPI.Repositories.Concerts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchestraAPI.Repositories.Orchestras;
using OrchestraAPI.Services.Concerts;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Models;
using AutoMapper;
using OrchestraAPI.Dtos.Orchestra;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class ConcertServiceTests
    {
        private readonly Mock<IConcertRepository> _mockConcertRepository;
        private readonly Mock<IOrchestraRepository> _mockOrchestraRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IConcertService _concertService;

        public ConcertServiceTests()
        {
            _mockConcertRepository = new Mock<IConcertRepository>();
            _mockOrchestraRepository = new Mock<IOrchestraRepository>();
            _mockMapper = new Mock<IMapper>();
            _concertService = new ConcertService(_mockConcertRepository.Object, _mockOrchestraRepository.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Test Get All Concerts returns concerts
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllConcerts_WhenCalled_ReturnsConcerts()
        {
            // Arrange
            var concerts = new List<Concert>
            {
                new Concert
                {
                    Id = 1,
                    Name = "Concert 1",
                    Description = "Concert 1 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 1 Image"
                },
                new Concert
                {
                    Id = 2,
                    Name = "Concert 2",
                    Description = "Concert 2 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 2 Image"
                }
            };
            var concertDtos = new List<ConcertDto>
            {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Concert 1",
                    Description = "Concert 1 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 1 Image"
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Concert 2",
                    Description = "Concert 2 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 2 Image"
                }
            };
            _mockConcertRepository.Setup(repo => repo.GetAllConcerts()).ReturnsAsync(concerts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ConcertDto>>(concerts)).Returns(concertDtos);
            
            // Act
            var result = await _concertService.GetAllConcerts();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllConcerts_EmptyData_ReturnsEmpty()
        {
            // Arrange
            var concerts = new List<Concert>();
            var concertDtos = new List<ConcertDto>();
            _mockConcertRepository.Setup(repo => repo.GetAllConcerts()).ReturnsAsync(concerts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ConcertDto>>(concerts)).Returns(concertDtos);

            // Act
            var result = await _concertService.GetAllConcerts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllConcerts_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var concerts = new List<Concert>();
            var concertDtos = new List<ConcertDto>();
            _mockConcertRepository.Setup(repo => repo.GetAllConcerts()).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.GetAllConcerts());

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Concert By Id returns concert
        /// </summary>
        [Fact]
        public async Task GetConcertById_ExistingConcert_ReturnsConcert()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };

            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };

            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);


            // Act
            var result = await _concertService.GetConcertById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        /// <summary>
        /// Test Get Concert By Id returns null when no concert exists
        /// </summary>
        [Fact]
        public async Task GetConcertById_NoExistingConcert_ReturnsNull()
        {
            // Arrange
            Concert concert = null;
            ConcertDto concertDto = null;
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await _concertService.GetConcertById(1);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Concert By Id returns error when repository fails
        /// </summary>
        [Fact]
        public async Task GetConcertById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.GetConcertById(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Add Concert returns concert
        /// </summary>
        [Fact]
        public async Task  AddConcert_ValidInputs_ReturnsConcert()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertCreationDto
            {
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };

            _mockConcertRepository.Setup(repo => repo.AddConcert(It.IsAny<Concert>())).ReturnsAsync(1);
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);

            // Act
            var result = await _concertService.AddConcert(concertUpdationDto);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        /// <summary>
        /// Test Add Concert returns null when invalid Inputs
        /// </summary>
        [Fact]
        public async Task AddConcert_InvalidInputs_ReturnsNull()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertCreationDto
            {
                Name = "Concert 3",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };

            _mockConcertRepository.Setup(repo => repo.AddConcert(It.IsAny<Concert>())).ReturnsAsync(0);
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await _concertService.AddConcert(concertUpdationDto);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Add Concert returns error when repository fails
        /// </summary>
        [Fact]
        public async Task AddConcert_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertCreationDto
            {
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            _mockConcertRepository.Setup(repo => repo.AddConcert(It.IsAny<Concert>())).ThrowsAsync(new Exception("Database connection error"));
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection error"));
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.AddConcert(concertUpdationDto));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Update concert returns true when concert exists
        /// </summary>
        [Fact]
        public async Task UpdateConcert_ConcertExist_ReturnsTrue()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertUpdateDto
            {
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            _mockConcertRepository.Setup(repo => repo.UpdateConcert(1, It.IsAny<Concert>())).ReturnsAsync(true);
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await _concertService.UpdateConcert(1, concertUpdationDto);
            
            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test oncert Update returns false when concert does not exist
        /// </summary>
        [Fact]
        public async Task UpdateConcert_ConcertNotExist_ReturnsFalse()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertUpdateDto
            {
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            _mockConcertRepository.Setup(repo => repo.UpdateConcert(1, It.IsAny<Concert>())).ReturnsAsync(false);
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ReturnsAsync(concert);
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await _concertService.UpdateConcert(1, concertUpdationDto);
            
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Update Concert returns error when repository fails
        /// </summary>
        [Fact]
        public async Task UpdateConcert_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var concert = new Concert
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image",
                OrchestraId = 1
            };
            var concertUpdationDto = new ConcertUpdateDto
            {
                Name = "Concert 1",
                Description = "Concert 1 Description",
                PerformanceDate = DateTime.Now,
                Image = "Concert 1 Image"
            };
            _mockConcertRepository.Setup(repo => repo.UpdateConcert(1, It.IsAny<Concert>())).ThrowsAsync(new Exception("Database connection error"));
            _mockConcertRepository.Setup(repo => repo.GetConcertById(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection error"));
            _mockMapper.Setup(mapper => mapper.Map<Concert>(concertDto)).Returns(concert);
            _mockMapper.Setup(mapper => mapper.Map<ConcertDto>(concert)).Returns(concertDto);
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.UpdateConcert(1, concertUpdationDto));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Delete Concert returns true when concert exists
        /// </summary>
        [Fact]
        public async Task DeleteConcert_ConcertExist_ReturnsTrue()
        {
            // Arrange
            _mockConcertRepository.Setup(repo => repo.DeleteConcert(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _concertService.DeleteConcert(1);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Delete Concert returns false when concert does not exist
        /// </summary>
        [Fact]
        public async Task DeleteConcert_ConcertNotExist_ReturnsFalse()
        {
            // Arrange
            _mockConcertRepository.Setup(repo => repo.DeleteConcert(It.IsAny<int>())).ReturnsAsync(false);
            
            // Act
            var result = await _concertService.DeleteConcert(1);
            
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Delete Concert returns error when repository fails
        /// </summary>
        [Fact]
        public async Task DeleteConcert_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockConcertRepository.Setup(repo => repo.DeleteConcert(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.DeleteConcert(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Concerts by Player Id returns concerts when player exists
        /// </summary>
        [Fact]
        public async Task GetConcertsByPlayerId_ExistId_ReturnConcerts()
        {
            // Arrange
            var playerId = 1;
            var concerts = new List<Concert>
            {
                new Concert
                {
                    Id = 1,
                    Name = "Concert 1",
                    Description = "Concert 1 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 1 Image",
                    OrchestraId = 1
                },
                new Concert
                {
                    Id = 2,
                    Name = "Concert 2",
                    Description = "Concert 2 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 2 Image",
                    OrchestraId = 1
                }
                };
                var concertsDto = new List<ConcertDto>
                {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Concert 1",
                    Description = "Concert 1 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 1 Image",
                    OrchestraId = 1
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Concert 2",
                    Description = "Concert 2 Description",
                    PerformanceDate = DateTime.Now,
                    Image = "Concert 2 Image",
                    OrchestraId = 1
                }
            };

            _mockConcertRepository.Setup(repo => repo.GetConcertsByPlayerId(playerId)).ReturnsAsync(concerts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ConcertDto>>(concerts)).Returns(concertsDto);

            // Act
            var result = await _concertService.GetConcertsByPlayerId(playerId);

            // Assert
            Assert.Equal(concertsDto, result);
        }

        /// <summary>
        /// Test Get Concerts by Player Id returns empty with no concert data
        /// </summary>
        [Fact]
        public async Task GetConcertsByPlayerId_NoConcert_ReturnsEmpty()
        {
            // Arrange
            var playerId = 1;
            var concerts = new List<Concert>();
            var concertsDto = new List<ConcertDto>();
            _mockConcertRepository.Setup(repo => repo.GetConcertsByPlayerId(playerId)).ReturnsAsync(concerts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ConcertDto>>(concerts)).Returns(concertsDto);
            
            // Act
            var result = await _concertService.GetConcertsByPlayerId(playerId);
            
            // Assert
            Assert.Equal(concertsDto, result);
        }

        /// <summary>
        /// Test Get Concets by Player Id returns error when repository fails
        /// </summary>
        [Fact]
        public async Task GetConcertsByPlayerId_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var playerId = 1;
            _mockConcertRepository.Setup(repo => repo.GetConcertsByPlayerId(playerId)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _concertService.GetConcertsByPlayerId(playerId));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
