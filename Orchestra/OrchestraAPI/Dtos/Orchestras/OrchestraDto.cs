using OrchestraAPI.Models;

namespace OrchestraAPI.Dtos.Orchestra
{
    public class OrchestraDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
        public string? Conductor { get; set; }
        public string? Description { get; set; }
    }
}
