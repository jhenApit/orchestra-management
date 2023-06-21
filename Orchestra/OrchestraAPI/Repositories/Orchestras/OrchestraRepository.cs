using Dapper;
using Microsoft.IdentityModel.Tokens;
using OrchestraAPI.Context;
using OrchestraAPI.Dtos.Orchestra;
using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Orchestras
{
    public class OrchestraRepository : IOrchestraRepository
    {
        private readonly DapperContext _context;

        public OrchestraRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddOrchestra(Orchestra orchestra)
        {
            var sql = "INSERT INTO Orchestra (Name, Image, ConductorId, Description) VALUES (@Name, @Image, @ConductorId, @Description);" +
                      "SELECT SCOPE_IDENTITY();";

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql, new
                {
                    orchestra.Name,
                    orchestra.Image,
                    Date = DateTime.Now,
                    ConductorId = orchestra.Conductor?.Id,
                    orchestra.Description
                });
            }
        }

        public async Task<bool> DeleteOrchestra(int id)
        {
            var sql = "DELETE FROM Orchestra WHERE Id = @id";

            using (var connection = _context.CreateConnection())
            {
                var res = await connection.ExecuteAsync(sql, new { id });
                return verifier(res);
            }
        }

        public async Task<IEnumerable<Orchestra>> GetAllOrchestras()
        {
            var sql = "SELECT * FROM Orchestra O LEFT JOIN Conductor C ON C.Id = O.ConductorId";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Orchestra, Conductor, Orchestra>(sql,(o,c) =>
                {
                    o.Conductor = c;
                    return o;
                });
            }
        }

        public async Task<Orchestra> GetOrchestraById(int id)
        {
            var sql = "SELECT * FROM Orchestra O LEFT JOIN Conductor C ON C.Id = O.ConductorId WHERE O.Id = @id";

            using (var connection = _context.CreateConnection())
            {
                var res = await connection.QueryAsync<Orchestra, Conductor, Orchestra>(sql,(o,c) =>
                {
                    o.Conductor = c;
                    return o;
                }, new { id });
                return res.Single();
            }
        }

        public async Task<Orchestra> GetOrchestraByName(string name)
        {
            var sql = "SELECT * FROM Orchestra O LEFT JOIN Conductor C ON C.Id = O.ConductorId WHERE O.Name = @name";
            using (var connection = _context.CreateConnection())
            {
                var res = await connection.QueryAsync<Orchestra, Conductor, Orchestra>(sql, (o, c) =>
                {
                    o.Conductor = c;
                    return o;
                }, new { name });
                return res.Single();
            }
        }

        public async Task<bool> UpdateOrchestra(int id, Orchestra orchestra)
        {
            var sql = "UPDATE Orchestra SET Name = @Name, Image = @Image, ConductorId = @ConductorId, Description = @Description WHERE Id = @id";

            using (var connection = _context.CreateConnection())
            {
                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    orchestra.Name,
                    orchestra.Image,
                    ConductorId = orchestra.Conductor?.Id,
                    orchestra.Description
                });
                return verifier(res);
            }
        }

        public async Task<IEnumerable<Orchestra>> GetOrchestrasByPlayerId(int id)
        {
            var sql = "SELECT O.* FROM [Orchestra] O " +
                "WHERE O.Id IN ( " +
                "SELECT E.OrchestraId FROM [Enroll] E " +
                "WHERE E.PlayerId = 1 AND E.isApproved = 1)";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Orchestra>(sql, new { id });
            }
        }

        public static bool verifier(int verify)
        {
            return (verify > 0) ? true : false;
        }
    }
}
