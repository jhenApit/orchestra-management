using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Models;
using OrchestraAPI.Services;
using System.Data;

namespace OrchestraAPI.Repositories.Instruments
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly DapperContext _context;

        public InstrumentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Instrument> GetInstrumentById(int id)
        {
            var sql = "SELECT I.Id, I.Name FROM Instrument I WHERE I.Id = @Id;";
            using (var connection = _context.CreateConnection())
            {
                var instrument = await connection.QueryAsync<Instrument>
                    (sql, new { id });
                return instrument.Single();
            }
        }

        public async Task<Instrument> GetInstrumentByName(string name)
        {
            var sql = "SELECT I.Id, I.Name FROM Instrument I WHERE I.Name = @name;";
            using (var connection = _context.CreateConnection())
            {
                var instrument = await connection.QueryAsync<Instrument>
                    (sql, new { name });
                return instrument.Single();
            }
        }
    }
}







