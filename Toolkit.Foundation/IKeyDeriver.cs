namespace Toolkit.Foundation;

public interface IKeyDeriver
{
    byte[] DeriveKey(byte[] phrased,
        byte[] salt,
        int keySize = 32,
        int iterations = 10000);
}