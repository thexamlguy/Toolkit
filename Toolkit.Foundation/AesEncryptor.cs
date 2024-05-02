using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class AesEncryptor : 
    IEncryptor
{
    private const int IvSize = 16;

    public byte[] Encrypt(byte[] data, 
        byte[] key)
    {
        if (key.Length != 32)
        {
            throw new ArgumentException("Key must be 256 bits (32 bytes).");
        }

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using MemoryStream memoryStream = new();
        memoryStream.Write(aes.IV.AsSpan(0, IvSize));

        using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
        }

        return memoryStream.ToArray();
    }
}
