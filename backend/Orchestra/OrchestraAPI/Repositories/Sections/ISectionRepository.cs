using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Sections
{
    public interface ISectionRepository
    {
        /// <summary>
        /// Get section by id
        /// </summary>
        /// <param name="id">Section id</param>
        /// <returns>Section</returns>
        Task<Section> GetSectionById(int id);

        /// <summary>
        /// Get all sections
        /// </summary>
        /// <returns>Sections</returns>
        Task<IEnumerable<Section>> GetAllSections();

        /// <summary>
        /// Get section by section name
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <returns>Section</returns>
        Task<Section> GetSectionByName(string sectionName);

        /// <summary>
        /// A method that returns a Player leaderboard by Section ID
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>An IEnumerable of Player leaderboard entries</returns>
        Task<IEnumerable<Player>> GetLeaderboardsBySection(int id);
    }
}
