using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.Section;
using OrchestraAPI.Services.Sections;

namespace OrchestraAPI.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ILogger<SectionsController> _logger;
        private readonly ISectionService _sectionService;

        public SectionsController(ILogger<SectionsController> logger, ISectionService sectionService)
        {
            _logger = logger;
            _sectionService = sectionService;
        }

        /// <summary>
        /// Get all Sections
        /// </summary>
        /// <returns>Returns all data from Section database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/sections
        ///     {
        ///         "id" : 1,
        ///         "name" : "String"
        ///     },
        ///     {
        ///         "id" : 2,
        ///         "name" : "Woodwinds",
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Sections</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name = "GetAllSections")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<SectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSections()
        {
            try
            {
                var sections = await _sectionService.GetAllSections();

                if (sections.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(sections);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get Section by ID
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>Section data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/sections/{id}
        ///     {
        ///         "id" : 2,
        ///         "name" : "Woodwinds"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Section by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Section Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetSectionById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionById(int id)
        {
            if (!_sectionService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var section = await _sectionService.GetSectionById(id);

                if (section == null)
                {
                    return NotFound($"Section with id {id} does not exist");
                }

                return Ok(section);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong.");
            }
        }

        /// <summary>
        /// Get Section Leaderboards by Section ID
        /// </summary>
        /// <param name="id">Section ID</param>
        /// <returns>Returns data from Player database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/sections/{id}/leaderboards
        ///     {
        ///         "id": 4,
        ///         "userId": 7,
        ///         "name": "John  Ambrad",
        ///         "section": null,
        ///         "instrument": null,
        ///         "concert": null,
        ///         "score": 99
        ///     },
        ///     {
        ///         "id": 1,
        ///         "userId": 4,
        ///         "name": "User Player",
        ///         "section": null,
        ///         "instrument": null,
        ///         "concert": null,
        ///         "score": 98
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Players Leaderboard by Section id</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Section Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}/leaderboards", Name = "GetSectionLeaderboards")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PlayerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLeaderboardsBySection(int id)
        {
            if (!_sectionService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var section = await _sectionService.GetSectionById(id);

                if (section == null)
                    return NotFound($"Section with id {id} does not exist.");

                var leaderboards = await _sectionService.GetLeaderboardsBySection(id);

                if (leaderboards.IsNullOrEmpty())
                {
                    return NoContent();
                }
                return Ok(leaderboards);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
