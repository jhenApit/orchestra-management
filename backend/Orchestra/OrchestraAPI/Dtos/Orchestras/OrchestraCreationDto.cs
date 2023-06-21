namespace OrchestraAPI.Dtos.Orchestra
{
    public class OrchestraCreationDto
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int ConductorId { get; set; }
    }
}
