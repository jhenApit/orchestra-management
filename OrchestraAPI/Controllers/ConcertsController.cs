using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Services.Concerts;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Instruments;
using OrchestraAPI.Services.Players;
using OrchestraAPI.Services.Sections;
using OrchestraAPI.Services.Orchestras;

namespace OrchestraAPI.Controllers
{
    [Route("api/concerts")]
    [ApiController]
    public class ConcertsController : ControllerBase  
    {
        private readonly ILogger<ConcertsController> _logger;
        private readonly IConcertService _concertService;
        private readonly IOrchestraService _orchestraService;
        private readonly IEnrollmentService _enrollmentService;

        public ConcertsController(
            ILogger<ConcertsController> logger,
            IConcertService concertService,
            IEnrollmentService enrollmentService,
            IOrchestraService orchestraService)
        {
            _logger = logger;
            _concertService = concertService;
            _enrollmentService = enrollmentService;
            _orchestraService = orchestraService;
        }

        /// <summary>
        /// Get all Concerts
        /// </summary>
        /// <returns>Returns all data from Concert database</returns>
        /// <remarks>
        /// Sample Output:
        ///
        ///     GET /api/concerts
        ///     {
        ///         "id": 1,
        ///         "name": "Concert 1",
        ///         "description": "Description 1",
        ///         "performanceDate": "2019-12-12T00:00:00",
        ///         "image": null,
        ///         "orchestraId": 0,
        ///         "players": 8
        ///     },
        ///     {
        ///         "id": 2,
        ///         "name": "Concert 2",
        ///         "description": "Description 2",
        ///         "performanceDate": "2019-12-12T00:00:00",
        ///         "image": null,
        ///         "orchestraId": 0,
        ///         "players": 0
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Concerts</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name ="GetAllConcerts")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ConcertDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConcerts()
        {
            try
            {
                var concerts = await _concertService.GetAllConcerts();

                if (concerts.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(concerts);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get Concert by ID
        /// </summary>
        /// <param name="id">Concert ID</param>
        /// <returns>Concert data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/concerts/{id}
        ///     {
        ///         "id": 1,
        ///         "name": "Concert 1",
        ///         "description": "Description 1",
        ///         "performanceDate": "2019-12-12T00:00:00",
        ///         "image": null,
        ///         "orchestraId": 0,
        ///         "players": 8
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Concert by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Concert Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetConcertById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConcertDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConcert(int id)
        {
            if (!_concertService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }

            try
            {
                var concert = await _concertService.GetConcertById(id);

                if (concert == null)
                {
                    return NotFound($"Concert with id {id} does not exist");
                }

                return Ok(concert);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Create New Concert
        /// </summary>
        /// <param name="concertDto">New Concert Details</param>
        /// <returns>Newly Created Concert</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST /api/concerts
        ///     {
        ///         "name": "string",
        ///         "description": "string"
        ///         "image": "string",
        ///         "performanceDate": "2023-05-24T10:23:59.326Z"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully Created Concert</response>
        /// <response code="400">Concert Details are Invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "CreateConcert")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConcertCreationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateConcert([FromBody] ConcertCreationDto concertDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newConcert = await _concertService.AddConcert(concertDto);
                return CreatedAtRoute("GetConcertById", new { newConcert.Id }, newConcert);
            }

            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Add an Orchestra to a Concert
        /// </summary>
        /// <param name="concertId">Concert ID</param>
        /// <param name="orchestraId">Orchestra ID</param>
        /// <returns>Newly Updated Concert</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /api/concerts/{concertId}/orchestras/{orchestraId}
        ///     
        /// </remarks>
        /// <response code="200">Successfully Updated Orchestra's Concert</response>
        /// <response code="404">Orchestra/Concert ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{concertId}/orchestras/{orchestraId}", Name = "AddOrchestraToConcert")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConcertDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrchestra([FromRoute] int concertId, [FromRoute] int orchestraId)
        {
            try
            {
                var concert = await _concertService.GetConcertById(concertId);
                if (concert == null)
                    return NotFound($"Concert with id {concertId} does not exist.");

                var orchestra = await _orchestraService.GetOrchestraById(orchestraId);
                if (orchestra == null)
                    return NotFound($"Orchestra with id {orchestraId} does not exist.");

                await _enrollmentService.AddOrchestra(concertId, orchestraId);
                return Ok($"Successfully added orchestra {orchestra.Name} at {concert.Name} concert.");

            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went Wrong");
            }
        }

        /// <summary>
        /// Update a Concert
        /// </summary>
        /// <param name="id">Concert ID</param>
        /// <param name="concertToUpdate">New Concert Details</param>
        /// <returns>Newly Updated Concert</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/concerts/{id}
        ///     {
        ///         "name": "string",
        ///         "description": "string",
        ///         "performanceDate": "2023-05-24T10:23:59.326Z",
        ///         "image": "string",
        ///         "orchestraId": "int"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully Updated Concert</response>
        /// <response code="400">Concert Details are Invalid</response>
        /// <response code="404">Concert ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "UpdateConcert")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConcertUpdateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateConcert(int id, [FromBody] ConcertUpdateDto concertToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var concert = await _concertService.GetConcertById(id);

                if (concert == null)
                    return NotFound($"Concert with id {id} does not exist.");

                var updated = await _concertService.UpdateConcert(id, concertToUpdate);
                return Ok($"Concert {id} is successfully Updated");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Delete a Concert
        /// </summary>
        /// <param name="id">Concert ID</param>
        /// <returns>Deleted Concert ID</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     DELETE /api/concerts/{id}
        ///     
        ///     STRING Output No ID found
        ///     Concert with id {id} does not exist.
        ///     
        ///     String Output Delete Success
        ///     Concert with id {id} is successfully deleted
        ///     
        /// </remarks>
        /// <response code="200">Successfully Deleted</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}", Name = "DeleteConcert")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConcert(int id)
        {
            if (!_concertService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }

            try
            {
                var concert = await _concertService.GetConcertById(id);
                if (concert == null)
                    return NotFound($"Concert with id {id} does not exist.");

                await _concertService.DeleteConcert(id);
                return Ok($"Concert with id {id} is successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
