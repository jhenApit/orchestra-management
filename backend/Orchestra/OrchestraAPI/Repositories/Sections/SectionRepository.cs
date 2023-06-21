using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Models;
using System.Data;

namespace OrchestraAPI.Repositories.Sections
{
    public class SectionRepository : ISectionRepository
    {
        private readonly DapperContext _context;

        public SectionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Section>> GetAllSections()
        {
            var sql = "SELECT * FROM SECTION";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Section>(sql);
            }
        }

        public async Task<Section> GetSectionById(int id)
        {
            var sql = "SELECT * FROM SECTION S WHERE S.Id = @id";
            using (var connection = _context.CreateConnection())
            {
                var s = await connection.QueryAsync<Section>(sql, new { id });
                return s.Single();
            }
        }

        public async Task<Section> GetSectionByName(string sectionName)
        {
            var sql = "SELECT * FROM SECTION S WHERE S.Name = @sectionName";
            using (var connection = _context.CreateConnection())
            {
                var s = await connection.QueryAsync<Section>(sql, new { sectionName });
                return s.Single();
            }
        }

        public async Task<IEnumerable<Player>> GetLeaderboardsBySection(int id)
        {
            var sql = "SELECT * FROM Player p WHERE p.SectionId = @id ORDER BY p.Score DESC";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Player>(sql, new { id } );
            }
        }
    }
}
