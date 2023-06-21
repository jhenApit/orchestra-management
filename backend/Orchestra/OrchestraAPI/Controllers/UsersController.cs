using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Services.Users;

namespace OrchestraAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns>Returns all data from User database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET api/users
        ///     {
        ///         "id": 1,
        ///         "username": "userAdmin",
        ///         "password": "$2a$11$DQcN4aYh0j43u2Xn8YicmesEZxq6255cf8fuCV5HTZu1vBo3QufeG",
        ///         "email": "admin@gmail.com",
        ///         "role": "Conductor",
        ///         "image": null,
        ///         "created_at": "2023-05-24T20:14:53.01"
        ///     },
        ///     {
        ///         "id": 2,
        ///         "username": "sebastian",
        ///         "password": "$2a$11$8r4XGvLkq7GDIDW4wJM1cergDEK025PSyv7MjZuvBjvb8UBYSvkkS",
        ///         "email": "sebastian@gmail.com",
        ///         "role": "Conductor",
        ///         "image": null,
        ///         "created_at": "2023-05-24T20:14:53.01"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return Users</response>
        /// <response code="204">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet(Name = "GetAllUsers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();

                if (users.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(users);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="user">New User Details</param>
        /// <returns>Newly Created User</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     POST api/users
        ///     {
        ///         "username": "string",
        ///         "password": "string",
        ///         "email": "string",
        ///         "role": 0
        ///     }
        /// </remarks>
        /// <response code = "201" >Successfully Created a User</response>
        /// <response code="400">User Details are Invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "AddUser")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newUser = await _userService.CreateUser(user);
                return CreatedAtRoute(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User data from the database</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/users/{id}
        ///     {
        ///         "id": 1,
        ///         "username": "string",
        ///         "password": "string",
        ///         "email": "sfsfsfsf@gmail.com",
        ///         "role": "Unknown",
        ///         "image": "",
        ///         "created_at": "2023-05-11T20:50:01.453"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return User by id</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">User Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetUserById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (!_userService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"User with id {id} does not exist");
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get User by Username and Password
        /// </summary>
        /// <param name="username">Username of the User</param>
        /// <param name="password">Password of the User</param>
        /// <returns>User data from the database</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     username = "jhen"
        ///     password = "apit"
        /// 
        /// Sample Output:
        /// 
        ///     GET /api/users/username-and-password
        ///     {
        ///         "id": 4,
        ///         "username": "jhen",
        ///         "password": "$2a$11$1ay75/p.dsehdNsYDUm5L.emh6rQ3gTtsaOE6ZS8RJvnTs1zxbqtO",
        ///         "email": "ja@gmail.com",
        ///         "role": "Conductor",
        ///         "image": "",
        ///         "created_at": "2023-05-19T19:09:05.473"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully return User</response>
        /// <response code="404">User Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("/users/username-and-password", Name = "GetUserbyUsernameandPassword")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserbyUsernameandPassword(string username, string password)
        {
            try
            {
                var user = await _userService.GetUserbyUsernameandPassword(username, password);

                if (user == null)
                {
                    return NotFound($"User with username {username} and password {password} does not exist");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="id">User ID to be updated</param>
        /// <param name="newUser">New User Details</param>
        /// <returns>Newly Updated User</returns>
        /// <remarks>
        /// Sample Input:
        /// 
        ///     PUT /api/users/{id}
        ///     {
        ///         "username": "string",
        ///         "password": "string",
        ///         "email": "string",
        ///         "role": 0
        ///     }
        /// </remarks>
        /// <response code="200">Successfully Updated User</response>
        /// <response code="400">User Details are Invalid</response>
        /// <response code="404">User Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "UpdateUser")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUpdateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                    return NotFound($"User with id {id} does not exist");
                
                await _userService.UpdateUser(id, newUser);
                return Ok($"User with id {id} is successfully updated");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="id">User ID to be deleted</param>
        /// <returns>Deleted User ID</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     DELETE /api/users/{id}
        ///     
        ///     STRING Output No Id found
        ///     User with id {id} does not exist.
        ///     
        ///     STRING Output Delete Success
        ///     User with id {id} is successfully deleted.
        ///     
        /// </remarks>
        /// <response code="200">Successfully Deleted</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">User Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!_userService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                    return NotFound($"User with id {id} does not exist.");

                await _userService.DeleteUser(id);
                return Ok($"User with id {id} is successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get Users by Role
        /// </summary>
        /// <param name="roleId">User Role ID</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     GET /api/roles/{roleId}
        ///     {
        ///         "id": 2,
        ///         "username": "admin",
        ///         "password": "admin",
        ///         "email": "admin@gmail.com",
        ///         "role": "Conductor",
        ///         "image": "",
        ///         "created_at": "2023-05-11T22:21:00.607"
        ///     },
        ///     {
        ///         "id": 4,
        ///         "username": "jhen",
        ///         "password": "$2a$11$1ay75/p.dsehdNsYDUm5L.emh6rQ3gTtsaOE6ZS8RJvnTs1zxbqtO",
        ///         "email": "ja@gmail.com",
        ///         "role": "Conductor",
        ///         "image": "",
        ///         "created_at": "2023-05-19T19:09:05.473"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully return Users by Role</response>
        /// <response code="400">Invalid Role ID</response>
        /// <response code="404">Users with Role Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("/roles/{roleId}", Name = "GetUsersbyRole")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersbyRole(int roleId)
        {
            if (!_userService.IsIdValid(roleId))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var users = await _userService.GetUsersbyRole(roleId);
                if (users == null)
                {
                    return NotFound($"User with Role id {roleId} does not exist.");
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update User Image
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="image">New User Image</param>
        /// <returns>User Image Update Status</returns>
        /// <remarks>
        /// Sample Output:
        /// 
        ///     PUT /api/users/{id}/image
        ///     
        ///     STRING Output No ID found
        ///     User with id {id} does not exist.
        ///     
        ///     STRING Output Delete Success
        ///     User with id {id} is successfully updated
        ///
        /// </remarks>
        /// <response code="200">Successfully Updated User Image</response>
        /// <response code="400">Invalid ID</response>
        /// <response code="404">Users Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}/image", Name = "AddUserImage")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserImage(int id, string image)
        {
            if (!_userService.IsIdValid(id))
            {
                return BadRequest("Invalid ID. ID must be a positive numeric value.");
            }
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"User with id {id} does not exist.");
                }
                await _userService.AddUserImage(id, image);
                return Ok($"User with id {id} is successfully updated");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}
