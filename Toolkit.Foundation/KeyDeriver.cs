using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class KeyDeriver :
    IKeyDeriver
{
    public byte[] DeriveKey(byte[] phrase,
        byte[] salt,
        int keySize = 32,
        int iterations = 100000)
    {
        using Rfc2898DeriveBytes pbkdf2 = new(phrase, salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(keySize);
    }
}