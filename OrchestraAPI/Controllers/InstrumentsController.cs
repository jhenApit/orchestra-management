using Microsoft.AspNetCore.Mvc;
using OrchestraAPI.Dtos.Instrument;
using OrchestraAPI.Services.Instruments;

namespace OrchestraAPI.Controllers
{
    [Route("api/instruments")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly ILogger<InstrumentsController> _logger;
        private readonly IInstrumentService _instrumentServices;

        public InstrumentsController(ILogger<InstrumentsController> logger, IInstrumentService instrumentServices)
        {
            _logger = logger;
            _instrumentServices = instrumentServices;
        }

        /// <summary>
        /// Get Instrument by ID
        /// </summary>
        /// <param name="id">Instrument ID</param>
        /// <returns>Instrument data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/instruments/1
        ///     {
        ///         "id": 1,
        ///         "name": "Violin"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully return Instrument by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Instrument Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetInstrumentById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InstrumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetInstrumentById(int id)
        {
            if (!_instrumentServices.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var instrument = await _instrumentServices.GetInstrumentById(id);

                if (instrument == null)
                {
                    return NotFound($"Instrument with ID {id} does not exist.");
                }

                return Ok(instrument);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get Instrument by Name
        /// </summary>
        /// <param name="name">Instrument Name</param>
        /// <returns>Instrument data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/instruments/byname/Violin
        ///     {
        ///         "id": 1,
        ///         "name": "Violin"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully return Instrument by name</response>
        /// <response code="404">Instrument ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("byname/{name}", Name = "GetInstrumentByName")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InstrumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetInstrumentByName(string name)
        {
            try
            {
                var instrument = await _instrumentServices.GetInstrumentByName(name);

                if (instrument == null)
                {
                    return NotFound($"Instrument {name} does not exist.");
                }

                return Ok(instrument);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}
