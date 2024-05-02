using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class PasswordHasher : 
    IPasswordHasher
{
    private const int SaltSize = 16;

    public string HashPassword(string password, int iterations = 10000)
    {
        using Rfc2898DeriveBytes pbkdf2 = new(password, SaltSize, iterations, HashAlgorithmName.SHA256);

        byte[] salt = pbkdf2.Salt;
        byte[] hash = pbkdf2.GetBytes(32);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }
}
