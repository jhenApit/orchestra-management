using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Services.Instruments;
using System;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class InstrumentsControllerTests
    {
        private readonly Mock<ILogger<InstrumentsController>> _mockLogger;
        private readonly Mock<IInstrumentService> _mockInstrumentService;
        private readonly InstrumentsController _controller;

        public InstrumentsControllerTests()
        {
            _mockLogger = new Mock<ILogger<InstrumentsController>>();
            _mockInstrumentService = new Mock<IInstrumentService>();
            _controller = new InstrumentsController(
                _mockLogger.Object,
                _mockInstrumentService.Object);
        }

        [Fact]
        public async Task GetInstrumentById_InstrumentExists_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var instrument = new InstrumentDto
            {
                Id = 1,
                Name = "Violin"
            };
            _mockInstrumentService.Setup(x => x.IsIdValid(id)).Returns(true);
            _mockInstrumentService.Setup(x => x.GetInstrumentById(id)).ReturnsAsync(instrument);

            // Act
            var result = await _controller.GetInstrumentById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetInstrumentById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1; // Invalid ID
            _mockInstrumentService.Setup(x => x.IsIdValid(id)).Returns(false);

            // Act
            var result = await _controller.GetInstrumentById(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal("Invalid ID. ID must be a positive numeric value.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetInstrumentById_InstrumentNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _mockInstrumentService.Setup(x => x.IsIdValid(id)).Returns(true);
            _mockInstrumentService.Setup(x => x.GetInstrumentById(id)).ReturnsAsync((InstrumentDto)null!);

            // Act
            var result = await _controller.GetInstrumentById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Instrument with ID {id} does not exist.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetInstrumentById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            _mockInstrumentService.Setup(x => x.IsIdValid(id)).Returns(true);
            _mockInstrumentService.Setup(x => x.GetInstrumentById(id)).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await _controller.GetInstrumentById(id);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Something went wrong", objectResult.Value);
        }

        [Fact]
        public async Task GetInstrumentByName_InstrumentExists_ReturnsOkResult()
        {
            // Arrange
            var name = "Violin";
            var instrument = new InstrumentDto
            {
                Id = 1,
                Name = name
            };
            _mockInstrumentService.Setup(x => x.GetInstrumentByName(name)).ReturnsAsync(instrument);

            // Act
            var result = await _controller.GetInstrumentByName(name);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetInstrumentByName_InstrumentNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var name = "NonexistentInstrument";
            _mockInstrumentService.Setup(x => x.GetInstrumentByName(name)).ReturnsAsync((InstrumentDto)null!);

            // Act
            var result = await _controller.GetInstrumentByName(name);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Instrument {name} does not exist.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetInstrumentByName_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var name = "Violin";
            var exceptionMessage = "Something went wrong.";
            _mockInstrumentService.Setup(x => x.GetInstrumentByName(name)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetInstrumentByName(name);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(exceptionMessage, objectResult.Value);
        }
    }
}
