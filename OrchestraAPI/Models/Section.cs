using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table("Section")]
    public class Section
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
