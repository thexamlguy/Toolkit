using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class AesEncryptor :
    IEncryptor
{
    private const int IvSize = 16;

    public bool TryEncrypt(byte[] data,
        byte[] key,
        out byte[]? encryptedData)
    {
        encryptedData = null;

        if (data is null || key is null || key.Length != 32)
        {
            return false;
        }

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            using MemoryStream memoryStream = new();
            memoryStream.Write(aes.IV, 0, IvSize);

            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
            }

            encryptedData = memoryStream.ToArray();
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