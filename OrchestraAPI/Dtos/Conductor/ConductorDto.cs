namespace OrchestraAPI.Dtos.Conductor
{
    public class ConductorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Orchestra { get; set; }
    }
}