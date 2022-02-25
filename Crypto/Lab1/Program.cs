using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CryptoLab
{
    internal class Programm
    {
        static void printArray(byte[] data)
        {
            Console.Out.WriteLine($"Length: {data.Length}");
            for (int i = 0; i < data.Length; i++)
            {
                Console.Out.Write($"{data[i]} ");
            }

            Console.Out.WriteLine("");
        }

        static void testEcb() // done.
        {
            Console.Out.WriteLine("Test ECB");
            var crypter = new Crypto();
            crypter.SetMode("ECB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec770233009113871qrrtq4508731450987130485178ce7"); //"f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            Console.Out.WriteLine("Data: ");
            Console.Out.WriteLine(crypter.ByteToMsg(data).Replace("-", ""));
            var answer = crypter.Encrypt(data);
            var answerString = crypter.ByteToMsg(crypter.Encrypt(data));

            var cryptString = crypter.Encrypt(data);

            var decryptByte = crypter.Decrypt(cryptString);
            string decryptString = crypter.ByteToMsg(decryptByte);
            Console.Out.WriteLine("Crypt: ");
            Console.WriteLine(answerString.Replace("-", ""));
            Console.WriteLine("Decrypt: ");
            Console.WriteLine(decryptString.Replace("-", ""));
            Console.WriteLine();
            Console.Out.WriteLine("Data:");
            printArray(data);
            Console.Out.WriteLine("Decrypt:");
            printArray(decryptByte);
        }

        static void testCbc() //done.
        {
            Console.Out.WriteLine("Test CBC");
            var crypter = new Crypto();
            crypter.SetMode("CBC");
            byte[] key =
            {
                0x14, 0x0b, 0x41, 0xb2, 0x2a, 0x29, 0xbe, 0xb4, 0x06, 0x1b, 0xda, 0x66, 0xb6, 0x74, 0x7e, 0x14
            };
            // Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            crypter.SetKey(key);
            byte[] data =
            {
                0x5b, 0x68, 0x62, 0x9f, 0xeb, 0x86, 0x06, 0xf9, 0xa6, 0x66, 0x76, 0x70, 0xb7, 0x5b, 0x38, 0xa5,
                0xb4, 0x83, 0x2d, 0x0f, 0x26, 0xe1, 0xab, 0x7d, 0xa3, 0x32, 0x49, 0xde, 0x7d, 0x4a, 0xfc, 0x48,
                0xe7, 0x13, 0xac, 0x64, 0x6a, 0xce, 0x36, 0xe8, 0x72, 0xad, 0x5f, 0xb8, 0xa5, 0x12, 0x42, 0x8a,
                0x6e, 0x21, 0x36, 0x4b, 0x0c, 0x37, 0x4d, 0xf4, 0x55, 0x03, 0x47, 0x3c, 0x52, 0x42, 0xa2, 0x53
            };
            var IV = new Span<byte>(data);
            var decryptBytes = crypter.Decrypt(data, IV.Slice(0, Const.AesMsgSize).ToArray());
            Console.Out.WriteLine(crypter.ByteToMsg(decryptBytes));
            Console.Out.WriteLine("Data:");
            printArray(data);
            Console.Out.WriteLine("Decrypt:");
            printArray(decryptBytes);
            Console.Out.WriteLine("");
        }

        static void testCfb() // done.
        {
            Console.Out.WriteLine("Test CFB");
            var crypter = new Crypto();
            crypter.SetMode("CFB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5fa19309183091297ad97ef79d59ce29f5f51eeca32eabedd9afa93299");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-", ""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
            Console.WriteLine();
            var decryptByte = crypter.Decrypt(crypter.Encrypt(data));
            printArray(data);
            printArray(decryptByte);
        }

        static void testOfb() // done.
        {
            Console.Out.WriteLine("Test OFB");
            var crypter = new Crypto();
            crypter.SetMode("OFB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-", ""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
            Console.WriteLine();
            var decrypt = crypter.Decrypt(crypter.Encrypt(data));
            printArray(data);
            printArray(decrypt);
        }

        static void testCtr()
        {
            Console.Out.WriteLine("Test CTR");
            var crypter = new Crypto();
            crypter.SetMode("CTR");
            byte[] key =
                {0x36, 0xf1, 0x83, 0x57, 0xbe, 0x4d, 0xbd, 0x77, 0xf0, 0x50, 0x51, 0x5c, 0x73, 0xfc, 0xf9, 0xf2};

            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            var data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5fa19309183091297ad97ef79d59ce29f5f51eeca32eabedd9afa93299");
            crypter.SetKey(key);
            
            Console.Out.WriteLine("Data:");
            printArray(data);
            Console.Out.WriteLine("Encrypt:");
            var encrypt = crypter.Encrypt(data);
            printArray(encrypt);
            crypter.ClearCtr();
            Console.Out.WriteLine("Decrypt:");
            var decrypt = crypter.Decrypt(encrypt);
            printArray(decrypt);
            printArray(crypter.GetIv());
        }

        public static void Main(string[] args)
        {
            var crypter = new Crypto();
            // testEcb();
            // testCbc();
            // testCfb();
            // testOfb();
            testCtr();
        }
    }
};