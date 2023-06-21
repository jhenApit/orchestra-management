using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Models;

namespace OrchestraAPI.Repositories.Enrollments
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DapperContext _context;

        public EnrollmentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Player> AcceptEnrollee(int orchestraID, int playerId)
        {
            var sql = "UPDATE Player " +
                      "SET SectionId = E.SectionId, " +
                      "OrchestraId = E.OrchestraId, " +
                      "InstrumentId = E.InstrumentId " +
                      "FROM Player INNER JOIN Enroll E ON Player.Id = E.PlayerId " +
                      "WHERE E.isApproved = 1 AND E.OrchestraId = @orchestraId AND E.PlayerId = @playerId;";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Player>(sql, new { orchestraID, playerId });
            }
        }

        public async Task<bool> EnrollToAnOrchestra(int playerId, int orchestraId, int sectionId, int instrumentId, int experience)
        {
            var sql = "INSERT INTO [Enroll] (PlayerId, OrchestraId, SectionId, InstrumentId, Experience) " +
                      "VALUES (@PlayerId, @OrchestraId, @SectionId, @InstrumentId, @Experience);";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(sql, new
                {
                    PlayerId = playerId,
                    OrchestraId = orchestraId,
                    SectionId = sectionId,
                    InstrumentId = instrumentId,
                    Experience = experience
                });

                Console.WriteLine(result);
                return result > 0;
            }
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrolleesByOrchestaId(int id)
        {
            var sql = "SELECT * FROM [Enroll] WHERE OrchestraId = @id AND isApproved = 0;";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Enrollment>(sql, new { id });
            }
        }
    }
}
