namespace Toolkit.Foundation;
public interface IDecryptor
{
    byte[] Decrypt(byte[] cipher,
        byte[] key);
}