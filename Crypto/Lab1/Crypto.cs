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
    public const int AesMsgSize = 16;
    public const int SizeMode = 5; // count of modes
}

public class Crypto
{
    private byte[] _key = null;

    private string _mode = Modes.ECB; //CT CBC CFB ECB OFB

    private byte[] _iv = null;

    private int _counter = 0;

    byte[] Encrypt(byte[] data, byte[] iv = null)
    {
        if (this._mode == Modes.ECB)
            return EncryptECB(data);

        if (this._mode == Modes.CTR)
        {
            //use IV vector
        }

        if (this._mode == Modes.CBC)
        {
            // patting = PKCS7
        }

        if (this._mode == Modes.CFB)
        {
            // patting = NON
        }

        if (this._mode == Modes.OFB)
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
                result.AddRange(ProcessBlockEncrypt(spanSlice.ToArray(), false, Padding.PKS7));
                continue;
            }

            var endOfData = spanData.Slice(i * Const.AesMsgSize, spanData.Length - i * Const.AesMsgSize);
            result.AddRange(ProcessBlockEncrypt(endOfData.ToArray(), true, Padding.PKS7));
        }

        return result.ToArray();
    }

    byte[] EncryptCBC(byte[] data, byte[] iv)
    {
        //TODO maybe throw some exception
        if (data.Length == 0)
            throw new Exception("Empty data array");

        var spanData = new Span<byte>(data);
        var result = new List<byte>();

        if (iv == null || iv.Length == 0)
        {
            GenerateIV();
            iv = this._iv;
        }
        else
            this._iv = iv;
        
        var plainTextFirst = spanData.Slice(0, Const.AesMsgSize);
        var firstBlock = XorBytes(plainTextFirst.ToArray(), this._iv);
        if (isEndOfArray(data, 0))
        {
            result.AddRange(ProcessBlockEncrypt(firstBlock, true, Padding.PKS7));
            return result.ToArray();
        }

        var cipherText = ProcessBlockEncrypt(firstBlock, false, Padding.PKS7);
        result.AddRange(cipherText);
        
        for (int i = 1; i < CountOfBlocks(data.Length); i++)
        {
            if (isEndOfArray(data, i))
            {
                var endOfData = spanData.Slice(i * Const.AesMsgSize, spanData.Length - i * Const.AesMsgSize);
                var tmpData = ProcessBlockEncrypt(endOfData.ToArray(), true, Padding.PKS7);
                result.AddRange(cipherText);
                return result.ToArray();
            }

            var plainText = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize); // a pies of data
            var xorData = XorBytes(plainText.ToArray(),cipherText);
            cipherText = ProcessBlockEncrypt(xorData, false, Padding.PKS7);
            result.AddRange(cipherText);
        }

        return result.ToArray();
    }

    byte[] EncryptCFB(byte[] data, byte[] iv)
    {
        //TODO maybe throw some exception
        if (data.Length == 0)
            throw new Exception("Empty data array");
        var spanData = new Span<byte>(data);
        var result = new List<byte>();

        if (iv == null || iv.Length == 0)
        {
            GenerateIV();
            iv = this._iv;
        }
        else
            this._iv = iv;
    
        var ivEncrypt = ProcessBlockEncrypt(this._iv, false, Padding.NON);
        var plainTextFirst = spanData.Slice(0, Const.AesMsgSize);
        if (isEndOfArray(data,0))
        {
            return XorBytes(plainTextFirst.ToArray(), ivEncrypt);
        }

        var cipherText = XorBytes(plainTextFirst.ToArray(), ivEncrypt);
        result.AddRange(cipherText);
        for (int i = 1; i < CountOfBlocks(data.Length); i++)
        {
            if (isEndOfArray(data, i))
            {
                cipherText = ProcessBlockEncrypt(cipherText, true, Padding.NON);
                var plainTextEnd = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize); // a pies of data
                var xorDataEnd = XorBytes(plainTextEnd.ToArray(), cipherText);
                result.AddRange(xorDataEnd);
                return result.ToArray();
            }
            cipherText = ProcessBlockEncrypt(cipherText, false, Padding.NON);
            var plainText = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize); // a pies of data
            var xorData = XorBytes(plainText.ToArray(), cipherText);
            result.AddRange(xorData);
            cipherText = xorData;
        }
        return result.ToArray();
    }

    byte[] EncryptEFB(byte[] data, byte[] iv)
    {
        return null;
    }

    byte[] XorBytes(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
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
            this._iv = myRj.IV;
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
        if (this._key.Length == 0)
            throw new Exception("Key is null");

        byte[] resultCipher = new byte[Const.AesKeySize];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesEncryptor = aes.CreateEncryptor(this._key, new byte[Const.AesKeySize]))
            {
                aesEncryptor.TransformBlock(data, 0, Const.AesKeySize, resultCipher, 0);
            }
        }

        return resultCipher;
    }

    byte[] Decrypt(byte[] data, byte[] iv = null)
    {
        if (this._mode == Modes.ECB)
        {
            // 
        }

        if (this._mode == Modes.CTR)
        {
        }

        if (this._mode == Modes.CBC)
        {
        }

        if (this._mode == Modes.CFB)
        {
        }

        if (this._mode == Modes.OFB)
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
        if (this._key.Length == 0)
            throw new Exception("Key is null");

        byte[] blockCipherDecrypt = new byte[Const.AesKeySize];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesDecryptor = aes.CreateDecryptor(this._key, new byte[Const.AesKeySize]))
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
            this._key = key;
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
            this._mode = mode;
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