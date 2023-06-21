using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Services.Conductors;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class ConductorsControllerTests
    {
        private readonly Mock<ILogger<ConductorsController>> _mockLogger;
        private readonly Mock<IConductorService> _mockConductorService;
        private readonly ConductorsController _controller;

        public ConductorsControllerTests()
        {
            _mockLogger = new Mock<ILogger<ConductorsController>>();
            _mockConductorService = new Mock<IConductorService>();
            _controller = new ConductorsController(
                _mockLogger.Object, 
                _mockConductorService.Object);
        }

        [Fact]
        public async void GetAllConductors_ConductorsExist_ReturnsOkResult()
        {
            // Arrange
            _mockConductorService.Setup(c => c.GetAllConductors()).ReturnsAsync(new List<ConductorDto> { new ConductorDto() });

            // Act
            var result = await _controller.GetAllConductors();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllConductors_NoConductorsExist_ReturnsNoContentResult()
        {
            // Arrange
            _mockConductorService.Setup(c => c.GetAllConductors()).ReturnsAsync(new List<ConductorDto>());

            // Act
            var result = await _controller.GetAllConductors();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetAllConductors_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            _mockConductorService.Setup(c => c.GetAllConductors()).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await _controller.GetAllConductors();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetConductorById_ValidId_ReturnsConductorDto()
        {
            // Arrange
            int validId = 1;
            var conductorDto = new ConductorDto();

            _mockConductorService.Setup(c => c.IsIdValid(validId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(validId)).ReturnsAsync(conductorDto);

            // Act
            var result = await _controller.GetConductorById(validId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(conductorDto, okResult.Value);
        }

        [Fact]
        public async void GetConductorById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;

            _mockConductorService.Setup(c => c.IsIdValid(invalidId)).Returns(false);

            // Act
            var result = await _controller.GetConductorById(invalidId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid ID. ID must be a positive numeric value.", badRequestResult.Value);
        }

        [Fact]
        public async void GetConductorById_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 999;

            _mockConductorService.Setup(c => c.IsIdValid(nonExistentId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(nonExistentId)).ReturnsAsync((ConductorDto)null!);

            // Act
            var result = await _controller.GetConductorById(nonExistentId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Conductor with id 999 does not exist", notFoundResult.Value);
        }

        [Fact]
        public async Task GetConductorById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            int validId = 1;
            var exceptionMessage = "Something went wrong";

            _mockConductorService.Setup(c => c.IsIdValid(validId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(validId)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetConductorById(validId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async void GetConductorByName_ConductorExists_ReturnsOkResult()
        {
            // Arrange
            string conductorName = "John Doe";
            var conductorDto = new ConductorDto { Name = conductorName };
            _mockConductorService.Setup(c => c.GetConductorByName(conductorName)).ReturnsAsync(conductorDto);

            // Act
            var result = await _controller.GetConductorByName(conductorName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(conductorDto, okResult.Value);
        }

        [Fact]
        public async void GetConductorByName_ConductorDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            string conductorName = "Nonexistent Conductor";
            _mockConductorService.Setup(c => c.GetConductorByName(conductorName)).ReturnsAsync((ConductorDto)null!);

            // Act
            var result = await _controller.GetConductorByName(conductorName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal($"Conductor {conductorName} does not exist", notFoundResult.Value);
        }

        [Fact]
        public async void GetConductorByName_ServiceThrowsException_ReturnsInternalServerErrorResult()
        {
            // Arrange
            string conductorName = "John Doe";
            var exceptionMessage = "Something went wrong";
            _mockConductorService.Setup(c => c.GetConductorByName(conductorName)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetConductorByName(conductorName);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(exceptionMessage, internalServerErrorResult.Value);
        }

        [Fact]
        public async void AddConductor_ValidConductor_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var conductorCreationDto = new ConductorCreationDto { Name = "John Doe", UserId = 1 };
            var expectedConductorDto = new ConductorDto { Id = 1, Name = "John Doe", UserId = 1 };

            _mockConductorService.Setup(c => c.AddConductor(conductorCreationDto)).ReturnsAsync(expectedConductorDto);

            // Act
            var result = await _controller.AddConductor(conductorCreationDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetConductorById", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues!["id"]);
            Assert.Equal(expectedConductorDto, createdAtActionResult.Value);
        }

        [Fact]
        public async void AddConductor_InvalidConductor_ReturnsBadRequestResult()
        {
            // Arrange
            var conductorCreationDto = new ConductorCreationDto { Name = null, UserId = 1 };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.AddConductor(conductorCreationDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void AddConductor_ServiceError_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var conductorCreationDto = new ConductorCreationDto { Name = "John Doe", UserId = 1 };
            var errorMessage = "An error occurred while adding the conductor.";

            _mockConductorService.Setup(c => c.AddConductor(conductorCreationDto)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.AddConductor(conductorCreationDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Something went wrong", statusCodeResult.Value);
        }

        [Fact]
        public async Task UpdateConductor_ValidInput_ReturnsOkResult()
        {
            // Arrange
            int conductorId = 1;
            ConductorUpdationDto newConductor = new ConductorUpdationDto { Name = "New Name" };
            _mockConductorService.Setup(c => c.GetConductorById(conductorId)).ReturnsAsync(new ConductorDto());

            // Act
            var result = await _controller.UpdateConductor(conductorId, newConductor);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateConductor_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            int conductorId = 1;
            ConductorUpdationDto newConductor = new ConductorUpdationDto { Name = null };
            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.UpdateConductor(conductorId, newConductor);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateConductor_ConductorNotFound_ReturnsNotFound()
        {
            // Arrange
            int conductorId = 1;
            ConductorUpdationDto newConductor = new ConductorUpdationDto { Name = "New Name" };
            _mockConductorService.Setup(c => c.GetConductorById(conductorId)).ReturnsAsync((ConductorDto)null!);

            // Act
            var result = await _controller.UpdateConductor(conductorId, newConductor);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateConductor_ServiceError_ReturnsInternalServerError()
        {
            // Arrange
            int conductorId = 1;
            ConductorUpdationDto newConductor = new ConductorUpdationDto { Name = "New Name" };
            _mockConductorService.Setup(c => c.GetConductorById(conductorId)).Throws(new Exception("Some error occurred."));

            // Act
            var result = await _controller.UpdateConductor(conductorId, newConductor);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteConductor_ValidId_ReturnsOkResult()
        {
            // Arrange
            int validId = 1;
            _mockConductorService.Setup(c => c.IsIdValid(validId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(validId)).ReturnsAsync(new ConductorDto());

            // Act
            var result = await _controller.DeleteConductor(validId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteConductor_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            _mockConductorService.Setup(c => c.IsIdValid(invalidId)).Returns(false);

            // Act
            var result = await _controller.DeleteConductor(invalidId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteConductor_NonexistentConductor_ReturnsNotFound()
        {
            // Arrange
            int nonexistentId = 999;
            _mockConductorService.Setup(c => c.IsIdValid(nonexistentId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(nonexistentId)).ReturnsAsync((ConductorDto)null!);

            // Act
            var result = await _controller.DeleteConductor(nonexistentId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteConductor_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            int validId = 1;
            _mockConductorService.Setup(c => c.IsIdValid(validId)).Returns(true);
            _mockConductorService.Setup(c => c.GetConductorById(validId)).Throws(new Exception("Something went wrong."));

            // Act
            var result = await _controller.DeleteConductor(validId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
        }
    }
}
