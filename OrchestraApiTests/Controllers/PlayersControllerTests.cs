using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Services.Concerts;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Instruments;
using OrchestraAPI.Services.Orchestras;
using OrchestraAPI.Services.Players;
using OrchestraAPI.Services.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class PlayersControllerTests
    {
        private readonly Mock<ILogger<PlayersController>> _mockLogger;
        private readonly Mock<IPlayerService> _mockPlayerService;
        private readonly Mock<IOrchestraService> _mockOrchestraService;
        private readonly Mock<ISectionService> _mockSectionService;
        private readonly Mock<IInstrumentService> _mockInstrumentService;
        private readonly Mock<IEnrollmentService> _mockEnrollmentService;
        private readonly Mock<IConcertService> _mockConcertService;
        private readonly PlayersController _controller;

        public PlayersControllerTests()
        {
            _mockLogger = new Mock<ILogger<PlayersController>>();
            _mockPlayerService = new Mock<IPlayerService>();
            _mockOrchestraService = new Mock<IOrchestraService>();
            _mockSectionService = new Mock<ISectionService>();
            _mockInstrumentService = new Mock<IInstrumentService>();
            _mockEnrollmentService = new Mock<IEnrollmentService>();
            _mockConcertService = new Mock<IConcertService>();
            _controller = new PlayersController(_mockLogger.Object, _mockPlayerService.Object, _mockOrchestraService.Object, _mockSectionService.Object, _mockInstrumentService.Object, _mockEnrollmentService.Object, _mockConcertService.Object);
        }

        /// <summary>
        /// Test Get All players returns Ok result 200
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_Successful_ReturnsOkResult()
        {
            // Arrange
            var players = new List<PlayerDto>
            {
                new PlayerDto
                {
                    Id = 1,
                    Name = "Test",
                    UserId = 2,
                    Section = "Test",
                    Instrument = "Test",
                    Concert = "Test",
                    Score = 1
                },

                new PlayerDto
                {
                    Id = 2,
                    Name = "Test2",
                    UserId = 2,
                    Section = "Test2",
                    Instrument = "Test2",
                    Concert = "Test2",
                    Score = 1
                },
            };

            _mockPlayerService.Setup(x => x.GetAllPlayers()).ReturnsAsync(players);

            // Act
            var result = await _controller.GetAllPlayers();

            // Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Get All Players returns No Content result 204
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_NoPlayers_ReturnsNoContent()
        {
            // Arrange
            var players = new List<PlayerDto>();
            _mockPlayerService.Setup(x => x.GetAllPlayers()).ReturnsAsync(players);
            
            // Act
            var result = await _controller.GetAllPlayers();
            
            // Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Get All Players returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task GetAllPlayers_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var players = new List<PlayerDto>();
            _mockPlayerService.Setup(x => x.GetAllPlayers()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAllPlayers();

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get Player By Id returns Ok result 200
        /// </summary>
        [Fact]
        public async Task GetPlayerById_HasPlayer_ReturnsOkResult()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 1
            };
            _mockPlayerService.Setup(x => x.GetPlayerById(It.IsAny<int>())).ReturnsAsync(player);
           
            // Act
            var result = await _controller.GetPlayerById(It.IsAny<int>());
           
            // Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Get Player By Id returns No Content result 404
        /// </summary>
        [Fact]
        public async Task GetPlayerById_NoPlayer_ReturnsNotfound()
        {
            // Arrange
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null!);
            
            // Act
            var result = await _controller.GetPlayerById(1);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Get Player By Id returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task GetPlayerById_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockPlayerService.Setup(x => x.IsIdValid(It.IsAny<int>())).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(It.IsAny<int>())).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetPlayerById(It.IsAny<int>());

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Create Player returns Bad Request 400
        /// </summary>
        [Fact]
        public async Task GetPlayerById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockPlayerService.Setup(x => x.IsIdValid(It.IsAny<int>())).Returns(false);
            
            // Act
            var result = await _controller.GetPlayerById(It.IsAny<int>());
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Update Player returns Ok result 200
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayer_Successful_ReturnsOkResult()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 1
            };

            var playerUpdate = new PlayerUpdationDto
            {
                Name = "Test",

            };

            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.UpdatePlayer(1, playerUpdate)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdatePlayer(1, playerUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Update Player returns Bad Request 404
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var playerUpdate = new PlayerUpdationDto
            {
                Name = "Test",
            };
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null!);
            
            // Act
            var result = await _controller.UpdatePlayer(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Update Player returns Bad Request 400
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_InvalidDetails_ReturnsBadRequest()
        {
            // Arrange
            var playerId = 1;
            var playerUpdate = new PlayerUpdationDto
            {
                Name = "Test",
            };
            _mockPlayerService.Setup(service => service.GetPlayerById(playerId)).ReturnsAsync((PlayerDto)null!);

            // Act
            var result = await _controller.UpdatePlayer(playerId, playerUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Player with id {playerId} does not exist.", (result as NotFoundObjectResult)?.Value);
        }



        /// <summary>
        /// Test Update Player returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task UpdatePlayer_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 1
            };
            var playerUpdate = new PlayerUpdationDto
            {
                Name = "Test",
            };
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.UpdatePlayer(1, playerUpdate)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.UpdatePlayer(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Delete Player returns Ok result 200
        /// </summary>
        [Fact]
        public async Task DeletePlayer_Successful_ReturnsOkResult()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 1
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.DeletePlayer(1)).ReturnsAsync(true);
            
            // Act
            var result = await _controller.DeletePlayer(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Delete player returns not found 404
        /// </summary>
        [Fact]
        public async Task DeletePlayer_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null!);
            
            // Act
            var result = await _controller.DeletePlayer(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Delete Player invalid id returns bad request 400
        /// </summary>
        [Fact]
        public async Task DeletePlayer_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(false);
            
            // Act
            var result = await _controller.DeletePlayer(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Delete Player returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task DeletePlayer_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 1
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.DeletePlayer(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.DeletePlayer(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Update Player Score returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_Successful_ReturnsOkResult()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var playerUpdate = new PlayerScoreUpdateDto
            {
                Score = 89
            };

            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.UpdatePlayerScore(1, playerUpdate)).ReturnsAsync(player);
            
            // Act
            var result = await _controller.UpdatePlayerScore(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test update player score player not found returns not found 404
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var playerUpdate = new PlayerScoreUpdateDto
            {
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null!);
            
            // Act
            var result = await _controller.UpdatePlayerScore(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test update player score invalid id returns bad request 400
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var playerUpdate = new PlayerScoreUpdateDto
            {
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(false);
            
            // Act
            var result = await _controller.UpdatePlayerScore(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test update player score returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task UpdatePlayerScore_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var playerUpdate = new PlayerScoreUpdateDto
            {
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockPlayerService.Setup(x => x.UpdatePlayerScore(1, playerUpdate)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.UpdatePlayerScore(1, playerUpdate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Add Player returns Created At Route 201
        /// </summary>
        [Fact]
        public async Task AddPlayer_Successful_ReturnsCreatedArRoute()
        {
            // Arrange
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var playerCreate = new PlayerCreationDto
            {
                Name = "Test",
                UserId = 2
            };
            _mockPlayerService.Setup(x => x.AddPlayer(playerCreate)).ReturnsAsync(player);
            
            // Act
            var result = await _controller.AddPlayer(playerCreate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        /// <summary>
        /// Test Add Player returns Bad Request 400
        /// </summary>
        [Fact]
        public async Task AddPlayer_InvalidDetails_ReturnsBadRequest()
        {
            // Arrange
            var playerCreate = new PlayerCreationDto
            {
                Name = "Test",
                UserId = 2
            };
            
            _controller.ModelState.AddModelError("UserId", "UserId not exist");
            _controller.ModelState.AddModelError("Name", "Name is required");
            
            // Act
            var result = await _controller.AddPlayer(playerCreate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Add Player returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task AddPlayer_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var playerCreate = new PlayerCreationDto
            {
                Name = "Test",
                UserId = 2
            };
            _mockPlayerService.Setup(x => x.AddPlayer(playerCreate)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.AddPlayer(playerCreate);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task EnrollPlayerToOrchestra_Successful_ReturnsOkResult()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;

            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };

            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };

            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestraById(1)).ReturnsAsync(new OrchestraDto());
            _mockSectionService.Setup(x => x.GetSectionById(1)).ReturnsAsync(new SectionDto());
            _mockInstrumentService.Setup(x => x.GetInstrumentById(1)).ReturnsAsync(new InstrumentDto());
            _mockEnrollmentService.Setup(x => x.EnrollPlayerToOrchestra(Id, orchestraId, sectionId, instrumentId, enroll.Experience)).ReturnsAsync(true);
            
            // Act
            var result = await _controller.EnrollPlayerToOrchestra(1, orchestraId, sectionId, instrumentId, enroll);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Enroll Player to Orchestra returns Bad Request 400
        /// </summary>
        [Fact]
        public async Task EnrollPlayerToOrchestra_InvalidPlayerId_ReturnsBadRequest()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };

            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(false);
            
            // Act
            var result = await _controller.EnrollPlayerToOrchestra(1, orchestraId, sectionId, instrumentId, enroll);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }


        /// <summary>
        /// Test Enroll Player to Orchestra returns Not Found 404
        /// </summary>
        [Fact]
        public async Task EnrollPlayerToOrchestra_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null);
            
            // Act
            var result = await _controller.EnrollPlayerToOrchestra(1, orchestraId, sectionId, instrumentId, enroll);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Enroll Player to Orchestra returns Server Error 500
        /// </summary>
        [Fact]
        public async Task EnrollPlayerToOrchestra_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestraById(1)).ReturnsAsync(new OrchestraDto());
            _mockSectionService.Setup(x => x.GetSectionById(1)).ReturnsAsync(new SectionDto());
            _mockInstrumentService.Setup(x => x.GetInstrumentById(1)).ReturnsAsync(new InstrumentDto());
            _mockEnrollmentService.Setup(x => x.EnrollPlayerToOrchestra(Id, orchestraId, sectionId, instrumentId, enroll.Experience)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.EnrollPlayerToOrchestra(1, orchestraId, sectionId, instrumentId, enroll);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Accept Enrollee returns Ok 200
        /// </summary>
        [Fact]
        public async Task AcceptEnrollee_Accepted_ReturnsOkResult()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestraById(1)).ReturnsAsync(new OrchestraDto());
            _mockSectionService.Setup(x => x.GetSectionById(1)).ReturnsAsync(new SectionDto());
            _mockInstrumentService.Setup(x => x.GetInstrumentById(1)).ReturnsAsync(new InstrumentDto());
            _mockEnrollmentService.Setup(x => x.AcceptEnrollee(orchestraId, Id)).ReturnsAsync(player);
            
            // Act
            var result = await _controller.AcceptEnrollee(orchestraId, Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Accept Enrollee returns Not Found 404
        /// </summary>
        [Fact]
        public async Task AcceptEnrollee_NoPlayer_ReturnsNotFound()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync((PlayerDto)null!);
            _mockOrchestraService.Setup(x => x.GetOrchestraById(1)).ReturnsAsync(new OrchestraDto());
            _mockSectionService.Setup(x => x.GetSectionById(1)).ReturnsAsync(new SectionDto());
            _mockInstrumentService.Setup(x => x.GetInstrumentById(1)).ReturnsAsync(new InstrumentDto());
            _mockEnrollmentService.Setup(x => x.AcceptEnrollee(orchestraId, Id)).ReturnsAsync(player);
            
            // Act
            var result = await _controller.AcceptEnrollee(orchestraId, Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Accept Enrollee returns Server Error 500
        /// </summary>
        [Fact]
        public async Task AcceptEnrollee_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var Id = 1;
            var orchestraId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var enroll = new EnrollPlayerDto
            {
                Experience = 5
            };
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            _mockPlayerService.Setup(x => x.IsIdValid(1)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(1)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestraById(1)).ReturnsAsync(new OrchestraDto());
            _mockSectionService.Setup(x => x.GetSectionById(1)).ReturnsAsync(new SectionDto());
            _mockInstrumentService.Setup(x => x.GetInstrumentById(1)).ReturnsAsync(new InstrumentDto());
            _mockEnrollmentService.Setup(x => x.AcceptEnrollee(orchestraId, Id)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.AcceptEnrollee(orchestraId, Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get All Concerts By Player Id returns Ok 200
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllConcertsByPlayerId_HasConcerts_ReturnsOkResult()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };

            var concerts = new List<ConcertDto>
            {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 1
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 3
                }
            };

            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockConcertService.Setup(x => x.GetConcertsByPlayerId(Id)).ReturnsAsync(concerts);
            
            // Act
            var result = await _controller.GetAllConcertsByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Concerts By Player Id returns Not Found 404
        /// </summary>
        [Fact]
        public async Task GetAllConcertsByPlayerId_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var concerts = new List<ConcertDto>
            {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 1
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 3
                }
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync((PlayerDto)null!);
            _mockConcertService.Setup(x => x.GetConcertsByPlayerId(Id)).ReturnsAsync(concerts);
            
            // Act
            var result = await _controller.GetAllConcertsByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Concerts By Player Id invalid Id returns Bad Request 400
        /// </summary>
        [Fact]
        public async Task GetAllConcertsByPlayerId_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var Id = -1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var concerts = new List<ConcertDto>
            {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 1
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 3
                }
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(false);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockConcertService.Setup(x => x.GetConcertsByPlayerId(Id)).ReturnsAsync(concerts);
            
            // Act
            var result = await _controller.GetAllConcertsByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Concerts by Player Id returns Server Error 500
        /// </summary>
        [Fact]
        public async Task GetAllConcertsByPlayerId_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var concerts = new List<ConcertDto>
            {
                new ConcertDto
                {
                    Id = 1,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 1
                },
                new ConcertDto
                {
                    Id = 2,
                    Name = "Test",
                    PerformanceDate = DateTime.Now,
                    OrchestraId = 3
                }
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockConcertService.Setup(x => x.GetConcertsByPlayerId(Id)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetAllConcertsByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get All Orchestras By Player Id returns Ok 200
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllOrchestrasByPlayerId_Successful_ReturnsOkResult()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var orchestras = new List<OrchestraDto>
            {
                new OrchestraDto
                {
                    Id = 1,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
                new OrchestraDto
                {
                    Id = 2,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },

            };

            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestrasByPlayerId(Id)).ReturnsAsync(orchestras);

            // Act
            var result = await _controller.GetAllOrchestrasByPlayerId(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Orchestras By Player Id returns Not Found 404
        /// </summary>
        [Fact]
        public async Task GetAllOrchestrasByPlayerId_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var orchestras = new List<OrchestraDto>
            {
                new OrchestraDto
                {
                    Id = 1,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
                new OrchestraDto
                {
                    Id = 2,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync((PlayerDto)null!);
            _mockOrchestraService.Setup(x => x.GetOrchestrasByPlayerId(Id)).ReturnsAsync(orchestras);
            // Act
            var result = await _controller.GetAllOrchestrasByPlayerId(Id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Orchestras By Player Id returns Bad Request 400
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrchestrasByPlayerId_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var Id = -1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var orchestras = new List<OrchestraDto>
            {
                new OrchestraDto
                {
                    Id = 1,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
                new OrchestraDto
                {
                    Id = 2,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(false);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestrasByPlayerId(Id)).ReturnsAsync(orchestras);
            
            // Act
            var result = await _controller.GetAllOrchestrasByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Orchestras By Player Id returns Internal Server Error 500
        /// </summary>
        [Fact]
        public async Task GetAllOrchestrasByPlayerId_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var Id = 1;
            var player = new PlayerDto
            {
                Id = 1,
                Name = "Test",
                UserId = 2,
                Section = "Test",
                Instrument = "Test",
                Concert = "Test",
                Score = 89
            };
            var orchestras = new List<OrchestraDto>
            {
                new OrchestraDto
                {
                    Id = 1,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
                new OrchestraDto
                {
                    Id = 2,
                    Name = "Test",
                    Image = "Test.png",
                    Date = DateTime.Now,
                    Conductor = "Tester",
                    Description = "Test Description"
                },
            };
            _mockPlayerService.Setup(x => x.IsIdValid(Id)).Returns(true);
            _mockPlayerService.Setup(x => x.GetPlayerById(Id)).ReturnsAsync(player);
            _mockOrchestraService.Setup(x => x.GetOrchestrasByPlayerId(Id)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetAllOrchestrasByPlayerId(Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

    }
}
