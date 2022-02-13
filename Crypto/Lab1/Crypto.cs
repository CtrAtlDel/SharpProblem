using System.Reflection.Metadata;

namespace CryptoLab;

public class Crypto
{
    private byte[] key;

    private string mode;

    void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.sizeBytes)
        {
            this.key = key;
        }

        throw new Exception("Key size not equal 128 bytes");
    }

    void SetMode(string mode) //указание режима шифрования
    {
        if (mode == Mode.CT || mode == Mode.CBC || mode == Mode.CFB || mode == Mode.ECB || mode == Mode.OFB)
        {
            this.mode = mode;
        }

        throw new Exception("You mode is ");
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding)
    {
        return data;
    }

    byte[] BlockCipherEncrypt(byte[] data)
    {
        
        return data;
    }
}