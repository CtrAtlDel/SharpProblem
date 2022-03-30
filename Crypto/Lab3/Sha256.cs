using System.Security.Cryptography;

namespace lab3;

public class Sha256 : IDisposable
{
    
    private RNGCryptoServiceProvider? _rng = new RNGCryptoServiceProvider();

    private SHA256? _sha256 = SHA256.Create();
    
    public byte[] RandomByteGenerator(int length)
    {
        if (length < 0)
            throw new Exception("Length < 0 ");
        byte[] bytes = new byte[length];

        _rng.GetBytes(bytes);
        return bytes;
    }

    public byte[] getSha(byte[] array)
    {
        try
        {
            var encryptArray = _sha256.ComputeHash(array);
            if (encryptArray == null)
                throw new Exception("Sha generate null");
            return encryptArray;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Access Exception: {e.Message}");
        }

        return null;
    }

    void IDisposable.Dispose()
    {
        _rng?.Dispose();
        _sha256?.Dispose();
    }
}