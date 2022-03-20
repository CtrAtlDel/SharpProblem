using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab;

/*IV генерируется один раз для одного подключения*/
public class Modes
{
    public const string Ecb = "ECB";
    public const string Cbc = "CBC";
    public const string Cfb = "CFB";
    public const string Ofb = "OFB";
    public const string Ctr = "CTR";
}

public class Padding
{
    public const string Pks7 = "PKS7";
    public const string Non = "NON";
}

public class Const
{
    public const int AesKeySize = 16; // 16 byte 128 bit max AES key
    public const int AesMsgSize = 16; //
    public const int AesNonceSize = 4;
    public const int AesIvSize = 16;
}

public sealed class Crypto : IDisposable
{
    private string _mode = Modes.Ecb; //default value

    private byte[] _key = null;

    private byte[] _iv = null;

    private byte[] _nonce = null;

    private byte[] _save = null;

    private Aes _aes = new AesCryptoServiceProvider();

    private bool _first = true;

    private bool _setIv = false;

    public byte[] Decrypt(Span<byte> data, byte[] iv = null)
    {
        var spanData = Span<byte>.Empty;
        if (!_setIv)
        {
            if (data.Length <= Const.AesIvSize)
                throw new Exception("Only Iv without message");

            _iv = data.Slice(0, Const.AesIvSize).ToArray();
            spanData = data.Slice(Const.AesIvSize);
        }
        else
        {
            if (iv != null)
                _iv = iv;
            spanData = data;
        }

        var resultSpan = new Span<byte>();
        _first = true;

        for (int i = 0; i < CountOfBlocks(spanData.Length); i++)
        {
            _first = i == 0;

            if (_mode == Modes.Ecb || _mode == Modes.Cbc)
            {
                bool check = isEndOfArray(spanData.ToArray(), i);
                resultSpan = Concat(resultSpan,
                    ProcessBlockDecrypt(SplitData(spanData, i), isEndOfArray(spanData.ToArray(), i), Padding.Pks7)
                        .AsSpan());
            }
            else
            {
                resultSpan = Concat(resultSpan, ProcessBlockDecrypt(SplitData(spanData, i),
                    isEndOfArray(spanData.ToArray(), i),
                    Padding.Non).AsSpan());
            }
        }

        _first = false;
        return resultSpan.ToArray();
    }

    Span<byte> Concat(Span<byte> span1, Span<byte> span2)
    {
        var newArray = new byte[span1.Length + span2.Length];
        if (!span1.IsEmpty)
        {
            for (int i = 0; i < span1.Length; i++)
            {
                newArray[i] = span1[i];
            }

            if (!span2.IsEmpty)
            {
                for (int i = span1.Length, j = 0; i < span2.Length + span1.Length; i++, j++)
                {
                    newArray[i] = span2[j];
                }
            }

            return newArray.AsSpan();
        }
        else
        {
            for (int i = 0; i < span2.Length; i++)
            {
                newArray[i] = span2[i];
            }

            return newArray.AsSpan();
        }
    }

    byte[] ProcessBlockDecrypt(byte[] data, bool isFinalBLock, string padding)
    {
        byte[] result = new byte[data.Length];
        Array.Copy(data, result, data.Length);

        if (isFinalBLock)
        {
            if (padding == Padding.Pks7)
            {
                if (data[Const.AesMsgSize - 1] == Const.AesMsgSize)
                {
                    return new Span<byte>().ToArray();
                }
            }
        }

        if (_mode == Modes.Ecb)
        {
            result = BlockCipherDecrypt(result);
        }
        else if (_mode == Modes.Cbc)
        {
            result = DecryptCbc(result);
            _save = data;
        }
        else if (_mode == Modes.Cfb)
        {
            result = EncryptCfb(result);
            _save = data;
        }
        else if (_mode == Modes.Ofb)
            result = EncryptOfb(result);
        else if (_mode == Modes.Ctr)
        {
            result = EncryptCtr(result);
            IncrementIv(_iv, Const.AesIvSize - 1);
        }
        else
            throw new Exception($"Unsupported mode [{_mode}]...");


        if (isFinalBLock)
        {
            if (padding == Padding.Pks7)
            {
                if (result[Const.AesMsgSize - 1] == Const.AesMsgSize)
                {
                    return new Span<byte>().ToArray();
                }

                var counter = result[Const.AesMsgSize - 1];
                var spanData = new Span<byte>(result);
                return spanData.Slice(0, spanData.Length - counter).ToArray();
            }

            if (_mode == Modes.Ctr)
                ClearCtr();
        }

        return result;
    }

    private void ClearCtr()
    {
        if (_iv == null)
            return;
        for (int i = Const.AesMsgSize - Const.AesNonceSize; i < Const.AesMsgSize; i++)
        {
            _iv[i] = 0;
        }
    }

    byte[] DecryptCbc(byte[] data)
    {
        if (_iv == null)
            throw new Exception("Cannot decrypt IV is null");
        if (_key == null)
            throw new Exception("Cannot find key");

        if (_first) //first block
        {
            return XorBytes(BlockCipherDecrypt(data), _iv);
        }

        return XorBytes(BlockCipherDecrypt(data), _save);
    }

    byte[] BlockCipherDecrypt(byte[] data)
    {
        if (_key == null)
            throw new Exception("Key is null...");
        if (this._key.Length == 0)
            throw new Exception("Key is empty...");

        byte[] resultCipher = new byte[Const.AesKeySize];
        _aes.Mode = CipherMode.ECB;
        _aes.Padding = PaddingMode.None;
        using (var decryptor = _aes.CreateDecryptor(_key, new byte[Const.AesKeySize]))
        {
            resultCipher = decryptor.TransformFinalBlock(data, 0, Const.AesKeySize);
        }

        return resultCipher;
    }

    public byte[] Encrypt(Span<byte> data, byte[] iv = null) //разбивка на блоки
    {
        var spanData = Span<byte>.Empty;
        if (data.IsEmpty)
            throw new Exception("Data is empty...");
        if (iv != null)
            _iv = iv;

        var result = new List<byte>();
        var resultSpan = new Span<byte>();
        _first = true;
        for (int i = 0; i < CountOfBlocks(data.Length); i++)
        {
            _first = i == 0;

            if (_mode == Modes.Ecb || _mode == Modes.Cbc)
            {
                resultSpan = Concat(resultSpan,
                    ProcessBlockEncrypt(SplitData(data, i), isEndOfArray(data.ToArray(), i), Padding.Pks7).AsSpan());
            }
            else
            {
                resultSpan = Concat(resultSpan,
                    ProcessBlockEncrypt(SplitData(data, i), isEndOfArray(data.ToArray(), i), Padding.Non).AsSpan());
            }
        }

        _first = false;
        return resultSpan.ToArray();
    }

    byte[] SplitData(Span<byte> data, int index)
    {
        if (isEndOfArray(data.ToArray(), index))
        {
            return data.Slice(index * Const.AesMsgSize, data.Length - index * Const.AesMsgSize).ToArray();
        }

        return data.Slice(index * Const.AesMsgSize, Const.AesMsgSize).ToArray();
    }

    byte[] ProcessBlockEncrypt(byte[] data, bool isFinalBLock, string padding)
    {
        byte[] result = new byte[data.Length];

        Array.Copy(data, result, data.Length);
        byte[] resultPadding = new byte[Const.AesMsgSize];

        if (isFinalBLock) //помнить про то, что нужно дополнять также и блок размером 16 
        {
            if (padding == Padding.Pks7)
            {
                if (data.Length == Const.AesMsgSize)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        resultPadding[i] = (byte) (Const.AesMsgSize);
                    }

                    resultPadding = BlockCipherEncrypt(resultPadding);
                }
                else
                {
                    result = Pks7(result);
                }
            }
        }

        if (_mode == Modes.Ecb)
        {
            result = BlockCipherEncrypt(result);
        }
        else if (_mode == Modes.Cbc)
        {
            _save = EncryptCbc(result);
            result = _save;
        }
        else if (_mode == Modes.Cfb)
        {
            _save = EncryptCfb(result);
            result = _save;
        }
        else if (_mode == Modes.Ofb)
        {
            result = EncryptOfb(result);
        }
        else if (_mode == Modes.Ctr)
        {
            result = EncryptCtr(result);
            IncrementIv(_iv, Const.AesMsgSize - 1);
        }
        else
            throw new Exception($"Unsupported mode [{_mode}]...");


        if (isFinalBLock)
        {
            if (padding == Padding.Non)
            {
                return Non(result, data.Length);
            }
        }

        if (isFinalBLock)
        {
            if (padding == Padding.Pks7)
            {
                if (data.Length == Const.AesMsgSize)
                {
                    return result.Concat(resultPadding).ToArray();
                }
            }
        }

        return result;
    }


    byte[] Pks7(byte[] data)
    {
        int oldLength = data.Length;

        Array.Resize(ref data, Const.AesMsgSize);

        for (int i = oldLength; i < data.Length; i++)
        {
            data[i] = (byte) (Const.AesMsgSize - oldLength);
        }

        return data;
    }

    static byte[] Non(byte[] data, int length)
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

        return BlockCipherEncrypt(XorBytes(data, _save));
    }

    byte[] EncryptCfb(byte[] data)
    {
        if (_key == null)
            throw new Exception("Cannot find key");

        if (_iv == null || _first)
        {
            if (_iv == null)
                GenerateIv();
            return XorBytes(data, BlockCipherEncrypt(_iv));
        }

        return XorBytes(data, BlockCipherEncrypt(_save));
    }

    byte[] EncryptOfb(byte[] data)
    {
        if (_key == null)
            throw new Exception("Cannot find key");

        if (_iv == null || _first)
        {
            if (_iv == null)
                GenerateIv();
            _save = BlockCipherEncrypt(_iv);
            return XorBytes(data, _save);
        }

        _save = BlockCipherEncrypt(_save);
        return XorBytes(data, _save);
    }

    byte[] EncryptCtr(byte[] data)
    {
        if (_iv == null)
        {
            GenerateIv();
            GenerateNonce();
            _iv = CreateIvNonce();
            return XorBytes(data, BlockCipherEncrypt(_iv));
        }

        return XorBytes(data, BlockCipherEncrypt(_iv));
    }

    static void IncrementIv(byte[] dataBytes, int index)
    {
        if (index < 0)
            throw new Exception("Index out of range...");

        if (dataBytes[index] == byte.MaxValue)
        {
            dataBytes[index] = 0;
            if (index > 0)
                IncrementIv(dataBytes, index - 1);
        }
        else
            dataBytes[index]++;
    }

    byte[] CreateIvNonce()
    {
        if (_iv == null)
            throw new Exception("Iv is empty...");
        if (_nonce == null)
            throw new Exception("Nonce is empty...");

        int j = Const.AesIvSize - 1;
        for (int i = Const.AesNonceSize - 1; i >= 0; i--)
        {
            _iv[i] = _nonce[i];
            _iv[j] = 0;
            --j;
        }

        return _iv;
    }

    byte[] XorBytes(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
        for (int i = 0; i < first.Length; i++)
            result[i] = (byte) (first[i] ^ second[i]);
        return result;
    }

    void GenerateIv()
    {
        byte[] iv = new byte[Const.AesIvSize];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(iv);
            _iv = iv;
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
        if (_key == null)
            throw new Exception("Key is null");

        if (this._key.Length == 0)
            throw new Exception("Key is empty");

        var resultCipher = new byte[Const.AesKeySize];
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
        if (mode == Modes.Ctr || mode == Modes.Cbc || mode == Modes.Cfb || mode == Modes.Ecb || mode == Modes.Ofb)
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

    public void SetIV(bool choice)
    {
        _setIv = choice;
    }

    public byte[] MsgToByte(string msg) // translate string msg to byte[] msg
    {
        return Encoding.UTF8.GetBytes(msg);
    }

    public string ByteToMsg(byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }

    //Utilits for testing 
    public byte[] GetIv()
    {
        if (_iv == null)
            throw new Exception("Iv is empty");

        return _iv;
    }

    private void GenerateNonce()
    {
        _nonce = GenerateRandom(Const.AesNonceSize);
    }

    public byte[] GenerateRandom(int length)
    {
        if (length < 0)
            throw new Exception("Length < 0 ");
        byte[] bytes = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }

        return bytes;
    }

    void IDisposable.Dispose()
    {
        _aes?.Dispose();
    }
}