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

    private byte[] _save = null;

    byte[] Encrypt(byte[] data, byte[] iv = null) //разбивка на блоки
    {
        if (data == null)
            throw new Exception("Data is empty...");

        var spanData = new Span<byte>(data);
        var result = new List<byte>();

        for (int i = 0; i < CountOfBlocks(data.Length); i++)
        {
            if (this._mode == Modes.ECB || this._mode == Modes.CBC)
            {
                result.AddRange(ProcessBlockEncrypt(SplitData(spanData, i), isEndOfArray(data, i), Padding.PKS7));
            }
            else
            {
                result.AddRange(ProcessBlockEncrypt(SplitData(spanData, i), isEndOfArray(data, i), Padding.NON));
            }
        }

        return result.ToArray();
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding) //обработка каждого блока
    {
        if (padding != Padding.PKS7 || padding != Padding.NON)
            throw new Exception("You padding is not declared");

        if (data.Length != Const.AesKeySize)
            throw new Exception("Data length is not 128 byte");

        if (isFinalBLock) //помнить про то, что нужно дополнять также и блок размером 16 
        {
            //Modifies data
            if (padding == Padding.PKS7)
            {
                //TODO padding PKS7
            }
            else // Padding NON
            {
                //TODO padding NON
            }
        }

        // передается только блок!

        if (_mode == Modes.ECB)
        {
            return BlockCipherEncrypt(data);
        }

        if (_mode == Modes.CBC)
        {
            _save = EncryptCbc(data); //Как узнать что блок первый? 
            return _save;
        }

        if (_mode == Modes.CFB)
        {
            _save = EncryptCfb(data);
            return _save;
        }

        if (_mode == Modes.OFB)
        {
            
        }
        
        byte[] resultEncrypt = BlockCipherEncrypt(data);


        return resultEncrypt;
    }

    byte[] SplitData(Span<byte> data, int index)
    {
        if (isEndOfArray(data.ToArray(), index))
        {
            return data.Slice(index * Const.AesMsgSize, data.Length - index * Const.AesMsgSize).ToArray();
        }

        return data.Slice(index * Const.AesMsgSize, Const.AesMsgSize).ToArray();
    }

    // byte[] EncryptECB(byte[] data)
    // {
    //     var spanData = new Span<byte>(data);
    //     var result = new List<byte>();
    //
    //     for (int i = 0; i < CountOfBlocks(data.Length); i++)
    //     {
    //         if (!isEndOfArray(data, i)) // get a part of data
    //         {
    //             var spanSlice = spanData.Slice(i * Const.AesMsgSize, Const.AesMsgSize);
    //             result.AddRange(ProcessBlockEncrypt(spanSlice.ToArray(), false, Padding.PKS7));
    //             continue;
    //         }
    //
    //         var endOfData = spanData.Slice(i * Const.AesMsgSize, spanData.Length - i * Const.AesMsgSize);
    //         result.AddRange(ProcessBlockEncrypt(endOfData.ToArray(), true, Padding.PKS7));
    //     }
    //
    //     return result.ToArray();
    // }

    byte[] EncryptCbc(byte[] data) //уже дополненный блок 
    {
        if (_iv == null) //first block
        {
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
        if (_iv == null) //first blocok
        {
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
        if (_iv == null)
        {
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

    byte[] EncryptCBC(byte[] data, byte[] iv)
    {
        // TODO maybe throw some exception

        if (data.Length == 0)
            throw new Exception("Empty data array");

        var spanData = new Span<byte>(data);
        var result = new List<byte>();

        if (iv == null || iv.Length == 0)
        {
            GenerateIv();
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
            var xorData = XorBytes(plainText.ToArray(), cipherText);
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
            GenerateIv();
            iv = this._iv;
        }
        else
            this._iv = iv;

        var ivEncrypt = ProcessBlockEncrypt(this._iv, false, Padding.NON);
        var plainTextFirst = spanData.Slice(0, Const.AesMsgSize);
        if (isEndOfArray(data, 0))
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

    byte[] XorBytes(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
        for (int i = 0; i < first.Length; i++)
        {
            result[i] = (byte) (first[i] ^ second[i]);
        }

        return result;
    }

    void GenerateIv() // sizeIV = sizeDataBlock it is 
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


    void SetKey(byte[] key) //установка ключа шифрования\расшифрования
    {
        if (key.Length == Const.AesKeySize)
        {
            this._key = key;
            _iv = null;
            _save = null;
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
            _mode = mode;
            _iv = null;
            _save = null;
        }
        else
        {
            throw new Exception("You mode is not declarated");
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