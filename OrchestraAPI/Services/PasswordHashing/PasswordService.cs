namespace OrchestraAPI.Services.PasswordHashing
{
    public class PasswordService : IPasswordService
    {
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
