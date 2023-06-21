using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Users;
using OrchestraAPI.Services.PasswordHashing;
using OrchestraAPI.Services.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockPasswordService = new Mock<IPasswordService>();
            _userService = new UserService(_mockUserRepository.Object, _mockPasswordService.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Test Get all users return users
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "test",
                    Email = "email@gmail.com",
                    Role = 1,
                    Password = "password"
                    },
                new User
                {
                    Id = 2,
                    Username = "test2",
                    Email = "email@gmai.com",
                    Role = 2,
                    Password = "password2"
                }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Username = "test",
                    Email = "email@gmail.com",
                    Role = "conductor"
                },
                new UserDto
                {
                    Id = 2,
                    Username = "test2",
                    Email = "email@gmail.com",
                    Role = "player"
                }
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(userDtos, result);
            Assert.IsType<List<UserDto>>(result);
            Assert.Equal(userDtos.Count, result.Count());
        }

        /// <summary>
        /// Test get all users return empty
        /// </summary>
        [Fact]
        public async Task GetAllUsers_WhenCalledNoData_ReturnEmpty()
        {
            // Arrange
            var users = new List<User>();
            var userDtos = new List<UserDto>();
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);
            
            // Act
            var result = await _userService.GetAllUsers();
            
            // Assert
            Assert.Equal(userDtos, result);
            Assert.IsType<List<UserDto>>(result);
            Assert.Equal(userDtos.Count, result.Count());
        }

        /// <summary>
        /// Test get all users when repository fails
        /// </summary>
        [Fact]
        public async Task GetAllUsers_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var users = new List<User>();
            var userDtos = new List<UserDto>();

            _mockUserRepository.Setup(repo => repo.GetAllUsers()).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetAllUsers());

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test get user by username and password returns user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserbyUsernameandPassword_ExistingUsernamePassword_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin") // Hash the password
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserbyUsername(user.Username)).ReturnsAsync(user);
            _mockPasswordService.Setup(service => service.VerifyPassword("admin", user.Password)).Returns(true); // Verify the password
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserbyUsernameandPassword(user.Username, "admin"); // Pass the actual password

            // Assert
            Assert.Equal(userDto, result);
            Assert.IsType<UserDto>(result);
        }

        /// <summary>
        /// Test get user by username and password wrong username returns null
        /// </summary>

        [Fact]
        public async Task GetUserbyUsernameandPassword_NonExistingUsername_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserbyUsername(It.IsAny<string>())).ReturnsAsync((User)null!);
            _mockPasswordService.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)null!);

            // Act
            var result = await _userService.GetUserbyUsernameandPassword("test2", "admin");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test get user by username and password wrong password returns null
        /// </summary>
        [Fact]
        public async Task GetUserbyUsernameandPassword_WrongPassword_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserbyUsername(It.IsAny<string>())).ReturnsAsync((User)null!);
            _mockPasswordService.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)null!);

            // Act
            var result = await _userService.GetUserbyUsernameandPassword("test2", "admin");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test get user by username and password when repository fails
        /// </summary>
        [Fact]
        public async Task GetUserbyUsernameandPassword_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserbyUsername(It.IsAny<string>())).ThrowsAsync(new Exception("Database connection error"));
            _mockPasswordService.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserbyUsernameandPassword("test2", "admin"));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test get users by role with existing roles returns users
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_ExistingRole_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "test",
                    Email = "email@gmail.com",
                    Role = 1,
                    Password = BCrypt.Net.BCrypt.HashPassword("admin") // Hash the password
                },
                new User
                {
                    Id = 2,
                    Username = "test2",
                    Email = "email@gmail.com",
                    Role = 1,
                    Password = BCrypt.Net.BCrypt.HashPassword("admin2") // Hash the password
                },
                new User
                {
                    Id = 3,
                    Username = "test3",
                    Email = "email@gmail.com",
                    Role = 1,
                    Password = BCrypt.Net.BCrypt.HashPassword("admin3") // Hash the password
                },
            };

            var userDtos = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Username = "test",
                    Email = "email@gmail.com",
                    Role = "conductor",
                },
                new UserDto
                {
                    Id = 2,
                    Username = "test2",
                    Email = "email@gmail.com",
                    Role = "conductor",
                },
                new UserDto
                {
                    Id = 3,
                    Username = "test3",
                    Email = "email@gmail.com",
                    Role = "conductor",
                }
            };

            _mockUserRepository.Setup(repo => repo.GetUsersbyRole(1)).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetUsersbyRole(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDtos, result);
            Assert.IsType<List<UserDto>>(result);
        }

        /// <summary>
        /// Test get users by role with non existing roles returns null
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_NonExistingRole_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUsersbyRole(1)).ReturnsAsync((List<User>)null!);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(It.IsAny<List<User>>())).Returns((List<UserDto>)null!);
            
            // Act
            var result = await _userService.GetUsersbyRole(1);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test get users by role no data returns empty
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_NoData_ReturnsEmpty()
        {
            // Arrange
            var users = new List<User>();
            var userDtos = new List<UserDto>();
            _mockUserRepository.Setup(repo => repo.GetUsersbyRole(1)).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);
            
            // Act
            var result = await _userService.GetUsersbyRole(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDtos, result);
            Assert.IsType<List<UserDto>>(result);
        }

        /// <summary>
        /// Test Get users by role when repository fails
        /// </summary>
        [Fact]
        public async Task GetUsersByRole_WhenRepositoryFail_ReturnsError()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUsersbyRole(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUsersbyRole(1));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Create User with valid inputs successful creation
        /// </summary>
        [Fact]
        public async Task CreateUser_ValidInputs_SuccessfulCreation()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(1);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _userService.CreateUser(userCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto, result);
            Assert.IsType<UserDto>(result);
        }

        /// <summary>
        /// Test Create User with invalid inputs returns null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateUser_InvalidInput_ReturnsNull()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(0);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)null!);

            // Act
            var result = await _userService.CreateUser(userCreationDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(userCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Update User with valid inputs returns true
        /// </summary>
        [Fact]
        public async Task UpdateUser_ValidInputs_ReturnsTrue()
        {
            // Arrange
            var useUpdationDto = new UserUpdateDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(userModel);
            _mockUserRepository.Setup(repo => repo.UpdateUser(1, It.IsAny<User>())).ReturnsAsync(true);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _userService.UpdateUser(1, useUpdationDto);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test update user invalid input returns null
        /// </summary>
        [Fact]
        public async Task UpdateUser_InvalidInputs_ReturnsFalse()
        {
            // Arrange
            var userUpdationDto = new UserUpdateDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync((User)null!);
            _mockUserRepository.Setup(repo => repo.UpdateUser(1, It.IsAny<User>())).ReturnsAsync(false);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)null);

            // Act
            var result = await _userService.UpdateUser(1, userUpdationDto);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Update User when repository fails returns error
        /// </summary>
        [Fact]
        public async Task UpdateUser_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var userUpdationDto = new UserUpdateDto
            {
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = "admin"
            };

            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(userModel);
            _mockUserRepository.Setup(repo => repo.UpdateUser(1, It.IsAny<User>())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateUser(1, userUpdationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Delete user user exist returns true
        /// </summary>
        [Fact]
        public async Task DeleteUser_UserExist_ReturnsTrue()
        {
            // Arrange
            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(userModel);
            _mockUserRepository.Setup(repo => repo.DeleteUser(1)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUser(1);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Delete user when user does not exist returns false
        /// </summary>
        [Fact]
        public async Task DeleteUser_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userModel = new User();
            var userDto = new UserDto();

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync((User)null!);
            _mockUserRepository.Setup(repo => repo.DeleteUser(1)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUser(1);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test elete user when repository fails returns error
        /// </summary>
        [Fact]
        public async  Task DeleteUser_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var userModel = new User();
            var userDto = new UserDto();

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(userModel);
            _mockUserRepository.Setup(repo => repo.DeleteUser(1)).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUser(1));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Get User By Id when user exist returns user
        /// </summary>
        [Fact]
        public async Task GetUserById_ExistingId_ReturnsUser()
        {
            var userModel = new User
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            var userDto = new UserDto
            {
                Id = 1,
                Username = "test",
                Email = "email@gmail.com",
                Role = "conductor"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(userModel);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _userService.GetUserById(1);

            // Assert
            Assert.Equal(userDto, result);
        }

        /// <summary>
        /// Test Get User By Id when user does not exist returns null
        /// </summary>
        [Fact]
        public async Task GetUserById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync((User)null!);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)null);
           
            // Act
            var result = await _userService.GetUserById(1);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test Get User By Id when repository fails returns error
        /// </summary>
        [Fact]
        public async Task GetUserById_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserById(1));
           
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test Add User Image when user exist returns true
        /// </summary>
        [Fact]
        public async Task AddUserImage_UserExist_ReturnsTrue()
        {
            // Arrange
            var iamge = "stringurlimage";

            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ReturnsAsync(new User());
            _mockUserRepository.Setup(repo => repo.AddUserImage(1, iamge)).ReturnsAsync(true);

            // Act
            var result = await _userService.AddUserImage(1, iamge);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test Add User Image when user does not exist returns false
        /// </summary>
        [Fact]
        public async Task AddUserImage_UserNotExist_ReturnsFalse()
        {
            // Arrange
            var iamge = "stringurlimage";
            _mockUserRepository.Setup(repo => repo.AddUserImage(1, iamge)).ReturnsAsync(false);
            
            // Act
            var result = await _userService.AddUserImage(1, iamge);
            
            
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test Add User Image when repository fails returns error
        /// </summary>
        [Fact]
        public async Task AddUserImage_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var iamge = "stringurlimage";
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).ThrowsAsync(new Exception("Database connection error"));

            _mockUserRepository.Setup(repo => repo.AddUserImage(1, iamge)).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.AddUserImage(1, iamge));


            // Assert
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
