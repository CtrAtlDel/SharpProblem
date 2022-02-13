using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab;

public class Crypto
{
    private byte[] key;

    private string mode = "ECB"; //CT CBC CFB ECB OFB

    void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.SizeBytes)
        {
            this.key = key;
        }
        else
        {
            throw new Exception("Key size not equal 128 bytes");
        }
    }

    void SetMode(string mode) //указание режима шифрования
    {
        if (mode == Modes.CT || mode == Modes.CBC || mode == Modes.CFB || mode == Modes.ECB || mode == Modes.OFB)
        {
            this.mode = mode;
        }
        else
        {
            throw new Exception("You mode is  undeclarated");
        }
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding)
    {
        // if this block is final use padding
        if (padding != Modes.PKS7 || padding != Modes.NON)
        {
            throw new Exception("You padding is undeclarated");
        }

        if (data.Length != Const.SizeBytes)
        {
            throw new Exception("Data length is not 128 byte");
        }

        if (isFinalBLock) //use padding
        {
            
        }

        byte[] resultEncrypt = new byte[Const.SizeBytes];
        resultEncrypt = BlockCipherEncrypt(data);
        return resultEncrypt;
    }

    byte[] BlockCipherEncrypt(byte[] data)
    {
        if (this.key.Length == 0)
        {
            throw new Exception("Key is null");
        }

        if (data.Length != key.Length)
        {
            throw new Exception("Data length not equal key length");
        }

        byte[] resultCipher = new byte[Const.SizeBytes];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesEncryptor = aes.CreateEncryptor(this.key, new byte[Const.SizeBytes]))
            {
                aesEncryptor.TransformBlock(data, 0, Const.SizeBytes, resultCipher, 0);
            }
        }

        return resultCipher;
    }

    // byte[] Encrypt(byte[] data, byte[] iv = null)
    // {
    // }

    // byte[] Decrypt(byte[] data, byte[] iv = null)
    // {
    // }

    byte[] MsgToByte(string msg) // translate string msg to byte[] msg
    {
        return Encoding.UTF8.GetBytes(msg);
    }

    string ByteToMsg(byte[] data)
    {
        return BitConverter.ToString(data);
    }
}