using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table("Orchestra")]
    public class Orchestra
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public Conductor? Conductor { get; set; }
    }
}
