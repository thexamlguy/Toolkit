namespace Toolkit.Foundation;
public interface IDecryptor
{
    bool TryDecrypt(byte[] cipher,
        byte[] key,
        out byte[]? decryptedData);
}