using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Sections;
using OrchestraAPI.Services.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class SectionServiceTests
    {
        private readonly Mock<ISectionRepository> _mockSectionRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SectionService _sectionService;

        public SectionServiceTests()
        {
            _mockSectionRepository = new Mock<ISectionRepository>();
            _mapperMock = new Mock<IMapper>();
            _sectionService = new SectionService(_mockSectionRepository.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Test Get All sections method to verify that it returns all sections
        /// </summary>
        [Fact]
        public async Task GetAllSections_WhenCalled_ReturnsAllSections()
        {
            // Arrange
            var sectionModel = new List<Section>
            {
                new Section
                {
                    Id = 1,
                    Name = "Violin"
                },
                new Section
                {
                    Id = 2,
                    Name = "Viola"
                }
            };

            var sectionDto = new List<SectionDto>
            {
                new SectionDto
                {
                    Id = 1,
                    Name = "Violin"
                },
                new SectionDto
                {
                    Id = 2,
                    Name = "Viola"
                }
            };

            _mockSectionRepository.Setup(repo => repo.GetAllSections()).ReturnsAsync(sectionModel);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<SectionDto>>(sectionModel)).Returns(sectionDto);

            // Act
            var result = await _sectionService.GetAllSections();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<SectionDto>>(result);
            Assert.Equal(2, result.Count());
        }

        /// <summary>
        /// Test Get All sections throws exception when repository fails
        /// </summary>
        [Fact]
        public async Task GetAllSections_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetAllSections()).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _sectionService.GetAllSections());

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Leaderboards by section method to verify that it returns all players in a section
        /// </summary>
        [Fact]
        public async Task GetLeaderboardsBySection_ValidId_ReturnsPlayers()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Name = "Strings"
            };

            var players = new List<PlayerDto>
            {
                new PlayerDto
                {
                    Id = 1,
                    Name = "John Doe",
                    Instrument = "Viola",
                    Section = "Strings",
                    Concert = "Met Gala",
                    Score = 100
                },
                new PlayerDto
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Instrument = "Violin",
                    Section = "Strings",
                    Concert = "Met Gala",
                    Score = 100
                },
                new PlayerDto
                {
                    Id = 3,
                    Name = "John Smith",
                    Instrument = "Violin",
                    Section = "Strings",
                    Concert = "Met Gala",
                    Score = 100
                }
            };

                var playerModels = new List<Player>
                {
                    new Player
                    {
                        Id = 1,
                        Name = "John Doe",
                        Instrument = "Viola",
                        Section = "Strings",
                        Concert = "Met Gala",
                        Score = 100
                    },
                    new Player
                    {
                        Id = 2,
                        Name = "Jane Doe",
                        Instrument = "Violin",
                        Section = "Strings",
                        Concert = "Met Gala",
                        Score = 100
                    },
                    new Player
                    {
                        Id = 3,
                        Name = "John Smith",
                        Instrument = "Violin",
                        Section = "Strings",
                        Concert = "Met Gala",
                        Score = 100
                    }
                };

            _mockSectionRepository.Setup(repo => repo.GetSectionById(section.Id)).ReturnsAsync(section);
            _mockSectionRepository.Setup(repo => repo.GetLeaderboardsBySection(section.Id)).ReturnsAsync(playerModels);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PlayerDto>>(playerModels)).Returns(players);

            // Act
            var result = await _sectionService.GetLeaderboardsBySection(section.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlayerDto>>(result);
            Assert.Equal(3, result.Count());
        }

        /// <summary>
        /// Test Get Leaderboards by section method throws exception when repository fails
        /// </summary>
        [Fact]
        public async Task GetLeaderboardsBySection_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetSectionById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockSectionRepository.Setup(repo => repo.GetLeaderboardsBySection(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _sectionService.GetLeaderboardsBySection(1));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Section by Id method to verify that it returns a section
        /// </summary>
        [Fact]
        public async Task GetSectionById_WithExistingId_ReturnsSection()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Name = "Strings"
            };
            var sectionDto = new SectionDto
            {
                Id = 1,
                Name = "Strings"
            };
            _mockSectionRepository.Setup(repo => repo.GetSectionById(section.Id)).ReturnsAsync(section);
            _mapperMock.Setup(mapper => mapper.Map<SectionDto>(section)).Returns(sectionDto);

            // Act
            var result = await _sectionService.GetSectionById(section.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<SectionDto>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Strings", result.Name);
        }

        /// <summary>
        /// Test Get Section by Id  with non existing Id method to verify that it returns null
        /// </summary>
        [Fact]
        public async Task GetSectionById_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetSectionById(1)).ReturnsAsync((Section)null!);

            // Act
            var result = await _sectionService.GetSectionById(1);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Section by Id method throws exception when repository fails
        /// </summary>
        [Fact]
        public async Task GetSectionById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetSectionById(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _sectionService.GetSectionById(1));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Section by Name method to verify that it returns a section
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetSectionByName_WithExistingName_ReturnsSection()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Name = "Strings"
            };
            var sectionDto = new SectionDto
            {
                Id = 1,
                Name = "Strings"
            };
            _mockSectionRepository.Setup(repo => repo.GetSectionByName(section.Name)).ReturnsAsync(section);
            _mapperMock.Setup(mapper => mapper.Map<SectionDto>(section)).Returns(sectionDto);

            // Act
            var result = await _sectionService.GetSectionByName(section.Name);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SectionDto>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Strings", result.Name);
        }

        /// <summary>
        /// Test Get Section by Name with non existing name method to verify that it returns null
        /// </summary>
        [Fact]
        public async Task GetSectionByName_WithNonExistingName_ReturnsNull()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetSectionByName("Strings")).ReturnsAsync((Section)null!);
            
            // Act
            var result = await _sectionService.GetSectionByName("Strings");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Section by Name method throws exception when repository fails
        /// </summary>
        [Fact]
        public async Task GetSectionByName_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockSectionRepository.Setup(repo => repo.GetSectionByName("Strings")).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _sectionService.GetSectionByName("Strings"));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

    }
}
