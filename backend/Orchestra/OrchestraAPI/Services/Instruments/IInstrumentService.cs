using OrchestraAPI.Models;
using OrchestraAPI.Dtos.Instrument;

namespace OrchestraAPI.Services.Instruments
{
    public interface IInstrumentService
    {
        /// <summary>
        /// A method that returns an Instrument by ID
        /// </summary>
        /// <param name="id">Instrument ID</param>
        /// <returns>Instrument</returns>
        Task<InstrumentDto> GetInstrumentById(int id);

        /// <summary>
        /// A method that returns an Instrument by Name
        /// </summary>
        /// <param name="name">Instrument Name</param>
        /// <returns>Instrument</returns>
        Task<InstrumentDto> GetInstrumentByName(string name);

        /// <summary>
        /// A method that checks if a Instrument ID is valid
        /// </summary>
        /// <param name="id">Instrument ID</param>
        /// <returns>A boolean value indicating the validity of the Instrument ID</returns>
        public bool IsIdValid(int id);
    }
}
