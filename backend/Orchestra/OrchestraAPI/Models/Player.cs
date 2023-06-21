using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace OrchestraAPI.Models
{
    [Table("Player")]
    public class Player
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
