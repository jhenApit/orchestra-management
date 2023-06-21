using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Services.Players;
using OrchestraAPI.Services.Orchestras;
using OrchestraAPI.Services.Sections;
using OrchestraAPI.Services.Instruments;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Dtos.Enrollment;
using OrchestraAPI.Services.Concerts;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Dtos.Concert;

namespace OrchestraAPI.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IPlayerService _playerService;
        private readonly IOrchestraService _orchestraService;
        private readonly ISectionService _sectionService;
        private readonly IInstrumentService _instrumentService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IConcertService _concertService;

        public PlayersController(ILogger<PlayersController> logger, IPlayerService playerService, IOrchestraService orchestraService, ISectionService sectionService, IInstrumentService instrumentService, IEnrollmentService enrollmentService, IConcertService concertService)
        {
            _logger = logger;
            _playerService = playerService;
            _orchestraService = orchestraService;
            _sectionService = sectionService;
            _instrumentService = instrumentService;
            _enrollmentService = enrollmentService;
            _concertService = concertService;
        }

        /// <summary>
        /// Get all Players
        /// </summary>
        /// <returns>Returns all data from Player database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/players
        ///     {
        ///         "id": 1,
        ///         "userId": 4,
        ///         "name": "User Player",
        ///         "section": "Strings",
        ///         "instrument": "Violin",
        ///         "concert": "Meta Gala",
        ///         "score": 84
        ///     },
        ///     {
        ///         "id": 2,
        ///         "userId": 5,
        ///         "name": "John Dave",
        ///         "section": "Strings",
        ///         "instrument": "Violas",
        ///         "concert": "Meta Gala",
        ///         "score": 37
        ///     }   
        /// </remarks>
        /// <response code="200">Successfully return Players</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name = "GetAllPlayers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PlayerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPlayers()
        {
            try
            {
                var players = await _playerService.GetAllPlayers();

                if (players.IsNullOrEmpty())    
                {
                    return NoContent();
                }

                return Ok(players);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get Player by Id
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Player data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/players/1
        ///     {
        ///         "id": 1,
        ///         "userId": 4,
        ///         "name": "User Player",
        ///         "section": "Strings",
        ///         "instrument": "Violin",
        ///         "concert": "Meta Gala",
        ///         "score": 84
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Player</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Player Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetPlayerById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult?> GetPlayerById(int id)
        {
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);

                if (player == null)
                {
                    return NotFound($"Player with id {id} does not exist");
                }

                return Ok(player);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong.");
            }
        }

        /// <summary>
        /// Update a Player
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="playerToUpdate">New Player Details</param>
        /// <returns>Newly Updated Player</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/players/{id}
        ///     {
        ///         "name": "string"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully Updated Player</response>
        /// <response code="400">Player Details are Invalid</response>
        /// <response code="404">Player ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "UpdatePlayer")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerUpdationDto playerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = await _playerService.GetPlayerById(id);

                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");

                await _playerService.UpdatePlayer(id, playerToUpdate);
                return Ok(await _playerService.GetPlayerById(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }


        /// <summary>
        /// Delete a Player
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Deleted Player ID</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     DELETE /api/players/{id}
        ///     
        ///     STRING Output No Id found
        ///     Player with id {id} does not exist.
        ///     
        ///     String Output Delete Success
        ///     Player with id {id} is successfully deleted
        ///     
        /// </remarks>
        /// <response code="200">Successfully Deleted</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}", Name = "DeletePlayer")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);
                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");

                await _playerService.DeletePlayer(id);
                return Ok($"Player with id {id} is successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Update a Player's Score
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="playerToUpdate">New Player Score</param>
        /// <returns>Newly Updated Player</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/players/{id}/score
        ///     {
        ///         "score": "int"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully Updated Player Score</response>
        /// <response code="400">Invalid ID/Player Details are Invalid</response>
        /// <response code="404">Player ID Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}/score", Name = "UpdatePlayerScore")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlayerScore(int id, [FromBody] PlayerScoreUpdateDto playerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);
                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");

                await _playerService.UpdatePlayerScore(id, playerToUpdate);
                return Ok(await _playerService.GetPlayerById(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Add new Player
        /// </summary>
        /// <param name="player">New Player Details</param>
        /// <returns>Newly Added Player</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST /api/players
        ///     {
        ///         "name": "string"
        ///         "userId": "int",
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully Created Player</response>
        /// <response code="400">Player Details are Invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "AddPlayer")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerCreationDto player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var playerCreated = await _playerService.AddPlayer(player);
                return CreatedAtRoute(nameof(GetPlayerById), new { id = playerCreated.Id }, playerCreated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Enroll a Player to an Orchestra
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="orchestraId">Orchestra ID</param>
        /// <param name="sectionId">Section ID</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="enrollPlayer">Player's Years of Experience</param>
        /// <returns>Orchestra Enrollment Status</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST /api/conductors
        ///     {
        ///         "experience": "int"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully Enrolled Player to Orchestra</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{id}/orchestras/{orchestraId}/sections/{sectionId}/instruments/{instrumentId}/enroll", Name = "EnrollPlayerToOrchestra")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(EnrollPlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnrollPlayerToOrchestra(int id, int orchestraId, int sectionId, int instrumentId, [FromBody] EnrollPlayerDto enrollPlayer)
        {
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);
                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");

                var orchestra = await _orchestraService.GetOrchestraById(orchestraId);
                if (orchestra == null)
                    return NotFound($"Orchestra with id {orchestraId} does not exist.");

                var section = await _sectionService.GetSectionById(sectionId);
                if (section == null)
                    return NotFound($"Section with id {sectionId} does not exist.");

                var instrument = await _instrumentService.GetInstrumentById(instrumentId);
                if (instrument == null)
                    return NotFound($"Instrument with id {instrumentId} does not exist.");

                var result = await _enrollmentService.EnrollPlayerToOrchestra(id, orchestraId, sectionId, instrumentId, enrollPlayer.Experience);
                return Ok($"Player {id} has successfully enrolled to Orchestra {orchestraId} in the Section {sectionId} with the Instrument {instrumentId}");

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Accept a Player Enrollee
        /// </summary>
        /// <param name="orchestraID">Orchestra ID</param>
        /// <param name="playerId">Player ID</param>
        /// <returns>Player Enrollee Status</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     PUT /api/players/{playerId}/orchestras/{orchestraID}/accept
        ///     
        ///     STRING Output No ID found
        ///     Player with id {playerId} does not exist.
        ///     Orchestra with id {orchestraID} does not exist.
        ///     
        ///     String Output Update Success
        ///     Player {playerId} has successfully been accepted to Orchestra {orchestraID}.
        ///     
        /// </remarks>
        /// <response code="200">Successfully Accepted Player to Orchestra</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{playerId}/orchestras/{orchestraID}/accept", Name = "AcceptEnrollee")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptEnrollee(int orchestraID, int playerId)
        {
            try
            {
                var player = await _playerService.GetPlayerById(playerId);
                if (player == null)
                    return NotFound($"Player with id {playerId} does not exist.");

                var orchestra = await _orchestraService.GetOrchestraById(orchestraID);
                if (orchestra == null)
                    return NotFound($"Orchestra with id {orchestraID} does not exist.");

                var result = await _enrollmentService.AcceptEnrollee(orchestraID, playerId);
                return Ok($"Player {playerId} has successfully been accepted to Orchestra {orchestraID}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get All Concerts of a Player by Player ID
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Returns all Player participated Concerts</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/players/{id}/concerts
        ///     {
        ///         "id": 1,
        ///         "name": "Meta Gala",
        ///         "description": "Meta Gala is a description",
        ///         "performanceDate": "2023-12-23T00:00:00",
        ///         "image": null,
        ///         "orchestraId": 1,
        ///         "players": null
        ///     }, ...
        ///     
        /// </remarks>
        /// <response code="200">Successfully return Player Concerts</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Player Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}/concerts", Name = "GetAllConcertsByPlayerId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ConcertDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllConcertsByPlayerId(int id)
        {
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);
                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");
                var concerts = await _concertService.GetConcertsByPlayerId(id);
                return Ok(concerts);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get All Orchestras of a Player by Player ID
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Returns all Player participated Orchestras</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/players/{id}/orchestras
        ///     {
        ///         "id": 1,
        ///         "name": "Manila Symphony Orchestra",
        ///         "image": null,
        ///         "date": "2023-05-24T20:14:53.03",
        ///         "conductor": "",
        ///         "description": "Manila Symphony Orchestra"
        ///     }, ...
        ///     
        /// </remarks>
        /// <response code="200">Successfully return Player Orchestras</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Player Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}/orchestras", Name = "GetAllOrchestrasByPlayerId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<OrchestraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrchestrasByPlayerId(int id)
        {
            if (!_playerService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var player = await _playerService.GetPlayerById(id);
                if (player == null)
                    return NotFound($"Player with id {id} does not exist.");
                var orchestras = await _orchestraService.GetOrchestrasByPlayerId(id);
                return Ok(orchestras);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
