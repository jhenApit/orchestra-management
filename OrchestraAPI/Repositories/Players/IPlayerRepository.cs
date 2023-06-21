using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Players
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Get player by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player with its concert,instrument, and section name</returns>
        Task<Player> GetPlayerById(int id);
        /// <summary>
        /// Get all Players
        /// </summary>
        /// <returns>All Players from the database </returns>
        Task<IEnumerable<Player>> GetAllPlayers();
        /// <summary>
        /// Add New player to the Database
        /// </summary>
        /// <param name="player"></param>
        /// <returns>player details</returns>
        Task<int> AddPlayer(Player player);
        /// <summary>
        /// Update Existing Player Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns>new player details</returns>
        Task<bool> UpdatePlayer(int id, Player player);

        /// <summary>
        /// Delete Existing Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True or false when success or not</returns>
        Task<bool> DeletePlayer(int id);

        /// <summary>
        /// A method that updates a Player's Score
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="player">New Player Score</param>
        /// <returns>Player with Updated Score</returns>
        Task<Player> UpdatePlayerScore(int id, Player player);
    }
}