using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrchestraAPI.Controllers;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Models;
using OrchestraAPI.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace OrchestraApiTests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_mockLogger.Object, _mockUserService.Object);
        }

        /// <summary>
        /// Test Get All Users returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task GetAllUsers_UsersExist_ReturnsOkResult()
        {
            // Arrange
            var users = new List<UserDto>()
            {
                new UserDto()
                {
                    Id = 1,
                    Username = "Tester",
                    Email = "test@gmail.com",
                    Role = "conductor",
                    Image = "test.png",
                    Created_at = DateTime.Now
                },
                new UserDto()
                {
                    Id = 2,
                    Username = "Tester2",
                    Email = "test@gmail.com",
                    Role = "conductor",
                    Image = "test.png",
                    Created_at = DateTime.Now
                },

            };

            _mockUserService.Setup(user => user.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Get All Users no content returns 204
        /// </summary>
        [Fact]
        public async Task GetAllUsers_IsNullOrEmpty_ReturnsNoContent()
        {
            // Arrange
            var users = new List<UserDto>();
            _mockUserService.Setup(user => user.GetAllUsers()).ReturnsAsync(users);
            
            // Act
            var result = await _controller.GetAllUsers();
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// Test Get All Users Internal Server Error returns 500
        /// </summary>
        [Fact]
        public async Task GetAllUsers_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(user => user.GetAllUsers()).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetAllUsers();
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Create Users Successful returns CreatedAtRoute
        /// </summary>
        [Fact]
        public async Task CreateUsers_Successful_ReturnsCreatedAtRoute()
        {
            // Arrange
            var user = new UserDto()
            {
                Id = 1,
                Username = "Tester",
                Email = "email@gmail.com",
                Role = "conductor",
                Image = "test.png",
                Created_at = DateTime.Now
            };
            var userModel = new User()
            {
                Id = 1,
                Username = "Tester",
                Email = "email@gmail.com",
                Role = 1,
                Image = "test.png",
                Created_at = DateTime.Now

            };
            var userCreation = new UserCreationDto
            {
                Username = "Tester",
                Email = "email@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = 1

            };

            _mockUserService.Setup(user => user.CreateUser(userCreation)).ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser(userCreation);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        /// <summary>
        /// Test Create Users Internal Server Error returns 400
        /// </summary>
        [Fact]
        public async Task CreateUser_InvalidDetails_ReturnsBadRequest()
        {
            // Arrange
            var userCreation = new UserCreationDto
            {
                Username = "Tester",
                Email = "wrongemail",
                Password = "notcrypted",
                Role = 3
            };
            
            _controller.ModelState.AddModelError("Email", "Email is not valid");

            // Act
            var result = await _controller.CreateUser(userCreation);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Create Users Internal Server Error returns 500
        /// </summary>
        [Fact]
        public async Task CreateUser_InternalServerError_ReturnsServerError()
        {
            // Arrange
            var userCreation = new UserCreationDto
            {
                Username = "Tester",
                Email = "email@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = 1
            };

            _mockUserService.Setup(user => user.CreateUser(userCreation)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateUser(userCreation);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get user by id returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task GetUserById_UserExists_ReturnsOkResult()
        {
            // Arrange
            var user = new UserDto()
            {
                Id = 1,
                Username = "Tester",
                Email = "email@gmail.com",
                Role = "conductor",
                Image = "test.png",
                Created_at = DateTime.Now
            };

            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test get user by id returns 400
        /// </summary>
        [Fact]
        public async Task GetUserById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(false);
            
            // Act
            var result = await _controller.GetUserById(1);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test get user by id returns 404
        /// </summary>
        [Fact]
        public async Task GetUserById_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync((UserDto)null);
           
            // Act
            var result = await _controller.GetUserById(1);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test get user by id returns 500
        /// </summary>
        [Fact]
        public async Task GetUserById_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetUserById(1);
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test get user by username and password returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task GetUserByUsernameAndPassword_UserExist_ReturnsOkResult()
        {
            // Arrange
            var user = new UserDto()
            {
                Id = 1,
                Username = "Tester",
                Email = "email@gmail.com",
                Role = "conductor",
                Image = "test.png",
                Created_at = DateTime.Now
            };

            var username = "Tester";
            var password = "admin";

            _mockUserService.Setup(us => us.GetUserbyUsernameandPassword(username, password)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserbyUsernameandPassword(username, password);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test get user by username and password returns 404
        /// </summary>
        [Fact]
        public async Task GetUserByUsernameAndPassword_UserNotExist_ReturnsNotFound()
        {
            // Arrange
            var user = new UserDto();
            _mockUserService.Setup(us => us.GetUserbyUsernameandPassword("Tester", "admin")).ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.GetUserbyUsernameandPassword("Tester", "admin");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Get user by username and password returns 500
        /// </summary>
        [Fact]
        public async Task GetUserByUsernameAndPassword_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.GetUserbyUsernameandPassword("Tester", "admin")).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetUserbyUsernameandPassword("Tester", "admin");
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Update User returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task UpdateUser_Valid_ReturnsOkResult()
        {
            // Arrange
            var user = new UserDto()
            {
                Id = 1,
                Username = "Tester",
                Email = "email@gmail.com",
                Role = "conductor",
                Image = "test.png",
                Created_at = DateTime.Now
            };

            var userUpdate = new UserUpdateDto()
            {
                Username = "Tester",
                Email = "email@gmail.com",
                Role = 1
            };

            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync(user);
            _mockUserService.Setup(us => us.UpdateUser(1, userUpdate)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(1, userUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Update User returns 404
        /// </summary>
        [Fact]
        public async Task UpdateUser_DetailsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var user = new UserUpdateDto()
            {
                Username = null,
                Email = "email",
                Role = 3
            };

            _controller.ModelState.AddModelError("Username", "Required");
            _controller.ModelState.AddModelError("Email", "Invalid Email");
            _controller.ModelState.AddModelError("Role", "Invalid Role");

            _mockUserService.Setup(us => us.IsIdValid(-1)).Returns(false);
            _mockUserService.Setup(us => us.UpdateUser(-1, user)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(1, user);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Update User returns 400
        /// </summary>
        [Fact]
        public async Task UpdateUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync((UserDto)null);
            
            // Act
            var result = await _controller.UpdateUser(1, new UserUpdateDto());
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Update User returns 500
        /// </summary>
        [Fact]
        public async Task UpdateUser_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.UpdateUser(1, new UserUpdateDto());
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Delete User returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task DeleteUser_SuccessfulDeletion_ReturnsOkResult()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync(new UserDto());
            _mockUserService.Setup(us => us.DeleteUser(1)).ReturnsAsync(true);
            
            // Act
            var result = await _controller.DeleteUser(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Test Delete User returns 400
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteUser_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(-1)).Returns(false);
            
            // Act
            var result = await _controller.DeleteUser(1);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Delete User returns 404
        /// </summary>
        [Fact]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.DeleteUser(1)).ReturnsAsync(false);
            
            // Act
            var result = await _controller.DeleteUser(1);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Delete User returns 500
        /// </summary>
        [Fact]
        public async Task DeleteUser_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.DeleteUser(1);
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get Users by Role returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_Successful_ReturnsOkResult()
        {
            // Arrange
            var users = new List<UserDto>()
            {
                new UserDto()
                {
                    Id = 1,
                    Username = "Tester",
                    Email = "email@gmail.com",
                    Role = "conductor",
                    Image = "test.png",
                    Created_at = DateTime.Now
                },
                new UserDto()
                {
                    Id = 2,
                    Username = "Tester2",
                    Email = "email@gmail.com",
                    Role = "conductor",
                    Image = "test.png",
                    Created_at = DateTime.Now
                }
            };

            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUsersbyRole(1)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsersbyRole(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Get Users by Role returns 404
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUsersByRole_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(-1)).Returns(false);
            
            // Act
            var result = await _controller.GetUsersbyRole(1);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Get Users by Role returns 400
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_NoUsersFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUsersbyRole(1)).ReturnsAsync((List<UserDto>)null);
            
            // Act
            var result = await _controller.GetUsersbyRole(1);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Get Users by Role returns 500
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUsersByRole_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUsersbyRole(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.GetUsersbyRole(1);
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Add User Image returns Ok Result 200
        /// </summary>
        [Fact]
        public async Task AddUserImage_ValidId_ReturnsOkResult()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync(new UserDto());
            _mockUserService.Setup(us => us.AddUserImage(1, "test.png")).ReturnsAsync(true);
            
            // Act
            var result = await _controller.AddUserImage(1, "test.png");
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test Add User Image returns 404
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddUserImage_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(-1)).Returns(false);
            
            // Act
            var result = await _controller.AddUserImage(1, "test.png");
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test Add User Image returns 400
        /// </summary>
        [Fact]
        public async Task AddUserImage_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ReturnsAsync((UserDto)null);
            
            // Act
            var result = await _controller.AddUserImage(1, "test.png");
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Test Add User Image returns 500
        /// </summary>
        [Fact]
        public async Task AddUserImage_InternalServerError_ReturnsServerError()
        {
            // Arrange
            _mockUserService.Setup(us => us.IsIdValid(1)).Returns(true);
            _mockUserService.Setup(us => us.GetUserById(1)).ThrowsAsync(new Exception());
            
            // Act
            var result = await _controller.AddUserImage(1, "test.png");
            
            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

    }
}
