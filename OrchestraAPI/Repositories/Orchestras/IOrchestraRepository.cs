using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Orchestras

{
    public interface IOrchestraRepository
    {
        /// <summary>
        /// A method that returns all Orchestras
        /// </summary>
        /// <returns>
        /// An IEnumerable of Orchestras entities
        /// </returns>
        Task<IEnumerable<Orchestra>> GetAllOrchestras();

        /// <summary>
        /// A method that returns an Orchestra by ID
        /// </summary>
        /// <param name="id">Represents the Orchestra ID</param>
        /// <returns>An Orchestra entity</returns>
        Task<Orchestra> GetOrchestraById(int id);

        /// <summary>
        /// A method that returns an Orchestra by Name
        /// </summary>
        /// <param name="name">Represents the Orchestra Name</param>
        /// <returns>An Orchestra entity</returns>
        Task<Orchestra> GetOrchestraByName(string name);

        /// <summary>
        /// A method that creates a new Orchestra
        /// </summary>
        /// <param name="orchestra">Represents all the fields needed to create the entity</param>
        /// <returns>An integer which represents the Id or Primary Key of the newly created entity</returns>
        Task<int> AddOrchestra(Orchestra orchestra);

        /// <summary>
        /// A method that updates an Orchestra
        /// </summary>
        /// <param name="id">Represents the Orchestra ID</param>
        /// <param name="orchestra">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateOrchestra(int id, Orchestra orchestra);

        /// <summary>
        /// A method that deletes an Orchestra
        /// </summary>
        /// <param name="id">Represents the Orchestra ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeleteOrchestra(int id);

        /// <summary>
        /// A method that returns all Orchestras by Player ID
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>An IEnumerable of Orchestras</returns>
        Task<IEnumerable<Orchestra>> GetOrchestrasByPlayerId(int id);
    }
}
