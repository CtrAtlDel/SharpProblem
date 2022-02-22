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
            Console.Out.WriteLine("Array: ");
            for (int i = 0; i < data.Length; i++)
            {
                Console.Out.Write($"{data[i]} ");
            }

            Console.Out.WriteLine("");
        }

        static void testEcb() // done.
        {
            var crypter = new Crypto();
            crypter.SetMode("ECB");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
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
            printArray(data);
            printArray(decryptByte);
        }

        static void testCbc() //done.
        {
            var crypter = new Crypto();
            crypter.SetMode("CBC");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-", ""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
            Console.WriteLine();
            var encryptBytes = crypter.Encrypt(data);
            var decryptBytes = crypter.Decrypt(encryptBytes);
            printArray(data);
            printArray(decryptBytes);
            Console.Out.WriteLine("");
        }

        static void testCfb() // done.
        {
            var crypter = new Crypto();
            crypter.SetMode("CFB");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
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
            var crypter = new Crypto();
            crypter.SetMode("OFB");
            var key = crypter.GenerateKey();
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

        public static void Main(string[] args)
        {
            var crypter = new Crypto();
            // testEcb();
            // testCbc();
            // testCfb();
            testOfb();
        }
    }
};