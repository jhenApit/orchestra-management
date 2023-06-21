using OrchestraAPI.Dtos.Orchestra;

namespace OrchestraAPI.Services.Orchestras
{
    public interface IOrchestraService
    {
         /// <summary>
        /// A method that returns all Orchestras
        /// </summary>
        /// <returns>An IEnumerable of Orchestras entities</returns>
        Task<IEnumerable<OrchestraDto>> GetAllOrchestras();

        /// <summary>
        /// A method that returns an Orchestra by ID
        /// </summary>
        /// <param name="id">Represents the Orchestra ID</param>
        /// <returns>An Orchestra entity</returns>
        Task<OrchestraDto> GetOrchestraById(int id);

        /// <summary>
        /// A method that returns an Orchestra by Name
        /// </summary>
        /// <param name="name">Represents the Orchestra Name</param>
        /// <returns>An Orchestra entity</returns>
        Task<OrchestraDto> GetOrchestraByName(string name);

        /// <summary>
        /// A method that creates a new Orchestra
        /// </summary>
        /// <param name="orchestra">Represents all the fields needed to create the entity</param>
        /// <returns>The newly created entity</returns>
        Task<OrchestraDto> AddOrchestra(OrchestraCreationDto orchestra);

        /// <summary>
        /// A method that updates an Orchestra
        /// </summary>
        /// <param name="id">Represents the Orchestra ID</param>
        /// <param name="orchestra">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateOrchestra(int id, OrchestraUpdationDto orchestra);

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
        Task<IEnumerable<OrchestraDto>> GetOrchestrasByPlayerId(int id);

        /// <summary>
        /// A method that checks if a Orchestra ID is valid
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>A boolean value indicating the validity of the Orchestra ID</returns>
        public bool IsIdValid(int id);
    }
}
