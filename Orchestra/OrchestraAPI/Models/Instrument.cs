using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table("Instrument")]
    public class Instrument
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Section { get; set; }
    }
}
