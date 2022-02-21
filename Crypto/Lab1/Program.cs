using System;
using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab
{
    
    internal class Programm
    {
        static void testEcb() // done.
        {
            var crypter = new Crypto();
            crypter.SetMode("ECB");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data = crypter.MsgToByte("0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            var answer = crypter.Encrypt(data);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.Write(answerString.Replace("-",""));
        }

        static void testCbc() //done.
        {
            var crypter = new Crypto();
            crypter.SetMode("CBC");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data = crypter.MsgToByte("0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-",""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
        }

        static void testCfb() // not done .
        {
            var crypter = new Crypto();
            crypter.SetMode("CFB");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data = crypter.MsgToByte("0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-",""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
        }

        static void testOfb() // done
        {
            var crypter = new Crypto();
            crypter.SetMode("OFB");
            var key = crypter.GenerateKey();
            Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data = crypter.MsgToByte("0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine(answerString.Replace("-",""));
            Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
        }

        public static void Main(string[] args)
        {
            var crypter = new Crypto();
            testOfb();

        }
    }
};