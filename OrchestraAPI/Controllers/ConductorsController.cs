using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Conductor;
using OrchestraAPI.Services.Conductors;

namespace OrchestraAPI.Controllers
{
    [Route("api/conductors")]
    [ApiController]

    public class ConductorsController : ControllerBase
    {
        private readonly ILogger<ConductorsController> _logger;
        private readonly IConductorService _conductorService;
        public ConductorsController(ILogger<ConductorsController> logger, IConductorService conductorService)
        {
            _conductorService = conductorService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Conductors
        /// </summary>
        /// <returns>Returns all data from Conductor database</returns>
        /// <remarks>
        /// Sample Output:
        ///     
        ///     GET /api/conductors
        ///     {
        ///         "id": 1,
        ///         "userId": 1,
        ///         "name": "Jane Doe",
        ///         "orchestra": "Orchestra 1"
        ///     },
        ///     {
        ///         "id": 2,
        ///         "userId": 2,
        ///         "name": "John Whick",
        ///         "orchestra": "Orchestra 2"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Conductors</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name = "GetAllConductors")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ConductorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllConductors()
        {
            try
            {
                var conductors = await _conductorService.GetAllConductors();

                if (conductors.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(conductors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get Conductor by ID
        /// </summary>
        /// <param name="id">Conductor ID</param>
        /// <returns>Conductor data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/conductors/{id}
        ///     {
        ///         "id": 1,
        ///         "userId": 1,
        ///         "name": "Jane Doe",
        ///         "orchestra": "Orchestra 1"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully return Conductor by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Conductor Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetConductorById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConductorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConductorById(int id)
        {
            if(!_conductorService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var conductor = await _conductorService.GetConductorById(id);

                if (conductor == null)
                {
                    return NotFound($"Conductor with id {id} does not exist");
                }
                return Ok(conductor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get Conductor by Name
        /// </summary>
        /// <param name="name">Conductor Name</param>
        /// <returns>Conductor data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/conductors/byname/{name}
        ///     {
        ///         "id": 1,
        ///         "userId": 1,
        ///         "name": "Jane Doe",
        ///         "orchestra": "Orchestra 1"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully return Conductor by name</response>
        /// <response code="404">Conductor Name Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("byname/{name}", Name = "GetConductorByName")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConductorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConductorByName(string name)
        {
            try
            {
                var conductor = await _conductorService.GetConductorByName(name);

                if (conductor == null)
                {
                    return NotFound($"Conductor {name} does not exist");
                }
                return Ok(conductor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Create New Conductor
        /// </summary>
        /// <param name="conductor">New Conductor Details</param>
        /// <returns>Newly Created Conductor</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST /api/conductors
        ///     {
        ///         "name": "string"
        ///         "userId": "int",
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully Created Conductor</response>
        /// <response code="400">Conductor Details are Invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "AddConductor")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConductorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddConductor([FromBody] ConductorCreationDto conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newConductor = await _conductorService.AddConductor(conductor);
                return CreatedAtAction("GetConductorById", new { id = newConductor.Id }, newConductor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Update a Conductor
        /// </summary>
        /// <param name="id">Conductor ID</param>
        /// <param name="newConductor">New Conductor Details</param>
        /// <returns>Newly Updated Conductor</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/conductors
        ///     {
        ///         "name": "string"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully Updated Conductor</response>
        /// <response code="400">Conductor Details are Invalid</response>
        /// <response code="404">Conductor ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "UpdateConductor")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConductorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateConductor(int id, [FromBody] ConductorUpdationDto newConductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var conductor = await _conductorService.GetConductorById(id);
                if (conductor == null)
                    return NotFound($"Conductor with id {id} does not exist");
                
                await _conductorService.UpdateConductor(id, newConductor);
                return Ok($"Conductor with id {id} is successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete a Conductor
        /// </summary>
        /// <param name="id">Conductor ID</param>
        /// <returns>Deleted Conductor ID</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     DELETE /api/conductors/{id}
        ///     
        ///     STRING Output No ID found
        ///     Conductor with id {id} does not exist.
        ///     
        ///     String Output Delete Success
        ///     Conductor with id {id} is successfully deleted.
        ///     
        /// </remarks>
        /// <response code="200">Successfully Deleted</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}", Name = "DeleteConductor")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConductor(int id)
        {
            if (!_conductorService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var conductor = await _conductorService.GetConductorById(id);
                if (conductor == null)
                    return NotFound($"Conductor with id {id} does not exist.");

                await _conductorService.DeleteConductor(id);
                return Ok($"Conductor with id {id} is successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
