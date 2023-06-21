using OrchestraAPI.Dtos.User;

namespace OrchestraAPI.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// A method that returns all Users
        /// </summary>
        /// <returns>An IEnumerable of Users entities</returns>
        Task<IEnumerable<UserDto>> GetAllUsers();

        /// <summary>
        /// A method that returns a User by Username and Password
        /// </summary>
        /// <param name="username">Represents the User Username</param>
        /// <param name="password">Represents the User Password</param>
        /// <returns>A User entity</returns>
        Task<UserDto> GetUserbyUsernameandPassword(string username, string password);
        
        /// <summary>
        /// A method that returns all Users by Role
        /// </summary>
        /// <param name="role">Represents the User Role</param>
        /// <returns>An IEnumerable of Users entities</returns>
        Task<IEnumerable<UserDto>> GetUsersbyRole(int role);

        /// <summary>
        /// A method that creates a new User
        /// </summary>
        /// <param name="user">Represents all the fields needed to create the entity</param>
        /// <returns>The newly created entity</returns>
        Task<UserDto> CreateUser(UserCreationDto user);

        /// <summary>
        /// A method that updates a User
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="user">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateUser(int id, UserUpdateDto user);

        /// <summary>
        /// A method that deletes a User
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeleteUser(int id);

        /// <summary>
        /// A method that returns a User by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>A User entity</returns>
        Task<UserDto> GetUserById(int id);

        /// <summary>
        /// A method that adds an image to a User
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="image">String Image</param>
        /// <returns>A boolean value indicating the success or failure of the add image operation</returns>
        Task<bool> AddUserImage(int id, string image);

        /// <summary>
        /// A method that checks if a User ID is valid
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>A boolean value indicating the validity of the User ID</returns>
        public bool IsIdValid(int id);
    }
}