using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table("Concert")]
    public class Concert
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime PerformanceDate { get; set; }
        public string? Image { get; set; }
        
        public int OrchestraId { get; set; }

        public int? Players { get; set; }
    }
}
