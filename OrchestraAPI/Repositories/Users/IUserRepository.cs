using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Users
{
    public interface IUserRepository
    {
        /// <summary>
        /// A method that returns all Users
        /// </summary>
        /// <returns>An IEnumerable of Users entities</returns>
        Task<IEnumerable<User>> GetAllUsers();

        /// <summary>
        /// A method that returns all Users by Role
        /// </summary>
        /// <param name="role">Represents the User Role</param>
        /// <returns>An IEnumerable of Users entities</returns>
        Task<IEnumerable<User>> GetUsersbyRole(int role);

        /// <summary>
        /// A method that creates a new User
        /// </summary>
        /// <param name="user">Represents all the fields needed to create the entity</param>
        /// <returns>An integer which represents the Id or Primary Key of the newly created entity</returns>
        Task<int> CreateUser(User user);

        /// <summary>
        /// A method that updates a User
        /// </summary>
        /// <param name="id">Represents the User ID</param>
        /// <param name="user">Represents all the fields needed to update the entity</param>
        /// <returns>A boolean value indicating the success or failure of the updation operation</returns>
        Task<bool> UpdateUser(int id, User user);

        /// <summary>
        /// A method that deletes a User
        /// </summary>
        /// <param name="id">Represents the User ID</param>
        /// <returns>A boolean value indicating the success or failure of the deletion operation</returns>
        Task<bool> DeleteUser(int id);

        /// <summary>
        /// A method that returns a User by ID
        /// </summary>
        /// <param name="id">Represents the User ID</param>
        /// <returns>A User entity</returns>
        Task<User> GetUserById(int id);

        /// <summary>
        /// A method that returns a User by Username
        /// </summary>
        /// <param name="username">Represents the Username of the User</param>
        /// <returns>A User entity</returns>
        Task<User> GetUserbyUsername(string username);

        /// <summary>
        /// A method that adds an image to a User
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="image">String Image</param>
        /// <returns>A boolean value indicating the success or failure of the add image operation</returns>
        Task<bool> AddUserImage(int id, string image);
    }
}
