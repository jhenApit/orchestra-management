using AutoMapper;
using Moq;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Concerts;
using OrchestraAPI.Repositories.Enrollments;
using OrchestraAPI.Repositories.Instruments;
using OrchestraAPI.Repositories.Orchestras;
using OrchestraAPI.Repositories.Players;
using OrchestraAPI.Repositories.Sections;
using OrchestraAPI.Services.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrchestraApiTests.Services
{
    public class EnrollmentServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly Mock<IConcertRepository> _mockConcertRepository;
        private readonly Mock<IOrchestraRepository> _mockOrchestraRepository;
        private readonly Mock<ISectionRepository> _mockSectionRepository;
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly Mock<IEnrollmentRepository> _mockEnrollmentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _mockConcertRepository = new Mock<IConcertRepository>();
            _mockOrchestraRepository = new Mock<IOrchestraRepository>();
            _mockSectionRepository = new Mock<ISectionRepository>();
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _mockEnrollmentRepository = new Mock<IEnrollmentRepository>();
            _mockMapper = new Mock<IMapper>();
            _enrollmentService = new EnrollmentService(_mockPlayerRepository.Object, _mockConcertRepository.Object, _mockOrchestraRepository.Object, _mockSectionRepository.Object, _mockInstrumentRepository.Object, _mockEnrollmentRepository.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Test to verify that the AddOrchestra method returns a concert with the correct id
        /// </summary>
        [Fact]
        public async Task AddOrchestra_ValidId_ReturnsConcert()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;
            var enrollment = new Enrollment
            {
                PlayerId = playerId,
                OrchestraId = orchestraId,
                SectionId = sectionId,
                InstrumentId = instrumentId,
                Experience = experience
            };
            var concert = new Concert
            {
                Id = 1,
                OrchestraId = orchestraId
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                OrchestraId = orchestraId
            };
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ReturnsAsync(new Orchestra());
            _mockConcertRepository.Setup(x => x.GetConcertById(concert.Id)).ReturnsAsync(concert);
            _mockConcertRepository.Setup(x => x.UpdateConcert(concert.Id, concert)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<ConcertDto>(concert)).Returns(concertDto);

            // Act
            var result = await _enrollmentService.AddOrchestra(concert.Id, orchestraId);

            // Assert
            Assert.Equal(concert.Id, result.Id);
            Assert.Equal(orchestraId, result.OrchestraId);
        }

        /// <summary>
        /// Test to verify that the AddOrchestra method returns null when the orchestra id is invalid
        /// </summary>
        [Fact]
        public async Task AddOrchestra_InvalidId_ReturnsNull()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;
            var enrollment = new Enrollment
            {
                PlayerId = playerId,
                OrchestraId = orchestraId,
                SectionId = sectionId,
                InstrumentId = instrumentId,
                Experience = experience
            };
            var concert = new Concert
            {
                Id = 1,
                OrchestraId = orchestraId
            };
            var concertDto = new ConcertDto
            {
                Id = 1,
                OrchestraId = orchestraId
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ReturnsAsync(new Orchestra());
            _mockConcertRepository.Setup(x => x.GetConcertById(concert.Id)).ReturnsAsync(concert);
            _mockConcertRepository.Setup(x => x.UpdateConcert(concert.Id, concert)).ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<ConcertDto>(concert)).Returns((ConcertDto)null!);

            // Act
            var result = await _enrollmentService.AddOrchestra(concert.Id, orchestraId);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Test to verify that the AddOrchestra method returns an error when repository fails
        /// </summary>
        [Fact]
        public async Task AddOrchestra_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;
            var enrollment = new Enrollment
            {
                PlayerId = playerId,
                OrchestraId = orchestraId,
                SectionId = sectionId,
                InstrumentId = instrumentId,
                Experience = experience
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ThrowsAsync(new Exception("Database connection error"));
            _mockConcertRepository.Setup(x => x.GetConcertById(orchestraId)).ThrowsAsync(new Exception("Database connection error"));
            _mockConcertRepository.Setup(x => x.UpdateConcert(orchestraId, new Concert())).ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _enrollmentService.AddOrchestra(orchestraId, orchestraId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        /// <summary>
        /// Test to Enroll a player to an orchestra with valid id and returns a true for success
        /// </summary>
        [Fact]
        public async Task EnrollPlayerToOrchestra_ValidId_SuccessEnrollment()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;

            var enrollment = new Enrollment
            {
                PlayerId = playerId,
                OrchestraId = orchestraId,
                SectionId = sectionId,
                InstrumentId = instrumentId,
                Experience = experience
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ReturnsAsync(new Orchestra());
            _mockPlayerRepository.Setup(x => x.GetPlayerById(playerId)).ReturnsAsync(new Player());
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ReturnsAsync(new Instrument());
            _mockSectionRepository.Setup(x => x.GetSectionById(sectionId)).ReturnsAsync(new Section());
            _mockEnrollmentRepository.Setup(x => x.EnrollToAnOrchestra(playerId, orchestraId, sectionId, instrumentId, experience)).ReturnsAsync(true);

            // Act
            var result = await _enrollmentService.EnrollPlayerToOrchestra(playerId, orchestraId, sectionId, instrumentId, experience);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EnrollPlayerToOrchestra_NotExistingId_ReturnsFalse()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;
            var enrollment = new Enrollment
            {
                PlayerId = playerId,
                OrchestraId = orchestraId,
                SectionId = sectionId,
                InstrumentId = instrumentId,
                Experience = experience
            };

            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ReturnsAsync((Orchestra)null!);
            _mockPlayerRepository.Setup(x => x.GetPlayerById(playerId)).ReturnsAsync((Player)null!);
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ReturnsAsync((Instrument)null!);
            _mockSectionRepository.Setup(x => x.GetSectionById(sectionId)).ReturnsAsync((Section)null!);
            _mockEnrollmentRepository.Setup(x => x.EnrollToAnOrchestra(playerId, orchestraId, sectionId, instrumentId, experience)).ReturnsAsync(false);
            
            // Act
            var result = await _enrollmentService.EnrollPlayerToOrchestra(playerId, orchestraId, sectionId, instrumentId, experience);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EnrollPlayerToOrchestra_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var orchestraId = 1;
            var playerId = 1;
            var sectionId = 1;
            var instrumentId = 1;
            var experience = 1;
            _mockOrchestraRepository.Setup(x => x.GetOrchestraById(orchestraId)).ThrowsAsync(new Exception("Database connection error"));
            _mockPlayerRepository.Setup(x => x.GetPlayerById(playerId)).ThrowsAsync(new Exception("Database connection error"));
            _mockInstrumentRepository.Setup(x => x.GetInstrumentById(instrumentId)).ThrowsAsync(new Exception("Database connection error"));
            _mockSectionRepository.Setup(x => x.GetSectionById(sectionId)).ThrowsAsync(new Exception("Database connection error"));
            _mockEnrollmentRepository.Setup(x => x.EnrollToAnOrchestra(playerId, orchestraId, sectionId, instrumentId, experience)).ThrowsAsync(new Exception("Database connection error"));
            
            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _enrollmentService.EnrollPlayerToOrchestra(playerId, orchestraId, sectionId, instrumentId, experience));
            
            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

    }
}
