using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Services.Concerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchestraAPI.Services.Instruments;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Players;
using OrchestraAPI.Services.Sections;
using OrchestraAPI.Services.Orchestras;
using OrchestraAPI.Models;
using OrchestraAPI.Dtos.Concert;
using Castle.Core.Logging;
using OrchestraAPI.Dtos.Player;
using Microsoft.AspNetCore.Http;
using OrchestraAPI.Dtos.Orchestra;
using Xunit;

namespace OrchestraApiTests.Controllers
{
    public class ConcertsControllerTests
    {
        private readonly Mock<ILogger<ConcertsController>> _mockLogger;
        private readonly Mock<IConcertService> _mockConcertService;
        private readonly Mock<IOrchestraService> _mockOrchestraService;
        private readonly Mock<IEnrollmentService> _mockEnrollmentService;
        private readonly ConcertsController _concertsController;

        public ConcertsControllerTests()
        {
            _mockLogger = new Mock<ILogger<ConcertsController>>();
            _mockConcertService = new Mock<IConcertService>();
            _mockOrchestraService = new Mock<IOrchestraService> { CallBase = true };
            _mockEnrollmentService = new Mock<IEnrollmentService>();
            _concertsController = new ConcertsController(
                _mockLogger.Object, 
                _mockConcertService.Object,
                _mockEnrollmentService.Object,
                _mockOrchestraService.Object);
        }

        /// <summary>
        /// Test Controller for GetConcerts with return Values 200
        /// </summary>
        [Fact]
        public async void GetConcerts_ConcertExists_ReturnsOkResult()
        {
            // Arrange
            _mockConcertService.Setup(c => c.GetAllConcerts()).ReturnsAsync(new List<ConcertDto> { new ConcertDto() });

            // Act
            var result = await _concertsController.GetConcerts();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>ss
        /// Test Controller for GetConcerts with return no value 204
        /// </summary>
        [Fact]
        public async Task GetConcerts_IsNullOrEmpty_NoContent()
        {
            // Arrange
            _mockConcertService.Setup(c => c.GetAllConcerts()).ReturnsAsync(new List<ConcertDto>());

            // Act
            var result = await _concertsController.GetConcerts();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// Test Controller for GetConcerts with return InternalServerError 500
        /// </summary>
        [Fact]
        public async Task GetConcerts_InternalServerError_ServerError()
        {
            // Arrange
            _mockConcertService.Setup(c => c.GetAllConcerts()).ThrowsAsync(new Exception());

            // Act
            var result = await _concertsController.GetConcerts();

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Controller for GetConcert with return Value 200
        /// </summary>
        [Fact]
        public async Task GetConcert_ConcertExists_ReturnsOkResult()
        {
            // Arrange
            var testId = 1;

            _mockConcertService.Setup(c => c.IsIdValid(testId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(testId)).ReturnsAsync(new ConcertDto { Id = testId });

            // Act
            var result = await _concertsController.GetConcert(testId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Controller for GetConcert with return Value 400
        /// </summary>
        [Fact]
        public async Task GetConcert_InvalidIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var invalidId = -3;

            _concertsController.ModelState.AddModelError("Id", "Id must be greater than 0");

            // Act
            var badResponse = await _concertsController.GetConcert(invalidId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            Assert.Equal(400, ((BadRequestObjectResult)badResponse).StatusCode);
        }

        /// <summary>
        /// Test Controller for GetConcert with return Value 404
        /// </summary>
        [Fact]
        public async Task GetConcert_ConcertNotFound_ReturnsNotFound()
        {
            // Arrange
            var notFoundId = 10;
            _mockConcertService.Setup(c => c.IsIdValid(notFoundId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(notFoundId))!.ReturnsAsync((ConcertDto)null!);

            // Act
            var result = await _concertsController.GetConcert(notFoundId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
        }

        /// <summary>
        /// Test Controller for GetConcert with return Value 500
        /// </summary>
        [Fact]
        public async Task GetConcert_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var errorId = 10;
            _mockConcertService.Setup(c => c.IsIdValid(errorId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(errorId)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _concertsController.GetConcert(errorId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }


        /// <summary>
        /// Test Controller for Create Concert with return Value 201
        /// </summary>
        [Fact]
        public async Task CreateConcert_Successful_ReturnsCreatedAtRoute()
        {
            // Arrange
            var concertDto = new ConcertCreationDto
            {
                Name = "Metro Gala",
                Description = "Testing Concert"

            };

            _mockConcertService.Setup(c => c.AddConcert(concertDto)).ReturnsAsync(new ConcertDto
            {
                Id = 1,
                Name = "Metro Gala",
                Description = "Testing Concert"
            });

            // Act
            var result = await _concertsController.CreateConcert(concertDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(result, createdAtRouteResult);
            Assert.Equal(StatusCodes.Status201Created, createdAtRouteResult.StatusCode);
        }

        /// <summary>
        /// Test Controller for Create Concert with return Value 400
        /// </summary>
        [Fact]
        public async Task CreateConcert_DetailsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var invalidConcertDto = new ConcertCreationDto
            {
                Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla fermentum eros nec quam sodales, " +
                        "at ornare leo laoreet. Duis dapibus metus nec orci viverra volutpat. Nulla facilisi. Proin rutrum eget nisl non euismod." +
                        " Vivamus non ante at arcu tempor tempus ut et massa. Mauris a commodo odio. Cras pulvinar libero eu aliquam faucibus. Vestibulum" +
                        " vel tincidunt augue. Etiam et felis interdum, consequat sapien eget, mattis magna. Etiam diam augue, sodales eget turpis ac, molestie" +
                        " tempor tortor. Suspendisse dapibus ipsum a tortor scelerisque fermentum id sit amet eros. Integer sit amet ligula cursus lorem feugiat" +
                        " porttitor eget et eros. Morbi feugiat cursus mollis. Nam sagittis placerat feugiat.",
                Description = null
            };

            _concertsController.ModelState.AddModelError("Name", "Name must be less than 50 characters");
            _concertsController.ModelState.AddModelError("Description", "Description is required");
            _concertsController.ModelState.AddModelError("Date", "Date is required");

            // Act
            var badRequestResult = await _concertsController.CreateConcert(invalidConcertDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.Equal(400, ((BadRequestObjectResult)badRequestResult).StatusCode);
        }

        /// <summary>
        /// Test Controller for Create Concert with return Value 500
        /// </summary>
        [Fact]
        public async Task CreateConcert_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var concertDto = new ConcertCreationDto
            {
                Name = "Metro Gala",
                Description = "Testing Concert"
            };
            _mockConcertService.Setup(c => c.AddConcert(concertDto)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _concertsController.CreateConcert(concertDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        /// <summary>
        /// Test Controller for Update Concert with return Value 200
        /// </summary>
        [Fact]
        public async Task UpdateConcert_Successful_ReturnsOk()
        {
            // Arrange
            var concertId = 1;
            var concertDto = new ConcertUpdateDto
            {
                Name = "Metro Gala",
                Description = "Testing Concert",
                PerformanceDate = DateTime.Now,
                Image = "TestImageURL",
                OrchestraId = 1
            };

            var concertModel = new ConcertDto
            {
                Id = 1,
                Name = "Metro Gala",
                Description = "Testing Concert",
                PerformanceDate = DateTime.Now,
                Image = "TestImageURL",
                OrchestraId = 1
            };

            _mockConcertService.Setup(c => c.IsIdValid(concertId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync(concertModel);
            _mockConcertService.Setup(c => c.UpdateConcert(concertId, concertDto)).ReturnsAsync(true);

            // Act
            var result = await _concertsController.UpdateConcert(concertId, concertDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Controller for Update Concert with return Value 400
        /// </summary>
        [Fact]
        public async Task UpdateConcert_DetailsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var concertId = 1;
            var invalidConcertDto = new ConcertUpdateDto
            {
                Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla fermentum eros nec quam sodales, " +
                        "at ornare leo laoreet. Duis dapibus metus nec orci viverra volutpat. Nulla facilisi. Proin rutrum eget nisl non euismod." +
                        " Vivamus non ante at arcu tempor tempus ut et massa. Mauris a commodo odio. Cras pulvinar libero eu aliquam faucibus. Vestibulum" +
                        " vel tincidunt augue. Etiam et felis interdum, consequat sapien eget, mattis magna. Etiam diam augue, sodales eget turpis ac, molestie" +
                        " tempor tortor. Suspendisse dapibus ipsum a tortor scelerisque fermentum id sit amet eros. Integer sit amet ligula cursus lorem feugiat" +
                        " porttitor eget et eros. Morbi feugiat cursus mollis. Nam sagittis placerat feugiat.",
                Description = null,
                OrchestraId = 3

            };
            _concertsController.ModelState.AddModelError("Name", "Name must be less than 50 characters");
            _concertsController.ModelState.AddModelError("Description", "Description is required");
            _concertsController.ModelState.AddModelError("PerformanceDate", "Date is required");

            // Act
            var badRequestResult = await _concertsController.UpdateConcert(concertId, invalidConcertDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.Equal(400, ((BadRequestObjectResult)badRequestResult).StatusCode);
        }

        /// <summary>
        /// Test Controller for Update Concert with return Value 404
        /// </summary>
        [Fact]
        public async Task UpdateConcert_ConcertNotFound_ReturnsNotFound()
        {
            // Arrange
            var concertId = 1;
            var concertDto = new ConcertUpdateDto
            {
                Name = "Metro Gala",
                Description = "Testing Concert",
            };
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync((ConcertDto)null!);

            // Act
            var result = await _concertsController.UpdateConcert(concertId, concertDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Controller for Update Concert with return Value 500
        /// </summary>
        [Fact]
        public async Task UpdateConcert_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var concertId = 1;
            var concertDto = new ConcertUpdateDto
            {
                Name = "Metro Gala",
                Description = "Testing Concert",
            };
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync(new ConcertDto
            {
                Id = concertId,
                Name = "Original Concert",
                Description = "Original Description",
                PerformanceDate = DateTime.Now
            });
            _mockConcertService.Setup(c => c.UpdateConcert(concertId, concertDto)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _concertsController.UpdateConcert(concertId, concertDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

        /// <summary>
        /// Test Controller for Delete Concert with return Value 200
        /// </summary>
        [Fact]
        public async Task DeleteConcert_Successful_ReturnsOk()
        {
            // Arrange
            var concertId = 1;
            _mockConcertService.Setup(c => c.IsIdValid(concertId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync(new ConcertDto
            {
                Id = concertId,
                Name = "Original Concert",
                Description = "Original Description",
                PerformanceDate = DateTime.Now
            });

            // Act
            var result = await _concertsController.DeleteConcert(concertId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Concert with id 1 is successfully deleted", okObjectResult.Value);
        }

        /// <summary>
        /// Test Controller for Delete Concert with return Value 404
        /// </summary>
        [Fact]
        public async Task DeleteConcert_ConcertNotFound_ReturnsNotFound()
        {
            // Arrange
            var concertId = 1;
            _mockConcertService.Setup(c => c.IsIdValid(concertId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync((ConcertDto)null!);

            // Act
            var result = await _concertsController.DeleteConcert(concertId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Controller for Delete Concert with return Value 400
        /// </summary>
        [Fact]
        public async Task DeleteConcert_ConcertNotFound_ReturnsBadRequest()
        {
            // Arrange
            var concertId = -1;
            _mockConcertService.Setup(c => c.IsIdValid(concertId)).Returns(false);

            // Act
            var result = await _concertsController.DeleteConcert(concertId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Controller for Delete Concert with return Value 500
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteConcert_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var concertId = 1;
            _mockConcertService.Setup(c => c.IsIdValid(concertId)).Returns(true);
            _mockConcertService.Setup(c => c.GetConcertById(concertId)).ReturnsAsync(new ConcertDto
            {
                Id = concertId,
                Name = "Original Concert",
                Description = "Original Description",
                PerformanceDate = DateTime.Now
            });
            _mockConcertService.Setup(c => c.DeleteConcert(concertId)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _concertsController.DeleteConcert(concertId);


            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
            Assert.Equal("Something went wrong", (result as ObjectResult)?.Value);
        }

    }
}
