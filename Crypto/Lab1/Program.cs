using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab
{
    public class Mode
    {
        public const int sizeModes = 5;
        public const string ECB = "ECB";
        public const string CBC = "CBC";
        public const string CFB = "CFB";
        public const string OFB = "CFB";
        public const string CT = "CT";
    }

    public class Const
    {
        public const int sizeBytes = 128; // 128 bytes max

        public const int sizeMode = 5; // count of modes
    }

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

            throw new Exception("Key size != 128 bytes");
        }

        void SetMode(string mode) //указание режима шифрования
        {
            if (mode == Mode.CT || mode == Mode.CBC || mode == Mode.CFB || mode == Mode.ECB || mode == Mode.OFB)
            {
                this.mode = mode;
            }

            throw new Exception("I dont know what is your mode");
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

    internal class Programm
    {
        public static void Main(string[] args)
        {
        }
    }
};