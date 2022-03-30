using System.Security.Cryptography;
namespace lab3;

public class Cipher : IDisposable
{
    private Aes? _aes = new AesCryptoServiceProvider();

    private byte[] _key = null;

    void SetKey(byte[] key)
    {
        if (key.Length == 0)
            throw new Exception("Key is 0...");
        if (key.Length != Const.AesKeySize)
            throw new Exception("Bad size of key...");

        _key = key;
    }
    
    byte[] BlockCipherEncrypt(byte[] data)
    {
        if (_key == null)
            throw new Exception("Key is null");

        if (this._key.Length == 0)
            throw new Exception("Key is empty");
        
        var resultCipher = new byte[Const.AesKeySize];

        _aes.Mode = CipherMode.ECB;
        using (var aesEncryptor = _aes.CreateEncryptor(this._key, new byte[Const.AesKeySize]))
        {
            aesEncryptor.TransformBlock(data, 0, Const.AesKeySize, resultCipher, 0);
        }

        return resultCipher;
    }
    
    byte[] XorBytes(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
        for (int i = 0; i < first.Length; i++)
            result[i] = (byte) (first[i] ^ second[i]);
        return result;
    }
    
    void IDisposable.Dispose()
    {
        _aes?.Dispose();
    }
}