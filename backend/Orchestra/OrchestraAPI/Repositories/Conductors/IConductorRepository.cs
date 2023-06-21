using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Conductors
{
    public interface IConductorRepository
    {
        /// <summary>
        /// A method that returns all Conductors
        /// </summary>
        /// <returns>An IEnumerable of Conductors entities</returns>
        Task<IEnumerable<Conductor>> GetAllConductors();

        /// <summary>
        /// A method that returns a Conductor by ID
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <returns>A Conductor Entity</returns>
        Task<Conductor> GetConductorById(int id);

        /// <summary>
        /// A method that returns a Conductor by Name
        /// </summary>
        /// <param name="name">Represents the Conductor Name</param>
        /// <returns>A Conductor Entity</returns>
        Task<Conductor> GetConductorByName(string name);

        /// <summary>
        /// A method that creates a new Conductor
        /// </summary>
        /// <param name="conductor">Represents all the fields needed to create the entity</param>
        /// <returns>An integer which represents the Id or Primary Key of the newly created entity</returns>
        Task<int> AddConductor(Conductor conductor);

        /// <summary>
        /// A method that updates a Conductor
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <param name="conductor">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateConductor(int id, Conductor conductor);

        /// <summary>
        /// A method that deletes a Conductor
        /// </summary>
        /// <param name="id">Represents the Conductor ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeleteConductor(int id);
    }
}
