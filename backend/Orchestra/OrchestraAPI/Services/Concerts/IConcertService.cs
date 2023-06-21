    using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Models;

namespace OrchestraAPI.Services.Concerts
{
    public interface IConcertService
    {
        /// <summary>
        /// A method that returns all concerts
        /// </summary>
        /// <returns>All Concerts</returns>
        Task<IEnumerable<ConcertDto>> GetAllConcerts();

        /// <summary>
        /// A method that returns a concert by id
        /// </summary>
        /// <param name="id">Concert id</param>
        /// <returns>Concert with that id</returns>
        Task<ConcertDto> GetConcertById(int id);

        /// <summary>
        /// A method that adds concert
        /// </summary>
        /// <param name="concertToAdd">New Concert Details to create</param>
        /// <returns>Newly Created Concert</returns>
        Task<ConcertDto> AddConcert(ConcertCreationDto concertToAdd);

        /// <summary>
        /// A method that updates a concert
        /// </summary>
        /// <param name="id">Concert id</param>
        /// <param name="concertToUpdate">New Concert Details for update</param>
        /// <returns>Newly Updated Concert</returns>
        Task<bool> UpdateConcert(int id, ConcertUpdateDto concertToUpdate);

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
        Task<IEnumerable<ConcertDto>> GetConcertsByPlayerId(int id);


        public bool IsIdValid(int id);
    }
}
