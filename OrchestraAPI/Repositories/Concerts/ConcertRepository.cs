using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Models;
using System.Data;
using System.Numerics;

namespace OrchestraAPI.Repositories.Concerts
{
    public class ConcertRepository : IConcertRepository
    {
        private readonly DapperContext _context;

        public ConcertRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddConcert(Concert concert)
        {
            var sql = "INSERT [dbo].[Concert] ([Name], [Description], [Image], [PerformanceDate]) VALUES (@Name, @Description, @Image, @PerformanceDate); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql, new { concert.Name, concert.Description, concert.Image, concert.PerformanceDate });
            }
        }

        public async Task<IEnumerable<Concert>> GetAllConcerts()
        {
            var sp = "spConcert_GetAllConcerts";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Concert>(sp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Concert> GetConcertById(int id)
        {
            var sp = "spConcert_GetConcertById";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Concert>(sp, new { id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdateConcert(int id, Concert concert)
        {
            var sql = "UPDATE [Concert] SET Name = @Name, Description = @Description, PerformanceDate = @PerformanceDate, OrchestraId = @OrchestraId, Image = @Image WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var res =  await connection.ExecuteAsync(sql, new { id, concert.Name, concert.Description, concert.PerformanceDate, concert.OrchestraId, concert.Image });

                return res > 0;
            }
        }

        public async Task<bool> DeleteConcert(int id)
        {
            var sql = "DELETE FROM [dbo].[Concert] WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<bool>(sql, new { id });
            }
        }
        public async Task<IEnumerable<Concert>> GetConcertsByPlayerId(int id)
        {
            var sql = "SELECT C.* FROM [Concert] C WHERE C.OrchestraId IN (" +
                " SELECT E.OrchestraId FROM [Enroll] E " +
                "WHERE E.PlayerId = 1 AND E.isApproved = 1)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Concert>(sql, new { id });
            }
        }

    }
}