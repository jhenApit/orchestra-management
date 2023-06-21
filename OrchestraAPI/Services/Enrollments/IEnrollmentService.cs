using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;

namespace OrchestraAPI.Services.Enrollments
{
    public interface IEnrollmentService
    {
        /// <summary>
        /// A method that enrolls a Player to a Concert
        /// </summary>
        /// <param name="concertId">Concert ID</param>
        /// <param name="playerId">Player id</param>
        /// <returns>Enrolled Concert</returns>
        Task<ConcertDto> AddOrchestra(int concertId, int playerId);

        /// <summary>
        /// A method that enrolls a Player to an Orchestra
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="orchestraId">Orchestra ID</param>
        /// <param name="sectionId">Section ID</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="experience">Years of Experience</param>
        /// <returns>A boolean value indicating the success or failure of enrolling the player to an orchestra</returns>
        Task<bool> EnrollPlayerToOrchestra(int playerId, int orchestraId, int sectionId, int instrumentId, int experience);

        /// <summary>
        /// A method that returns all Enrollees by Orchestra ID
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>An IEnumerable of Enrollees</returns>
        Task<IEnumerable<EnrolleesDto>> GetAllEnrolleesByOrchestraId(int id);

        /// <summary>
        /// A method that accepts a Player Enrollee and assigns them to an Orchestra.
        /// </summary>
        /// <param name="orchestraID">Orchestra ID</param>
        /// <param name="playerId">Player ID</param>
        /// <returns>Updated Player</returns>
        Task<PlayerDto> AcceptEnrollee(int orchestraID, int playerId);
    }
}
