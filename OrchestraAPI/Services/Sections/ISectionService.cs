using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Models;

namespace OrchestraAPI.Services.Sections
{
    public interface ISectionService
    {
        /// <summary>
        /// A method that returns all Sections
        /// </summary>
        /// <returns>All sections</returns>
        Task<IEnumerable<SectionDto>> GetAllSections();

        /// <summary>
        /// A method that returns a Section by Section ID
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>Section</returns>
        Task<SectionDto?> GetSectionById(int id);

        /// <summary>
        /// A method that returns a Section by Section Name
        /// </summary>
        /// <param name="sectionName">Section Name</param>
        /// <returns>Section</returns>
        Task<SectionDto?> GetSectionByName(string sectionName);

        /// <summary>
        /// A method that returns a Player leaderboard by Section ID
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>An IEnumerable of Player leaderboard entries</returns>
        Task<IEnumerable<PlayerDto>> GetLeaderboardsBySection(int id);

        /// <summary>
        /// A method that checks if a Section ID is valid
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>A boolean value indicating the validity of the Section ID</returns>
        public bool IsIdValid(int id);
    }
}
