using Dapper;
using OrchestraAPI.Context;
using OrchestraAPI.Controllers;
using OrchestraAPI.Models;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Numerics;
using Instrument = OrchestraAPI.Models.Instrument;

namespace OrchestraAPI.Repositories.Players
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DapperContext _context;

        public PlayerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddPlayer(Player player)
        {
            var sql = "INSERT INTO Player (Name, UserId) VALUES (@Name, @UserId)" +
                      "SELECT SCOPE_IDENTITY();";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql, new
                    {
                        player.Name,
                        player.UserId
                    });
            }
        }

        public async Task<bool> DeletePlayer(int id)
        {
            var sql = "DELETE FROM Player WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<bool>(sql, new { id });
            }
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            var sql = "SELECT p.id, p.userId , p.name, p.score ,s.id ,s.name  , i.id , i.name, c.id ,c.name FROM player p " +
                      "LEFT JOIN section s on s.id = p.sectionid " +
                      "LEFT JOIN instrument i on i.id = p.instrumentid " +
                      "LEFT JOIN concert c on c.id = p.concertid " +
                      "GROUP BY p.id, p.userId , p.name, p.score ,s.id ,s.name  , i.id , i.name, c.id ,c.name;";
            
            using (var connection = _context.CreateConnection())
            {
                var players = await connection.QueryAsync<Player, Section, Instrument, Concert, Player>
                    (sql, MapPlayer);
                return players;
            }
        }

        public async Task<Player> GetPlayerById(int id)
        {
            var sql = "SELECT p.id, p.userId, p.name, p.score ,s.id ,s.name  , i.id , i.name, c.id ,c.name FROM player p " +
                "LEFT JOIN section s on s.id = p.sectionid " +
                "LEFT JOIN instrument i on i.id = p.instrumentid " +
                "LEFT JOIN concert c on c.id = p.concertid " +
                "WHERE p.Id = @Id " +
                "GROUP BY p.id, p.userId, p.name, p.score ,s.id ,s.name  , i.id , i.name, c.id ,c.name;";
            using (var connection = _context.CreateConnection())
            {
                var player = await connection.QueryAsync<Player, Section, Instrument, Concert, Player>
                    (sql, MapPlayer, new { id });
                return player.FirstOrDefault();
            }
        }

        public async Task<bool> UpdatePlayer(int id, Player player)
        {
            var sql = "UPDATE Player Set Name = @Name WHERE Id = @id";
            using (var connection = _context.CreateConnection())
            {
                var res = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    player.Name
                });

                return verifier(res);
            }
        }

        public static Player MapPlayer(Player player, Section section, Instrument instrument, Concert? concert)
        {

            player.Concert = concert?.Name;
            player.Section = section?.Name;
            player.Instrument = instrument?.Name;
            return player;
        }

        public async Task<Player> UpdatePlayerScore(int id, Player player)
        {
            var sql = "UPDATE Player SET Score = @Score WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Player>(sql, new {player.Score, id});
            }
        }

        public static bool verifier(int verify)
        {
            return verify > 0 ? true : false;

        }
    }
}