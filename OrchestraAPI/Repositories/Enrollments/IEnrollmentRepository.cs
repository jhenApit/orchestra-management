using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Enrollments
{
    public interface IEnrollmentRepository
    {
        /// <summary>
        /// A method that enrolls a Player to an Orchestra
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="orchestraId">Orchestra ID</param>
        /// <param name="sectionId">Section ID</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="experience">Years of Experience</param>
        /// <returns>A boolean value indicating the success or failure of enrolling the player to an orchestra</returns>
        Task<bool> EnrollToAnOrchestra(int playerId, int orchestraId, int sectionId, int instrumentId, int experience);

        /// <summary>
        /// A method that returns all Enrollees by Orchestra ID
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>An IEnumerable of Enrollees</returns>
        Task<IEnumerable<Enrollment>> GetAllEnrolleesByOrchestaId(int id);

        /// <summary>
        /// A method that accepts a Player Enrollee and assigns them to an Orchestra.
        /// </summary>
        /// <param name="orchestraID">Orchestra ID</param>
        /// <param name="playerId">Player ID</param>
        /// <returns>Updated Player</returns>
        Task<Player> AcceptEnrollee(int orchestraID, int playerId);
    }
}