using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Orchestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class OrchestrasControllerTests
    {
        private readonly Mock<ILogger<OrchestrasController>> _mockLogger;
        private readonly Mock<IOrchestraService> _orchestraServiceMock;
        private readonly Mock<IEnrollmentService> _mockEnrollmentService;
        private readonly OrchestrasController _orchestrasController;

        public OrchestrasControllerTests()
        {
            _mockLogger = new Mock<ILogger<OrchestrasController>>();
            _orchestraServiceMock = new Mock<IOrchestraService>();
            _mockEnrollmentService = new Mock<IEnrollmentService>();
            _orchestrasController = new OrchestrasController(
                _mockLogger.Object,
                _orchestraServiceMock.Object,
                _mockEnrollmentService.Object);
        }

        [Fact]
        public async Task GetAllOrchestras_ReturnsOkResult()
        {
            // Arrange
            _orchestraServiceMock.Setup(x => x.GetAllOrchestras())
                .ReturnsAsync(new List<OrchestraDto> { new OrchestraDto() });

            // Act
            var result = await _orchestrasController.GetAllOrchestras();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllOrchestras_IsNullOrEmpty_NoContent()
        {
            // Arrange
            _orchestraServiceMock.Setup(x => x.GetAllOrchestras())
                .ReturnsAsync(new List<OrchestraDto>());

            // Act
            var result = await _orchestrasController.GetAllOrchestras();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllOrchestras_InternalServerError_ServerError()
        {
            // Arrange
            _orchestraServiceMock.Setup(x => x.GetAllOrchestras())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _orchestrasController.GetAllOrchestras();

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task GetOrchestraById_ConcertExists_ReturnsOkResult()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(c => c.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto { Id = testId });

            // Act
            var result = await _orchestrasController.GetOrchestraById(testId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrchestraById_InvalidIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var testId = -1;

            _orchestrasController.ModelState.AddModelError("Id", "Id must be greater than 0");

            // Act
            var result = await _orchestrasController.GetOrchestraById(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task GetOrchestraById_OrchestraNotFound_ReturnsNotFound()
        {
            // Arrange
            var notFoundId = 10;

            _orchestraServiceMock.Setup(x => x.IsIdValid(notFoundId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(notFoundId))!
                .ReturnsAsync((OrchestraDto)null!);

            // Act
            var result = await _orchestrasController.GetOrchestraById(notFoundId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task GetOrchestraById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.GetOrchestraById(testId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task GetOrchestraByName_OrchestraExists_ReturnsOkResult()
        {
            // Arrange
            var testName = "Test Orchestra";

            _orchestraServiceMock.Setup(x => x.GetOrchestraByName(testName))
                .ReturnsAsync(new OrchestraDto { Name = testName });

            // Act
            var result = await _orchestrasController.GetOrchestraByName(testName);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrchestraByName_OrchestraNotFound_ReturnsNotFound()
        {
            // Arrange
            var notFoundName = "Not Found Orchestra";

            _orchestraServiceMock.Setup(x => x.GetOrchestraByName(notFoundName))
                .ReturnsAsync((OrchestraDto)null!);

            // Act
            var result = await _orchestrasController.GetOrchestraByName(notFoundName);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task GetOrchestraByName_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var testName = "Test Orchestra";

            _orchestraServiceMock.Setup(x => x.GetOrchestraByName(testName))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.GetOrchestraByName(testName);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task DeleteOrchestra_OrchestraExists_ReturnsOkResult()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync( new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Date = DateTime.Now,
                    Conductor = "Test Conductor",
                    Image = "Test Image",
                    Description = "Test Description"
                });

            // Act
            var result = await _orchestrasController.DeleteOrchestra(testId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal($"Orchestra with id {testId} is successfully deleted", okResult.Value);
        }

        [Fact]
        public async Task DeleteOrchestra_InvalidIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var testId = -1;

            _orchestrasController.ModelState.AddModelError("Id", "Id must be greater than 0");

            // Act
            var result = await _orchestrasController.DeleteOrchestra(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteOrchestra_OrchestraNotFound_ReturnsNotFound()
        {
            // Arrange
            var notFoundId = 10;

            _orchestraServiceMock.Setup(x => x.IsIdValid(notFoundId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(notFoundId))
                .ReturnsAsync((OrchestraDto)null!);

            // Act
            var result = await _orchestrasController.DeleteOrchestra(notFoundId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task DeleteOrchestra_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.DeleteOrchestra(testId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task CreateOrchestra_Successful_ReturnsCreatedAtRoute()
        {
            // Arrange
            var orchestraDto = new OrchestraCreationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                Description = "Test Description",
                ConductorId = It.IsAny<int>()
            };

            _orchestraServiceMock.Setup(x => x.AddOrchestra(orchestraDto))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = 1,
                    Name = orchestraDto.Name,
                    Image = orchestraDto.Image,
                    Description = orchestraDto.Description,
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });

            // Act
            var result = await _orchestrasController.CreateOrchestra(orchestraDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(result, createdAtRouteResult);
            Assert.Equal(StatusCodes.Status201Created, createdAtRouteResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrchestra_InvalidModelPassed_ReturnsBadRequest()
        {
            // Arrange
            var orchestraDto = new OrchestraCreationDto
            {
                Name = null,
                Image = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla fermentum eros nec quam sodales, " +
                        "at ornare leo laoreet. Duis dapibus metus nec orci viverra volutpat. Nulla facilisi. Proin rutrum eget nisl non euismod." +
                        " Vivamus non ante at arcu tempor tempus ut et massa. Mauris a commodo odio. Cras pulvinar libero eu aliquam faucibus. Vestibulum" +
                        " vel tincidunt augue. Etiam et felis interdum, consequat sapien eget, mattis magna. Etiam diam augue, sodales eget turpis ac, molestie" +
                        " tempor tortor. Suspendisse dapibus ipsum a tortor scelerisque fermentum id sit amet eros. Integer sit amet ligula cursus lorem feugiat" +
                        " porttitor eget et eros. Morbi feugiat cursus mollis. Nam sagittis placerat feugiat.",
                Description = null,
                ConductorId = -3
            };

            _orchestrasController.ModelState.AddModelError("ConductorId", "ConductorId must be greater than 0");
            _orchestrasController.ModelState.AddModelError("Name", "Name is required");
            _orchestrasController.ModelState.AddModelError("Image", "Image must be less than 250 characters");
            _orchestrasController.ModelState.AddModelError("Description", "Description is required");

            // Act
            var result = await _orchestrasController.CreateOrchestra(orchestraDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task CreateOrchestra_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var orchestraDto = new OrchestraCreationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                Description = "Test Description",
                ConductorId = It.IsAny<int>()
            };

            _orchestraServiceMock.Setup(x => x.AddOrchestra(orchestraDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.CreateOrchestra(orchestraDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task UpdateOrchestra_Successful_ReturnsOk()
        {
            // Arrange
            var orchestraDto = new OrchestraUpdationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                Description = "Test Description",
                ConductorId = It.IsAny<int>()
            };

            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Description = "Test Description",
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });

            _orchestraServiceMock.Setup(x => x.UpdateOrchestra(testId, orchestraDto))
                .ReturnsAsync(true);

            // Act
            var result = await _orchestrasController.UpdateOrchestra(testId, orchestraDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(200, ((OkObjectResult)result)?.StatusCode);
        }

        [Fact]
        public async Task UpdateOrchestra_InvalidModelPassed_ReturnsBadRequest()
        {
            // Arrange
            var orchestraDto = new OrchestraUpdationDto
            {
                Name = null,
                Image = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla fermentum eros nec quam sodales, " +
                        "at ornare leo laoreet. Duis dapibus metus nec orci viverra volutpat. Nulla facilisi. Proin rutrum eget nisl non euismod." +
                        " Vivamus non ante at arcu tempor tempus ut et massa. Mauris a commodo odio. Cras pulvinar libero eu aliquam faucibus. Vestibulum" +
                        " vel tincidunt augue. Etiam et felis interdum, consequat sapien eget, mattis magna. Etiam diam augue, sodales eget turpis ac, molestie" +
                        " tempor tortor. Suspendisse dapibus ipsum a tortor scelerisque fermentum id sit amet eros. Integer sit amet ligula cursus lorem feugiat" +
                        " porttitor eget et eros. Morbi feugiat cursus mollis. Nam sagittis placerat feugiat.",
                Description = null,
                ConductorId = -3
            };

            var testId = 1;

            _orchestrasController.ModelState.AddModelError("ConductorId", "ConductorId must be greater than 0");
            _orchestrasController.ModelState.AddModelError("Name", "Name is required");
            _orchestrasController.ModelState.AddModelError("Image", "Image must be less than 250 characters");
            _orchestrasController.ModelState.AddModelError("Description", "Description is required");

            // Act
            var result = await _orchestrasController.UpdateOrchestra(testId, orchestraDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task UpdateOrchestra_OrchestraNotFound_ReturnsNotFound()
        {
            // Arrange
            var orchestraDto = new OrchestraUpdationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                Description = "Test Description",
                ConductorId = It.IsAny<int>()
            };

            var testId = 1;

            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId)).ReturnsAsync((OrchestraDto)null!);

            // Act
            var result = await _orchestrasController.UpdateOrchestra(testId, orchestraDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task UpdateOrchestra_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var orchestraDto = new OrchestraUpdationDto
            {
                Name = "Test Orchestra",
                Image = "Test Image",
                Description = "Test Description",
                ConductorId = It.IsAny<int>()
            };

            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Description = "Test Description",
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });

            _orchestraServiceMock.Setup(x => x.UpdateOrchestra(testId, orchestraDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.UpdateOrchestra(testId, orchestraDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task GetAllEnrolleesByOrchestraId_Successful_ReturnsOk()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Description = "Test Description",
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });
            _mockEnrollmentService.Setup(x => x.GetAllEnrolleesByOrchestraId(testId))
                .ReturnsAsync(new List<EnrolleesDto>
                {
            new EnrolleesDto
            {
                PlayerId = 1,
                OrchestraId = testId,
                SectionId = 1,
                InstrumentId = 1,
                Experience = 1,
                isApproved = 0,
            }
                });

            // Act
            var result = await _orchestrasController.GetAllEnrolleesByOrchestraId(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var enrollees = Assert.IsAssignableFrom<IEnumerable<EnrolleesDto>>(okResult.Value);
            Assert.NotNull(enrollees);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllEnrolleesByOrchestraId_IsNullOrEmpty_NoContent()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Description = "Test Description",
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });
            _mockEnrollmentService.Setup(x => x.GetAllEnrolleesByOrchestraId(testId))
                .ReturnsAsync(new List<EnrolleesDto>());

            // Act
            var result = await _orchestrasController.GetAllEnrolleesByOrchestraId(testId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetAllEnrolleesByOrchestraId_OrchestraIdIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var testId = -1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(false);

            // Act
            var result = await _orchestrasController.GetAllEnrolleesByOrchestraId(testId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetAllEnrolleesByOrchestraId_OrchestraNotFound_ReturnsNotFound()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId)).ReturnsAsync((OrchestraDto)null!);

            // Act
            var result = await _orchestrasController.GetAllEnrolleesByOrchestraId(testId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetAllEnrolleesByOrchestraId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var testId = 1;

            _orchestraServiceMock.Setup(x => x.IsIdValid(testId)).Returns(true);
            _orchestraServiceMock.Setup(x => x.GetOrchestraById(testId))
                .ReturnsAsync(new OrchestraDto
                {
                    Id = testId,
                    Name = "Test Orchestra",
                    Image = "Test Image",
                    Description = "Test Description",
                    Conductor = "Test Conductor",
                    Date = DateTime.Now
                });
            _mockEnrollmentService.Setup(x => x.GetAllEnrolleesByOrchestraId(testId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _orchestrasController.GetAllEnrolleesByOrchestraId(testId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Something went wrong", objectResult.Value);
        }
    }
}