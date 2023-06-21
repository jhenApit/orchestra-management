using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Models;

namespace OrchestraAPI.Services.Conductors
{
    public interface IConductorService
    {
        /// <summary>
        /// A method that returns all Conductors
        /// </summary>
        /// <returns>An IEnumerable of Conductors entities</returns>
        Task<IEnumerable<ConductorDto>> GetAllConductors();

        /// <summary>
        /// A method that returns a Conductor by ID
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <returns>A Conductor Entity</returns>
        Task<ConductorDto> GetConductorById(int id);

        /// <summary>
        /// A method that returns a Conductor by Name
        /// </summary>
        /// <param name="name">Represents the Conductor Name</param>
        /// <returns>A Conductor Entity</returns>
        Task<ConductorDto> GetConductorByName(string name);

        /// <summary>
        /// A method that creates a new Conductor
        /// </summary>
        /// <param name="conductor">Represents all the fields needed to create the entity</param>
        /// <returns>The newly created entity</returns>
        Task<ConductorDto> AddConductor(ConductorCreationDto conductor);

        /// <summary>
        /// A method that updates a Conductor
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <param name="conductor">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateConductor(int id, ConductorUpdationDto conductor);

        /// <summary>
        /// A method that deletes a Conductor
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeleteConductor(int id);

        /// <summary>
        /// A method that checks if a Conductor ID is valid
        /// </summary>
        /// <param name="id">Conductor ID</param>
        /// <returns>A boolean value indicating the validity of the Conductor ID</returns>
        public bool IsIdValid(int id);
    }
}
