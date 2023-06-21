using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Instruments
{
    public interface IInstrumentRepository
    {
        /// <summary>
        /// A method that returns an Instrument by ID
        /// </summary>
        /// <param name="id">Instrument ID</param>
        /// <returns>Instrument</returns>
        Task<Instrument> GetInstrumentById(int id);

        /// <summary>
        /// A method that returns an Instrument by Name
        /// </summary>
        /// <param name="name">Instrument Name</param>
        /// <returns>Instrument</returns>
        Task<Instrument> GetInstrumentByName(string name);
    }
}