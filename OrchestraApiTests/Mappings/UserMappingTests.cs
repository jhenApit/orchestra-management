using AutoMapper;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Mappings;
using OrchestraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Mappings
{
    public class UserMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public UserMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test User maps correctly to UserDto
        /// </summary>
        [Fact]
        public void UserToUserDto_MapsCorrectly_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "Test User",
                Password = "Test Password",
                Email = "Email@gmail.com",
                Role = 1,
                Image = "TestImageUrl",
            Created_at = DateTime.Now
            };

            // Act
            var switcher = new UserMapping();
            var userDto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(user.Username, userDto.Username);
            Assert.Equal(user.Password, userDto.Password);
            Assert.Equal(user.Email, userDto.Email);
            Assert.Equal(switcher.MapUserRole(user.Role), userDto.Role);
        }

        /// <summary>
        /// Test that UserDto maps incorrectly to User
        /// </summary>
        [Fact]
        public void UserToUserDto_NullInput_ReturnsNull()
        {
            // Arrange
            User user = null!;

            // Act
            var userDto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.Null(userDto);
        }

        [Fact]
        public void UserDtoToUserCreationDto_MapsCorrectly_ReturnsUserDto()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = 1,
                Username = "Test User",
                Password = "Test Password",
                Email = "Email@gmail.com",
                Role = "Admin",
                Image = "Test Image Url",
                Created_at = DateTime.Now
            };

            // Act
            var switcher = new UserMapping();
            var userCreationDto = _mapper.Map<UserCreationDto>(userDto);

            // Assert
            Assert.NotNull(userCreationDto);
            Assert.Equal(userDto.Username, userCreationDto.Username);
            Assert.Equal(userDto.Password, userCreationDto.Password);
            Assert.Equal(userDto.Email, userCreationDto.Email);
            Assert.Equal(switcher.MapRoleUser(userDto.Role), userCreationDto.Role);
        }
    }
}
