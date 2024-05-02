using System.Security.Cryptography;

namespace Toolkit.Foundation;

public class KeyGenerator : 
    IKeyGenerator
{
    public byte[] Generate(int size)
    {
        byte[] key = new byte[size];
        RandomNumberGenerator.Fill(key);

        return key;
    }
}
