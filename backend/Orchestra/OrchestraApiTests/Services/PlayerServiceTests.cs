using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Players;
using OrchestraAPI.Repositories.Users;
using OrchestraAPI.Services.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IPlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _playerService = new PlayerService(_mockPlayerRepository.Object, _mockUserRepository.Object, _mockMapper.Object);
        }


        /// <summary>
        /// Test Add Player with existing user id
        /// </summary>
        [Fact]
        public async Task AddPlayer_ExistingUserId_ReturnsPlayer()
        {
            // Arrange
            var playerCreationDto = new PlayerCreationDto
            {
                Name = "Test Name",
                UserId = 1
            };

            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };

            var user = new User
            {
                Id = 1,
                Username = "TestName",
                Email = "email@gmail.com",
                Password = "password"
            };

            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };

            _mockMapper.Setup(m => m.Map<Player>(playerCreationDto)).Returns(player);
            _mockUserRepository.Setup(p => p.GetUserById(1)).ReturnsAsync(user);
            _mockPlayerRepository.Setup(p => p.AddPlayer(player)).ReturnsAsync(1);
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(It.IsAny<Player>())).Returns(playerDto);

            // Act
            var result = await _playerService.AddPlayer(playerCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(playerDto.Id, result.Id);
            Assert.Equal(playerDto.Name, result.Name);
            Assert.Equal(playerDto.UserId, result.UserId);
        }

        /// <summary>
        /// Test Add Player with non existing user id
        /// </summary>
        [Fact]
        public async Task AddPlayer_NonExistingUserId_ReturnsNull()
        {
            // Arrange
            var playerCreationDto = new PlayerCreationDto();
            var player = new Player();
            var user = new User();
            var playerDto = new PlayerDto();

            _mockMapper.Setup(m => m.Map<Player>(playerCreationDto)).Returns((Player)null!);
            _mockUserRepository.Setup(p => p.GetUserById(1)).ReturnsAsync((User)null!);
            _mockPlayerRepository.Setup(p => p.AddPlayer(player)).ReturnsAsync(null);
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync((Player)null!);

            // Act
            var result = await _playerService.AddPlayer(playerCreationDto);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Add Player when repository fails
        /// </summary>
        [Fact]
        public async Task AddPlayer_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var playerCreationDto = new PlayerCreationDto();

            _mockUserRepository.Setup(p => p.GetUserById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockPlayerRepository.Setup(p => p.AddPlayer(It.IsAny<Player>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.AddPlayer(playerCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Delete player with existing id
        /// </summary>
        [Fact]
        public async Task DeletePlayer_ExistingId_ReturnsTrue()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerRepository.Setup(p => p.DeletePlayer(player.Id)).ReturnsAsync(true);

            // Act
            var result = await _playerService.DeletePlayer(1);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Delete player with non existing id
        /// </summary>
        [Fact]
        public async Task DeletePlayer_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync((Player)null!);
            _mockPlayerRepository.Setup(p => p.DeletePlayer(player.Id)).ReturnsAsync(false);

            // Act
            var result = await _playerService.DeletePlayer(1);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Delete player when repository fails
        /// </summary>
        [Fact]
        public async Task DeletePlayer_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockPlayerRepository.Setup(p => p.DeletePlayer(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.DeletePlayer(1));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get All Players when called returns players
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_WhenCalled_ReturnsPlayers()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Name = "Test Name",
                    UserId = 1
                },
                new Player
                {
                    Id = 2,
                    Name = "Test Name 2",
                    UserId = 1
                }
            };
            var playerDtos = new List<PlayerDto>
            {
                new PlayerDto
                {
                    Id = 1,
                    Name = "Test Name",
                    UserId = 1
                },
                new PlayerDto
                {
                    Id = 2,
                    Name = "Test Name 2",
                    UserId = 1
                }
            };
            _mockPlayerRepository.Setup(p => p.GetAllPlayers()).ReturnsAsync(players);
            _mockMapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(players)).Returns(playerDtos);
            
            // Act
            var result = await _playerService.GetAllPlayers();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        /// <summary>
        /// Test Get All Players when called returns empty list
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_EmptyList_ReturnsEmptyPlayers()
        {
            // Arrange
            var players = new List<Player>();
            var playerDtos = new List<PlayerDto>();
            _mockPlayerRepository.Setup(p => p.GetAllPlayers()).ReturnsAsync(players);
            _mockMapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(players)).Returns(playerDtos);
            
            // Act
            var result = await _playerService.GetAllPlayers();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Test Get All Players when repository fails
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockPlayerRepository.Setup(p => p.GetAllPlayers()).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.GetAllPlayers());
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get Player By Id when existing id returns player
        /// </summary>
        [Fact]
        public async Task GetPlayerById_ExistingId_ReturnsPlayer()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(player)).Returns(playerDto);
            
            // Act
            var result = await _playerService.GetPlayerById(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        /// <summary>
        /// Test Get Player By Id when non existing id returns null
        /// </summary>
        [Fact]
        public async Task GetPlayerById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync((Player)null!);
            
            // Act
            var result = await _playerService.GetPlayerById(1);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get Player By Id when repository fails
        /// </summary>
        [Fact]
        public async Task GetPlayerById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.GetPlayerById(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Update Player when existing player returns true
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_ExistingPlayer_ReturnsTrue()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };

            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerRepository.Setup(p => p.UpdatePlayer(player.Id, It.IsAny<Player>())).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<Player>(playerDto)).Returns(player);

            // Act
            var result = await _playerService.UpdatePlayer(player.Id, playerUpdateDto);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Update Player when non existing player returns false
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_NonExistingPlayer_ReturnsFalse()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync((Player)null!);
            _mockPlayerRepository.Setup(p => p.UpdatePlayer(player.Id, It.IsAny<Player>())).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<Player>(playerDto)).Returns(player);
            // Act
            var result = await _playerService.UpdatePlayer(player.Id, playerUpdateDto);
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test update player when repository fails
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };
            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerRepository.Setup(p => p.UpdatePlayer(player.Id, It.IsAny<Player>())).ThrowsAsync(new Exception("Database connection error"));
            _mockMapper.Setup(m => m.Map<Player>(playerDto)).Returns(player);
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.UpdatePlayer(player.Id, playerUpdateDto));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Update Player Score when existing player returns player
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_ExistingId_ReturnsPlayer()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };

            var playerScoreUpdationDto = new PlayerScoreUpdateDto
            {
                Score = 10
            };

            var playerReturn = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 10
            };

            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerRepository.Setup(p => p.UpdatePlayerScore(player.Id, It.IsAny<Player>())).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(player)).Returns(playerReturn);

            // Act
            var result = await _playerService.UpdatePlayerScore(player.Id, playerScoreUpdationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Score);
        }

        /// <summary>
        /// Test Update Player Score when non existing player returns null
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_NonExistingId_ReturnsNull()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };

            var playerScoreUpdationDto = new PlayerScoreUpdateDto
            {
                Score = 10
            };

            var playerReturn = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 10
            };

            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerRepository.Setup(p => p.UpdatePlayerScore(player.Id, It.IsAny<Player>())).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(player)).Returns((PlayerDto)null!);

            // Act
            var result = await _playerService.UpdatePlayerScore(player.Id, playerScoreUpdationDto);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Update Player Score when repository fails
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerDto = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 0
            };
            var playerUpdateDto = new PlayerUpdationDto
            {
                Name = "Updated Name",
            };

            var playerScoreUpdationDto = new PlayerScoreUpdateDto
            {
                Score = 10
            };

            var playerReturn = new PlayerDto
            {
                Id = 1,
                Name = "Test Name",
                UserId = 1,
                Score = 10
            };

            _mockPlayerRepository.Setup(p => p.GetPlayerById(1)).ThrowsAsync(new Exception("Database connection error"));
            _mockPlayerRepository.Setup(p => p.UpdatePlayerScore(1, It.IsAny<Player>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _playerService.UpdatePlayerScore(player.Id, playerScoreUpdationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);

        }

    }
}

