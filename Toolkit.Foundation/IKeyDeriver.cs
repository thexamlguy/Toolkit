namespace Toolkit.Foundation;
public interface IKeyDeriver
{
    byte[] DeriveKey(string password, byte[] salt, int keySize = 32, int iterations = 10000);
}
