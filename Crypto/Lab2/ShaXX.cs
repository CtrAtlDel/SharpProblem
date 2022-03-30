using System.Buffers;
using System.Collections;
using System.Drawing;
using System.Security.Cryptography;

namespace lab2;

public class Const
{
    public const int MinXx = 1;
    public const int MaxXx = 4;
}

public class ShaXx : IDisposable
{
    private readonly int _hashSize;

    private RNGCryptoServiceProvider? _rng = new RNGCryptoServiceProvider();

    private SHA256? _sha256 = SHA256.Create();

    public ShaXx(int hashSize)
    {
        if (hashSize < Const.MinXx || hashSize > Const.MaxXx)
            throw new Exception("Bad size for message");
        _hashSize = hashSize;
    }

    public byte[] GetHash(byte[] array)
    {
        try
        {
            var encryptArray = _sha256.ComputeHash(array);
            var spanArray = new Span<byte>(encryptArray);
            var bitsArray = ShaBitsConver(encryptArray);
            return spanArray.Slice(0, _hashSize).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Access Exception: {e.Message}");
        }

        return null;
    }

    public byte[] ShaBitsConver(byte[] encryptBytes)
    {
        var bitArray = new BitArray(encryptBytes);
        return null;
    }

    public byte[] RandomByteGenerator(int length)
    {
        if (length < 0)
            throw new Exception("Length < 0 ");
        byte[] bytes = new byte[length];

        _rng.GetBytes(bytes);
        return bytes;
    }


    void IDisposable.Dispose()
    {
        _rng?.Dispose();
        _sha256?.Dispose();
    }
}