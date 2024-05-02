namespace Toolkit.Foundation;
public interface IEncryptor
{
    byte[] Encrypt(byte[] data, byte[] key);
}