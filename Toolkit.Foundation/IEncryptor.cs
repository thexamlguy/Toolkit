namespace Toolkit.Foundation;

public interface IEncryptor
{
    bool TryEncrypt(byte[] data,
        byte[] key,
        out byte[]? encryptedData);
}