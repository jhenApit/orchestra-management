using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table("Conductor")]
    public class Conductor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public Orchestra? Orchestra { get; set; }
    }
}