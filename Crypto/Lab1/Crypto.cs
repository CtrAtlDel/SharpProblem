using System.Data;
using System.Dynamic;
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
}

public class Padding
{
    public const string PKS7 = "PKS7";
    public const string NON = "NON";
}

public class Const
{
    public const int AesKeySize = 128; // 128 bytes max AES key
    public const int AesMsgSize = 256;
    public const int AesMsgSize64 = 64;
    public const int AesMsgSize128 = 128;
    public const int AesMsgSize256 = 256;
    public const int SizeMode = 5; // count of modes
}

public class Crypto
{
    private byte[] key = null;

    private string mode = Modes.ECB; //CT CBC CFB ECB OFB

    byte[] Encrypt(byte[] data, byte[] iv = null)
    {
        if (this.mode == Modes.ECB)
            return EncryptECB(data);

            if (this.mode == Modes.CTR)
        {
            //use IV vector
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

    byte[] EncryptECB(byte[] data)
    {
        var spanData = new Span<byte>(data);
        byte[] result = Array.Empty<byte>();
        for (int i = 0; i < CountOfBlocks(data.Length); i++) // -> to alone metode
        {
            if (!isEndOfArray(data, i)) // get a part of 
            {
                var spanSlice = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize);
                result.Concat(ProcessBlockEncrypt(spanSlice.ToArray(), false, Padding.NON)); 
                continue;
            }
            // if is end
            var endOfData = spanData.Slice(i * Const.AesMsgSize, spanData.Length - i * Const.AesMsgSize);
            result.Concat(ProcessBlockEncrypt(endOfData.ToArray(), true, Padding.NON));
        }

        return result;
    }

    bool isEndOfArray(byte[] data, int index)
    {
        if (index == CountOfBlocks(data.Length) - 1)
        {
            return true;
        }

        return false;
    }

    static int CountOfBlocks(int size)
    {
        if (size % Const.AesMsgSize == 0)
        {
            return size / Const.AesMsgSize;
        }

        return size / Const.AesMsgSize + 1;
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
        if (padding != Padding.PKS7 || padding != Padding.NON)
            throw new Exception("You padding is not declared");

        if (data.Length != Const.AesKeySize)
            throw new Exception("Data length is not 128 byte");

        byte[] resultEncrypt = BlockCipherEncrypt(data);

        //TODO ProcessBlockDecrypt

        if (isFinalBLock)
        {
            //use padding for our key
        }

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

        byte[] blockCipherDecrypt = new byte[Const.AesKeySize];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesDecryptor = aes.CreateDecryptor(this.key, new byte[Const.AesKeySize]))
            {
                aesDecryptor.TransformBlock(data, 0, Const.AesKeySize, blockCipherDecrypt, 0);
            }
        }

        return blockCipherDecrypt;
    }

    byte[] BlockCipherEncrypt(byte[] data)
    {
        if (this.key.Length == 0)
            throw new Exception("Key is null");

        byte[] resultCipher = new byte[Const.AesKeySize];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesEncryptor = aes.CreateEncryptor(this.key, new byte[Const.AesKeySize]))
            {
                aesEncryptor.TransformBlock(data, 0, Const.AesKeySize, resultCipher, 0);
            }
        }

        return resultCipher;
    }

    void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.AesKeySize)
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