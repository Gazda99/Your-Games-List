namespace YGL.API.Services; 

public interface ICustomPasswordHasher {
    /// <summary>
    /// Used to hash password provided by the user.
    /// </summary>
    /// <param name="password">Password provided by the user</param>
    /// <param name="salt">Salt used in hashed password (16 bytes)</param>
    /// <returns>Hashed password (32 bytes)</returns>
    public byte[] HashPassword(string password, out byte[] salt);

    /// <summary>
    /// Verifies the password provided by the user with the one stored on server
    /// </summary>
    /// <param name="password">Password provided by the user</param>
    /// <param name="salt">Salt used in hashed password</param>
    /// <param name="hashedPassword">Password stored on server</param>
    /// <returns>Bool indicating if password is valid</returns>
    public bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt);
}