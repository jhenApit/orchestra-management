

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Services.Sections;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class SectionsControllerTests
    {
        private readonly Mock<ILogger<SectionsController>> _loggerMock;
        private readonly Mock<ISectionService> _sectionServiceMock;
        private readonly SectionsController _sectionsController;

        public SectionsControllerTests()
        {
            _loggerMock = new Mock<ILogger<SectionsController>>();
            _sectionServiceMock = new Mock<ISectionService>();
            _sectionsController = new SectionsController(_loggerMock.Object, _sectionServiceMock.Object);
        }

        [Fact]
        public async Task GetAllSections_SectionsExist_ReturnsOkResult()
        {
            // Arrange
            var sections = new List<SectionDto>
        {
            new SectionDto { Id = 1, Name = "Section A" },
            new SectionDto { Id = 2, Name = "Section B" }
        };
            _sectionServiceMock.Setup(x => x.GetAllSections()).ReturnsAsync(sections);

            // Act
            var result = await _sectionsController.GetAllSections();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(sections, okResult!.Value);
        }

        [Fact]
        public async Task GetAllSections_NoSectionsExist_ReturnsNoContentResult()
        {
            // Arrange
            var emptySections = new List<SectionDto>();
            _sectionServiceMock.Setup(x => x.GetAllSections()).ReturnsAsync(emptySections);

            // Act
            var result = await _sectionsController.GetAllSections();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllSections_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var exceptionMessage = "An error occurred.";
            _sectionServiceMock.Setup(x => x.GetAllSections()).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _sectionsController.GetAllSections();

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult!.StatusCode);
            Assert.Equal(exceptionMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetSectionById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1; // An invalid ID
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(false);

            // Act
            var result = await _sectionsController.GetSectionById(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetSectionById_SectionNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1; // An existing ID, but section does not exist
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ReturnsAsync((SectionDto)null!);

            // Act
            var result = await _sectionsController.GetSectionById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetSectionById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1; // An existing ID
            var section = new SectionDto { Id = 1, Name = "Section 1" };
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ReturnsAsync(section);

            // Act
            var result = await _sectionsController.GetSectionById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetSectionById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1; // An existing ID
            var exceptionMessage = "Something went wrong.";
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _sectionsController.GetSectionById(id);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult!.StatusCode);
            Assert.Equal(exceptionMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetLeaderboardsBySection_IdValid_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var section = new SectionDto
            {
                Id = 1,
                Name = "Section 1"
            };
            var leaderboards = new List<PlayerDto>
    {
        new PlayerDto { Id = 1, Name = "Player 1" },
        new PlayerDto { Id = 2, Name = "Player 2" }
    };

            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ReturnsAsync(section);
            _sectionServiceMock.Setup(x => x.GetLeaderboardsBySection(id)).ReturnsAsync(leaderboards);

            // Act
            var result = await _sectionsController.GetLeaderboardsBySection(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLeaderboardsBySection_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1;
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(false);

            // Act
            var result = await _sectionsController.GetLeaderboardsBySection(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetLeaderboardsBySection_SectionNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ReturnsAsync((SectionDto)null!);

            // Act
            var result = await _sectionsController.GetLeaderboardsBySection(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetLeaderboardsBySection_NoLeaderboards_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var section = new SectionDto
            {
                Id = 1,
                Name = "Section 1"
            };

            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ReturnsAsync(section);
            _sectionServiceMock.Setup(x => x.GetLeaderboardsBySection(id)).ReturnsAsync(new List<PlayerDto>());

            // Act
            var result = await _sectionsController.GetLeaderboardsBySection(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetLeaderboardsBySection_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var errorMessage = "Something went wrong";
            _sectionServiceMock.Setup(x => x.IsIdValid(id)).Returns(true);
            _sectionServiceMock.Setup(x => x.GetSectionById(id)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _sectionsController.GetLeaderboardsBySection(id);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(errorMessage, objectResult.Value);
        }

    }
}
