using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Orchestras;

namespace OrchestraAPI.Controllers
{
    [Route("api/orchestras")]
    [ApiController]
    public class OrchestrasController : ControllerBase
    {
        private readonly ILogger<OrchestrasController> _logger;
        private readonly IOrchestraService _orchestraServices;
        private readonly IEnrollmentService _enrollmentServices;

        public OrchestrasController(ILogger<OrchestrasController> logger, IOrchestraService orchestraServices, IEnrollmentService enrollmentService)
        {
            _logger = logger;
            _orchestraServices = orchestraServices;
            _enrollmentServices = enrollmentService;
        }

        /// <summary>
        /// Get all Orchestras
        /// </summary>
        /// <returns>Returns all data from Orchestra database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/orchestras
        ///     {
        ///         "id": 1,
        ///         "name": "Manila Symphony Orchestra",
        ///         "image": null,
        ///         "date": "2023-05-24T10:51:48.203",
        ///         "conductor": "User Admin",
        ///         "description": null
        ///     },
        ///     {
        ///         "id": 1,
        ///         "name": "Philippine Philharmonic Orchestra",
        ///         "image": null,
        ///         "date": "2023-05-24T10:51:48.203",
        ///         "conductor": "User Admin",
        ///         "description": null
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Orchestras</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name = "GetAllOrchestras")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<OrchestraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllOrchestras()
        {
            try
            {
                var orchestras = await _orchestraServices.GetAllOrchestras();

                if (orchestras.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(orchestras);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets the Orchestra by Id
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>Orchestra data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/orchestras/{id}
        ///     {
        ///         "id": 1,
        ///         "name": "Manila Symphony Orchestra",
        ///         "image": null,
        ///         "date": "2023-05-24T10:51:48.203",
        ///         "conductor": "User Admin",
        ///         "description": null
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Orchestra by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Orchestra Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetOrchestraById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrchestraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrchestraById(int id)
        {
            if (!_orchestraServices.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }

            try
            {
                var orchestra = await _orchestraServices.GetOrchestraById(id);

                if (orchestra == null)
                {
                    return NotFound($"Orchestra with id {id} does not exist");
                }

                return Ok(orchestra);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets the Orchestra by Name
        /// </summary>
        /// <returns>Orchestra data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/orchestras/byname/{name}
        ///     {
        ///         "id": 1,
        ///         "name": "Manila Symphony Orchestra",
        ///         "image": null,
        ///         "date": "2023-05-24T10:51:48.203",
        ///         "conductor": "User Admin",
        ///         "description": null
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Orchestra by name</response>
        /// <response code="404">Orchestra Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("byname/{name}", Name = "GetOrchestraByName")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrchestraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrchestraByName(string name)
        {
            try
            {
                var orchestra = await _orchestraServices.GetOrchestraByName(name);

                if (orchestra == null)
                {
                    return NotFound($"Orchestra {name} does not exist");
                }

                return Ok(orchestra);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Deletes an Orchestra
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>Deleted Orchestra ID</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     DELETE /api/orchestras/{id}
        ///     
        ///     STRING Output No ID found
        ///     Orchestra with id {id} does not exist.
        ///     
        ///     STRING Output Delete Success
        ///     Orchestra with id {id} is successfully deleted
        ///     
        /// </remarks>
        /// <response code="200">Successfully Deleted</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}", Name = "DeleteOrchestra")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteOrchestra(int id)
        {
            if (!_orchestraServices.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var orchestra = await _orchestraServices.GetOrchestraById(id);
                if (orchestra == null)
                {
                    return NotFound($"Orchestra with id {id} does not exist.");
                }

                await _orchestraServices.DeleteOrchestra(id);
                return Ok($"Orchestra with id {id} is successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Creates an Orchestra
        /// </summary>
        /// <param name="orchestraDto">New Orchestra Details</param>
        /// <returns>Newly Created Orchestra</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST /api/orchestras
        ///     {
        ///         "name": "String",
        ///         "image": "String",
        ///         "description": "string",
        ///         "conductorId": int
        ///     }
        /// </remarks>
        /// <response code = "201" >Successfully Created Orchestra</response>
        /// <response code="400">Orchestra Details are Invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "CreateOrchestra")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrchestraDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateOrchestra([FromBody] OrchestraCreationDto orchestraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var orchestra = await _orchestraServices.AddOrchestra(orchestraDto);
                return CreatedAtRoute(nameof(GetOrchestraById), new { id = orchestra.Id }, orchestra);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update an Orchestra
        /// </summary>
        /// <param name="id">Orchestra ID to be updated</param>
        /// <param name="orchestraDto">Orchestra Details</param>
        /// <returns>Newly Updated Orchestra</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/orchestras/{id}
        ///     {
        ///         "name": "string",
        ///         "image": "string",
        ///         "description": "string",
        ///         "conductorId": 0
        ///     }
        /// </remarks>
        /// <response code="200">Successfully Updated Orchestra</response>
        /// <response code="400">Orchestra Details are Invalid</response>
        /// <response code="404">Orchestra Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "UpdateOrchestra")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrchestraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateOrchestra(int id, [FromBody] OrchestraUpdationDto orchestraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var orchestra = await _orchestraServices.GetOrchestraById(id);
                if (orchestra == null)
                    return NotFound($"Orchestra with id {id} does not exist.");
                
                await _orchestraServices.UpdateOrchestra(id, orchestraDto);
                return Ok($"Orchestra with id {id} is successfully updated");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get All Unapproved Enrollees by Orchestra Id
        /// </summary>
        /// <param name="id">Orchestra ID</param>
        /// <returns>Returns all Unapproved Enrollees data from Enroll database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET api/orchestras/{id}/enrollees
        ///     {
        ///         "playerId": 1,
        ///         "orchestraId": 1,
        ///         "sectionId": 1,
        ///         "instrumentId": 1,
        ///         "experience": 1,
        ///         "isApproved": 0
        ///     },
        ///     {
        ///         "playerId": 2,
        ///         "orchestraId": 1,
        ///         "sectionId": 1,
        ///         "instrumentId": 3,
        ///         "experience": 1,
        ///         "isApproved": 0
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Unapproved Enrollees by Orchestra id</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Orchestra Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}/enrollees", Name = "GetAllEnrolleesByOrchestraId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<EnrolleesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllEnrolleesByOrchestraId(int id)
        {
            if (!_orchestraServices.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }

            if (await _orchestraServices.GetOrchestraById(id) == null)
            {
                return NotFound($"Orchestra with id {id} does not exist.");
            }

            try
            {
                var enrollees = await _enrollmentServices.GetAllEnrolleesByOrchestraId(id);

                if (enrollees.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(enrollees);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

    }
}
