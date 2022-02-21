using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab;

/*IV генерируется один раз для одного подключения*/
public class Modes
{
    public const string ECB = "ECB";
    public const string CBC = "CBC";
    public const string CFB = "CFB";
    public const string OFB = "OFB";
    public const string CTR = "CTR";
}

public class Padding
{
    public const string PKS7 = "PKS7";
    public const string NON = "NON";
}

public class Const
{
    public const int AesKeySize = 16; // 16 byte 128 bit max AES key
    public const int AesMsgSize = 16; // 
    public const int SizeMode = 5; // count of modes
}

public class Crypto
{
    private string _mode = Modes.ECB; //CT CBC CFB ECB OF

    private byte[] _key = null;

    private byte[] _iv = null;

    private int _counter = 0; // size is 128 bits -> 4 bytes

    private byte[] _save = null;

    private bool _first = true;

    // save null parametr is first he is null

    public byte[] Encrypt(byte[] data, byte[] iv = null) //разбивка на блоки
    {
        if (data == null)
            throw new Exception("Data is empty...");
        if (iv != null)
        {
            _iv = iv;
        }

        var spanData = new Span<byte>(data);
        var result = new List<byte>();
        _first = true;
        for (int i = 0; i < CountOfBlocks(data.Length); i++)
        {
            if (i == 0) //chek this
                _first = true;
            else
                _first = false;

            if (this._mode == Modes.ECB || this._mode == Modes.CBC)
            {
                result.AddRange(ProcessBlockEncrypt(SplitData(spanData, i), isEndOfArray(data, i), Padding.PKS7));
            }
            else
            {
                result.AddRange(ProcessBlockEncrypt(SplitData(spanData, i), isEndOfArray(data, i), Padding.NON));
            }
        }

        _first = false;
        return result.ToArray();
    }

    byte[] SplitData(Span<byte> data, int index)
    {
        if (isEndOfArray(data.ToArray(), index))
        {
            return data.Slice(index * Const.AesMsgSize, data.Length - index * Const.AesMsgSize).ToArray();
        }

        return data.Slice(index * Const.AesMsgSize, Const.AesMsgSize).ToArray();
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding) //обработка каждого блока
    {
        byte[] result = new byte[data.Length];
        Array.Copy(data, result, data.Length);

        if (isFinalBLock) //помнить про то, что нужно дополнять также и блок размером 16 
        {
            //Modifies data
            if (padding == Padding.PKS7)
            {
                result = Pks7(result);
            }
        }

        // передается только блок!

        if (_mode == Modes.ECB)
        {
            return BlockCipherEncrypt(result);
        }

        if (_mode == Modes.CBC)
        {
            _save = EncryptCbc(result); //Как узнать что блок первый? 
            result = _save;
        }

        if (_mode == Modes.CFB)
        {
            _save = EncryptCfb(result);
            result = _save;
        }

        if (_mode == Modes.OFB)
        {
            result = EncryptOfb(result);
        }

        if (_mode == Modes.CTR)
        {
            
        }

        if (isFinalBLock)
        {
            if (padding == Padding.NON)
            {
                result = Non(result, data.Length);
            }
        }

        return result;
    }

    byte[] Pks7(byte[] data)
    {
        if (Const.AesMsgSize > 255)
            throw new Exception("Data length > 255 (huge length)");

        int oldLength = data.Length;
        if (data.Length == Const.AesMsgSize)
        {
            Array.Resize(ref data, 2 * Const.AesMsgSize);
        }
        else
        {
            Array.Resize(ref data, Const.AesMsgSize);
        }

        for (int i = oldLength; i < data.Length; i++)
        {
            data[i] = (byte) (Const.AesMsgSize - oldLength);
        }

        return data;
    }

    byte[] Non(byte[] data, int length)
    {
        var list = new Span<byte>(data);
        return list.Slice(0, length).ToArray();
    }

    byte[] EncryptCbc(byte[] data) 
    {
        if (_iv == null || _first) //first block
        {
            if (_iv == null)
                GenerateIv();
            return BlockCipherEncrypt(XorBytes(data, _iv));
        }
        else
        {
            return BlockCipherEncrypt(XorBytes(data, _save));
        }
    }

    byte[] EncryptCfb(byte[] data)
    {
        if (_iv == null || _first) //first blocok
        {
            if (_iv == null)
                GenerateIv();
            return XorBytes(data, BlockCipherEncrypt(_iv));
        }
        else
        {
            return XorBytes(data, BlockCipherEncrypt(_save));
        }
    }

    byte[] EncryptOfb(byte[] data)
    {
        if (_iv == null || _first)
        {
            if (_iv == null)
                GenerateIv();
            _save = BlockCipherEncrypt(_iv);
            return XorBytes(data, _save);
        }
        else
        {
            _save = BlockCipherEncrypt(_save);
            return XorBytes(data, _save);
        }
    }

    byte[] EncryptCtr(byte[] data)
    {
        return data;
    }

    byte[] XorBytes(byte[] first, byte[] second)
    {
        // if (second.Length > first.Length)
        //     throw new Exception("Length second argument > Length first argument");
        
        var result = new byte[first.Length];
        for (int i = 0; i < first.Length; i++)
        {
            result[i] = (byte) (first[i] ^ second[i]);
        }

        return result;
    }

    void GenerateIv() // sizeIV = sizeDataBlock it is 
    {
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

    byte[] BlockCipherEncryptFinal(byte[] data)
    {
        if (_key == null)
            throw new Exception("Key is null");
        
        if (this._key.Length == 0)
            throw new Exception("Key is empty");

        byte[] resultCipher = new byte[Const.AesKeySize];

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.ECB;
            using (var aesEncryptor = aes.CreateEncryptor(this._key, new byte[Const.AesKeySize]))
            {
                resultCipher = aesEncryptor.TransformFinalBlock(data, 0, Const.AesKeySize);
            }
        }

        return resultCipher;
    }

    byte[] BlockCipherEncrypt(byte[] data)
    {
        if (_key == null)
            throw new Exception("Key is null");
        
        if (this._key.Length == 0)
            throw new Exception("Key is empty");

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


    public void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.AesKeySize)
        {
            this._key = key;
            _iv = null;
            _save = null;
        }
        else
        {
            throw new Exception("Key size not equal 16 bytes");
        }
    }

    public void SetMode(string mode) //указание режима шифрования
    {
        if (mode == Modes.CTR || mode == Modes.CBC || mode == Modes.CFB || mode == Modes.ECB || mode == Modes.OFB)
        {
            _mode = mode;
            _iv = null;
            _save = null;
        }
        else
        {
            throw new Exception("You mode is not declarated");
        }
    }

    //TODO change private -> public
    public byte[] MsgToByte(string msg) // translate string msg to byte[] msg
    {
        return Encoding.UTF8.GetBytes(msg);
    }

    public string ByteToMsg(byte[] data)
    {
        return BitConverter.ToString(data);
    }

    //Utilits generate key

    public byte[] GetIv()
    {
        return _iv;
    }

    public byte[] GenerateKey()
    {
        byte[] bytes = new byte[16];
        var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return bytes;

    }
}