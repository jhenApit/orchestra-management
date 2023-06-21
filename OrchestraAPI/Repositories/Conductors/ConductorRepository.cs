using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Conductors
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly DapperContext _context;

        public ConductorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddConductor(Conductor conductor)
        {
            var sql = "INSERT INTO Conductor (Name, UserId) VALUES (@Name, @UserId) " +
                       "SELECT SCOPE_IDENTITY();";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new 
                {
                    conductor.Name,
                    conductor.UserId
                });
            }
        }

        public async Task<bool> DeleteConductor(int id)
        {
            var sql = "DELETE FROM Conductor WHERE Id = @id;";

            using (var con = _context.CreateConnection())
            {
                var res = await con.ExecuteAsync(sql, new { id });
                return verifier(res);
            }
        }

        public async Task<IEnumerable<Conductor>> GetAllConductors()
        {
            var sql = "SELECT * FROM Conductor C LEFT JOIN Orchestra O ON O.ConductorId = C.Id;";

            using(var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Conductor, Orchestra, Conductor>(sql, (c, o) =>
                {
                    c.Orchestra = o;
                    return c;
                });
            }
        }

        public async Task<Conductor> GetConductorById(int id)
        {
            var sql = "SELECT * FROM Conductor WHERE Id = @id;";

            using (var con = _context.CreateConnection())
            {
                var res = await con.QueryAsync<Conductor>(sql, new {id});
                return res.Single();
            }
        }

        public async Task<Conductor> GetConductorByName(string name)
        {
            var sql = "SELECT * FROM Conductor WHERE Name = @name;";

            using (var con = _context.CreateConnection())
            {
                var res = await con.QueryAsync<Conductor>(sql, new { name });
                return res.Single();
            }
        }

        public async Task<bool> UpdateConductor(int id, Conductor conductor)
        {
            var sql = "UPDATE Conductor SET Name = @Name WHERE Id = @id;";

            using (var con = _context.CreateConnection())
            {
                var res = await con.ExecuteAsync(sql, new { conductor.Name, id });
                return verifier(res);
            }
        }

        public static bool verifier(int verify)
        {
            return (verify > 0) ? true : false;

        }
    }
}
