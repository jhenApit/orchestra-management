using AutoMapper;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Dtos.User;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Users;
using BCrypt.Net;
using OrchestraAPI.Services.PasswordHashing;

namespace OrchestraAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;


        public UserService(IUserRepository repository, IPasswordService passwordService, IMapper mapper)
        {
            _repository = repository;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var userModels = await _repository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserDto>>(userModels);
        }
        public async Task<UserDto> CreateUser(UserCreationDto user)
        {
            var userModel = new User
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
            };

            // Hash the password using bcrypt   
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userModel.Password = hashedPassword;

            userModel.Id = await _repository.CreateUser(userModel);
            return _mapper.Map<UserDto>(userModel);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _repository.DeleteUser(id);
        }

        public async Task<IEnumerable<UserDto>> GetUsersbyRole(int role)
        {
            var users = await _repository.GetUsersbyRole(role);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<bool> UpdateUser(int id, UserUpdateDto user)
        {
            var existingUser = await _repository.GetUserById(id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            if (!string.IsNullOrEmpty(user.Password))
            {
                // Hash the password using bcrypt   
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                existingUser.Password = hashedPassword;
            }

            return await _repository.UpdateUser(id, existingUser);
        }

        public async Task<UserDto> GetUserbyUsernameandPassword(string username, string password)
        {
            var user = await _repository.GetUserbyUsername(username);

            if (user == null)
            {
                return null;
            }

            // Verify the password
            bool isPasswordValid = _passwordService.VerifyPassword(password, user.Password);

            if (!isPasswordValid)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var model = await _repository.GetUserById(id);
            if (model == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(model);
        }

        public async Task<bool> AddUserImage(int id, string image)
        {
            var existingUser = await _repository.GetUserById(id);
            if (existingUser == null)
            {
                return false;
            }

            return await _repository.AddUserImage(id, image);
        }

        public bool IsIdValid(int id)
        {
            return id > 0 && IsNumeric(id.ToString());
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}
