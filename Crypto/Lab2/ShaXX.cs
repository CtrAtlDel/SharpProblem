using System.Buffers;
using System.Drawing;
using System.Security.Cryptography;

namespace lab2;

public class Const
{
    public const int MinXx = 15;
    public const int MaxXx = 20;
}

public class ShaXx
{
    private readonly int _hashSize;

    public ShaXx(int hashSize)
    {
        if (hashSize < Const.MinXx || hashSize > Const.MaxXx)
            throw new Exception("Bad size for message");
        _hashSize = hashSize;
    }

    public byte[] GetHash(byte[] array)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            try
            {
                var encryptArray = sha256.ComputeHash(array);
                var spanArray = new Span<byte>(encryptArray);
                return spanArray.Slice(0, _hashSize).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Access Exception: {e.Message}");
            }
        }

        return null;
    }

    public static byte[] RandomByteGenerator(int length)
    {
        if (length < 0)
            throw new Exception("Length < 0 ");
        byte[] bytes = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }
        return bytes;
    }
}