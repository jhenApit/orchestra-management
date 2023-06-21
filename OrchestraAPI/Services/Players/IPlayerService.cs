using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;

namespace OrchestraAPI.Services.Players
{
    public interface IPlayerService
    {
        /// <summary>
        /// A method that adds a new Player
        /// </summary>
        /// <param name="player">New Player Details</param>
        /// <returns>New Player</returns>
        Task<PlayerDto> AddPlayer(PlayerCreationDto player);

        /// <summary>
        /// A method that returns all Players
        /// </summary>
        /// <returns>All Players from the database </returns>
        Task<IEnumerable<PlayerDto>> GetAllPlayers();

        /// <summary>
        /// A method that returns a Player by Id
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Player with its concert,instrument, and section name</returns>
        Task<PlayerDto> GetPlayerById(int id);

        /// <summary>
        /// A method that updates an existing Player
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="playerToUpdate">New Player Details</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdatePlayer(int id, PlayerUpdationDto playerToUpdate);

        /// <summary>
        /// A method that deletes an existing Player
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeletePlayer(int id);

        /// <summary>
        /// A method that updates a Player's Score
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="player">New Player Score</param>
        /// <returns>Player with Updated Score</returns>
        Task<PlayerDto> UpdatePlayerScore(int id, PlayerScoreUpdateDto player);

        /// <summary>
        /// A method that checks if a Player ID is valid
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>A boolean value indicating the validity of the Player ID</returns>
        public bool IsIdValid(int id);
    }
}
