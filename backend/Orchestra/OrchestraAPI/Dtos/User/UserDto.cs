    namespace OrchestraAPI.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; }
        public string? Image { get; set; }
        public DateTime Created_at { get; set; }

    }
}
