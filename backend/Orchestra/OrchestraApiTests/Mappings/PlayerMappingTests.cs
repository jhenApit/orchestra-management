using AutoMapper;
using OrchestraAPI.Dtos.Player;
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
    public class PlayerMappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public PlayerMappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PlayerMapping());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Test Player maps correctly to PlayerDto
        /// </summary>
        [Fact]
        public void PlayerToPlayerDto_MapsCorrectly_ReturnsPlayer()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Player",
                Section = "Test Section",
                Instrument = "Test Instrument",
                Concert = "Test Concert",
                Score = 99
            };
            // Act
            var playerDto = _mapper.Map<PlayerDto>(player);
            // Assert
            Assert.NotNull(playerDto);
            Assert.Equal(player.Id, playerDto.Id);
            Assert.Equal(player.Name, playerDto.Name);
            Assert.Equal(player.Section, playerDto.Section);
            Assert.Equal(player.Instrument, playerDto.Instrument);
            Assert.Equal(player.Concert, playerDto.Concert);
            Assert.Equal(player.Score, playerDto.Score);
        }

        /// <summary>
        /// Test that PlayerDto maps incorrectly to Player
        /// </summary>
        [Fact]
        public void PlayerToPlayerDto_NullInput_ReturnsNull()
        {
            // Arrange
            Player player = null!;

            // Act
            var playerDto = _mapper.Map<PlayerDto>(player);

            // Assert
            Assert.Null(playerDto);
        }
    }
}