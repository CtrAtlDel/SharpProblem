using System.Data;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;

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
    public const int AesMsgSize = 16;
    public const int SizeMode = 5; // count of modes
}

public class Crypto
{
    private byte[] key = null;

    private string mode = Modes.ECB; //CT CBC CFB ECB OFB

    private byte[] IV = null;

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
        var result = new List<byte>();

        for (int i = 0; i < CountOfBlocks(data.Length); i++)
        {
            if (!isEndOfArray(data, i)) // get a part of data
            {
                var spanSlice = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize);
                result.AddRange(ProcessBlockEncrypt(spanSlice.ToArray(), false, Padding.NON));
                continue;
            }

            var endOfData = spanData.Slice(i * Const.AesMsgSize, spanData.Length - i * Const.AesMsgSize);
            result.AddRange(ProcessBlockEncrypt(endOfData.ToArray(), true, Padding.NON));
        }

        return result.ToArray();
    }

    byte[] EncryptCBC(byte[] data, byte[] iv)
    {
        //TODO maybe throw some exception
        var spanData = new Span<byte>(data);
        var result = new List<byte>();
        
        if (iv == null || iv.Length == 0) //???
        {
            GenerateIV();
            iv = this.IV;
        }
        else
            this.IV = iv;
        //TODO point
        // result.AddRange(firstCBCEncrypt());
        
        for (int i = 1; i < CountOfBlocks(data.Length); i++)
        {
            if (!isEndOfArray(data, i))
            {
                var spanSlice = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize);
                
            }
            else
            {
                
            }
        }
        return result.ToArray();
    }

    byte[] firstCBCEncrypt(byte[] data, byte[] IV)
    {
        return XorBytes(data, IV);
    }

    byte[] XorBytes(byte[] first, byte[] second)
    {
        if (first.Length != second.Length)
            throw new Exception("Can't do XOR, different length");
        byte[] result = new byte[first.Length];
        for (int i = 0; i < first.Length; i++)
        {
            result[i] = (byte) (first[i] ^ second[i]);
        }

        return result;
    }

    void GenerateIV() // sizeIV = sizeDataBlock it is 
    {
        //TODO ask about size IV
        using (var myRj = new AesCryptoServiceProvider())
        {
            myRj.GenerateIV();
            this.IV = myRj.IV;
        }
    }

    bool isEndOfArray(byte[] data, int index)
    {
        if (index == CountOfBlocks(data.Length) - 1)
            return true;

        return false;
    }

    static int CountOfBlocks(int lenghtData)
    {
        if (lenghtData % Const.AesMsgSize == 0)
            return lenghtData / Const.AesMsgSize;
        return lenghtData / Const.AesMsgSize + 1;
    }


    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding)
    {
        if (padding != Padding.PKS7 || padding != Padding.NON)
            throw new Exception("You padding is not declared");

        if (data.Length != Const.AesKeySize)
            throw new Exception("Data length is not 128 byte");

        byte[] resultEncrypt = BlockCipherEncrypt(data);

        if (isFinalBLock)
        {
            //use padding for our data
            //TODO add if with padding
        }

        return resultEncrypt;
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