using OrchestraAPI.Models;

namespace OrchestraAPI.Dtos.Player
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Section { get; set; }
        public string? Instrument { get; set; }
        public string? Concert { get; set; }
        public decimal Score { get; set; }
    }
}
