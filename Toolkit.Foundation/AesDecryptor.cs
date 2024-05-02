using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class AesDecryptor :
    IDecryptor
{
    private const int IvSize = 16;

    public byte[] Decrypt(byte[] cipher, byte[] key)
    {
        Span<byte> iv = cipher.AsSpan(0, IvSize);
        ReadOnlySpan<byte> encryptedContent = cipher.AsSpan(IvSize);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv.ToArray();

        using MemoryStream memoryStream = new(encryptedContent.ToArray());
        using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

        using MemoryStream resultStream = new();
        cryptoStream.CopyTo(resultStream);

        return resultStream.ToArray();
    }
}
