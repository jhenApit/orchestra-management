namespace OrchestraAPI.Services.PasswordHashing
{
    public interface IPasswordService
    {
        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password">Inputted Password</param>
        /// <param name="hashedPassword">Hassed Password</param>
        /// <returns>A boolean value indicating the validity of the password</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}
