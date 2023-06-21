using OrchestraAPI.Context;
using OrchestraAPI.Models;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using OrchestraAPI.Dtos.User;
using static System.Net.Mime.MediaTypeNames;

namespace OrchestraAPI.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var sql = "SELECT * FROM [Users]";
            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<User>(sql);
            }
        }

        public async Task<int> CreateUser(User user)
        {
            var sql = "INSERT INTO [Users] (Username, Email, Password, Role) VALUES (@Username, @Email, @Password, @Role) " +
                        "SELECT SCOPE_IDENTITY();";

            using (var con = _context.CreateConnection())
            {

                return await con.ExecuteScalarAsync<int>(sql, new
                {  
                    user.Username,
                    user.Email,
                    user.Password,
                    user.Role,
                });
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            var sql = "DELETE FROM Users WHERE Id = @Id";
            using (var con = _context.CreateConnection())
            {
                var verify = await con.ExecuteAsync(sql, new { Id = id });
                return verifier(verify);
            }
        }

        public async Task<IEnumerable<User>> GetUsersbyRole(int role)
        {
            var sql = "SELECT * FROM Users WHERE Role = @Role";

            using (var con = _context.CreateConnection())
            {
                var users = await con.QueryAsync<User>(sql, new { Role = role });
                return users;
            }
        }

        public async Task<User> GetUserbyUsername(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";

            using (var con = _context.CreateConnection())
            {
                var user = await con.QueryAsync<User>(sql, new { Username = username });
                return user.Single();
            }
        }

        public async Task<bool> UpdateUser(int id, User user)
        {
            var sql = "UPDATE Users SET Username = @Username, Password = @Password, Email = @Email, Role = @Role WHERE Id = @id";

            using (var c = _context.CreateConnection())
            {
                var verify = await c.ExecuteAsync(sql,
                    new
                    {
                        Id = id,
                        user.Username,
                        user.Password,
                        user.Email,
                        user.Role
                    });
                return verifier(verify);
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var sql = "SELECT * FROM [Users] WHERE Id = @Id";
            using (var con = _context.CreateConnection())
            {
                var wait = await con.QueryAsync<User>(sql, new { Id = id });
                return wait.Single();
            }
        }

        public async Task<bool> AddUserImage(int id, string image)
        {
            var sql = "UPDATE [Users] SET Image = @Image WHERE Id = @Id";

            using (var con = _context.CreateConnection())
            {
                var verify = await con.ExecuteAsync(sql, new { Id = id, Image = image });
                return verifier(verify);
            }
        }

        public static bool verifier(int verify)
        {
            return verify > 0 ? true : false;

        }
    }
}
