using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class AesDecryptor :
    IDecryptor
{
    private const int IvSize = 16;

    public bool TryDecrypt(byte[] cipher,
        byte[] key, 
        out byte[]? decryptedData)
    {
        decryptedData = null;

        if (cipher is null || key is null || cipher.Length < IvSize)
        {
            return false;
        }

        try
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

            decryptedData = resultStream.ToArray();

            return true;
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
