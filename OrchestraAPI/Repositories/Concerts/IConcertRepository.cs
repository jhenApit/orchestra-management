using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Concerts
{
    public interface IConcertRepository
    {
        /// <summary>
        /// A method that returns a concert by id
        /// </summary>
        /// <param name="id">Concert id</param>
        /// <returns>Concert with that id</returns>
        Task<Concert> GetConcertById(int id);

        /// <summary>
        /// A method that returns all concerts
        /// </summary>
        /// <returns>All Concerts</returns>
        Task<IEnumerable<Concert>> GetAllConcerts();

        /// <summary>
        /// A method that adds concert
        /// </summary>
        /// <param name="concert">New Concert Details to create</param>
        /// <returns>Newly Created Concert</returns>
        Task<int> AddConcert(Concert concert);

        /// <summary>
        /// A method that updates a concert
        /// </summary>
        /// <param name="id">Concert id</param>
        /// <param name="concert">New Concert Details for update</param>
        /// <returns>Newly Updated Concert</returns>
        Task<bool> UpdateConcert(int id, Concert concert);

        /// <summary>
        /// A method that deletes a concert
        /// </summary>
        /// <param name="id">Concert id</param>
        /// <returns>Concert with id {id} is successfully deleted</returns>
        /// <returns>Concert with id {id} does not exist.</returns>
        Task<bool> DeleteConcert(int id);

        /// <summary>
        /// A method that returns all Concerts by Player ID
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>A boolean value indicating the validity of the Concerts ID</returns>
        Task<IEnumerable<Concert>> GetConcertsByPlayerId(int id);
    }
}
