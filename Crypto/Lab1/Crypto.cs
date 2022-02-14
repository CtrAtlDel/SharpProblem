using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab;

public class Modes
{
    public const string ECB = "ECB";
    public const string CBC = "CBC";
    public const string CFB = "CFB";
    public const string OFB = "CFB";
    public const string CTR = "CTR";
    public const string PKS7 = "PKS7";
    public const string NON = "NON";
}

public class Const
{
    public const int Bytes = 128; // 128 bytes max
    public const int SizeMode = 5; // count of modes
}

public class Crypto
{
    private byte[] key;

    private string mode = Modes.ECB; //CT CBC CFB ECB OFB

    byte[] Encrypt(byte[] data, byte[] iv = null)
    {
        if (this.mode == Modes.ECB)
        {
            // use without patting, default
        }

        if (this.mode == Modes.CTR)
        {
            // patting = NON
        }

        if (this.mode == Modes.CBC)
        {
            // patting = PKCS7
        }

        if (this.mode == Modes.CFB)
        {
            // patting = NON
        }

        if (this.mode == Modes.OFB)
        {
            //patting = NON
        }

        return null;
    }

    byte[] Decrypt(byte[] data, byte[] iv = null)
    {
        if (this.mode == Modes.ECB)
        {
            // 
        }

        if (this.mode == Modes.CTR)
        {
        }

        if (this.mode == Modes.CBC)
        {
        }

        if (this.mode == Modes.CFB)
        {
        }

        if (this.mode == Modes.OFB)
        {
        }

        return null;
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding)
    {
        // if this block is final use padding
        if (padding != Modes.PKS7 || padding != Modes.NON)
            throw new Exception("You padding is undeclarated");

        if (data.Length != Const.Bytes)
            throw new Exception("Data length is not 128 byte");
        //TODO ProcessBlockDecrypt
        byte[] resultEncrypt = new byte[Const.Bytes];
        resultEncrypt = BlockCipherEncrypt(data);
        return resultEncrypt;
    }

    byte[] ProcessBlockDecrypt(byte[] data, bool isFinalBlock, string padding)
    {
        return null;
    }

    byte[] BlockCipherDecrypt(byte[] data)
    {
        if (this.key.Length == 0)
            throw new Exception("Key is null");
        byte[] blockCipherDecrypt = new byte[Const.Bytes];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesDecryptor = aes.CreateDecryptor(this.key, new byte[Const.Bytes]))
            {
                aesDecryptor.TransformBlock(data, 0, Const.Bytes, blockCipherDecrypt, 0);
            }
        }

        return blockCipherDecrypt;
    }

    byte[] BlockCipherEncrypt(byte[] data)
    {
        if (this.key.Length == 0)
        {
            throw new Exception("Key is null");
        }

        byte[] resultCipher = new byte[Const.Bytes];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesEncryptor = aes.CreateEncryptor(this.key, new byte[Const.Bytes]))
            {
                aesEncryptor.TransformBlock(data, 0, Const.Bytes, resultCipher, 0);
            }
        }

        return resultCipher;
    }

    void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.Bytes)
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
        if (mode == Modes.CTR || mode == Modes.CBC || mode == Modes.CFB || mode == Modes.ECB || mode == Modes.OFB)
        {
            this.mode = mode;
        }
        else
        {
            throw new Exception("You mode is  undeclarated");
        }
    }

    private byte[] MsgToByte(string msg) // translate string msg to byte[] msg
    {
        return Encoding.UTF8.GetBytes(msg);
    }

    private string ByteToMsg(byte[] data)
    {
        return BitConverter.ToString(data);
    }
}