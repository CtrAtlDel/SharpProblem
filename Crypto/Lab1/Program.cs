﻿using System;
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
            var key = crypter.GenerateKey();
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
            // var key = crypter.GenerateKey();
            // Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            // byte[] data =
            // crypter.MsgToByte(
            // "4ca00ff4c898d61e1edbf1800618fb2828a226d160dad07883d04e008a7897ee2e4b7465d5290d0c0e6c6822236e1daafb94ffe0c5da05d9476be028ad7c1d81");
            byte[] key =
            {
                0x14, 0x0b, 0x41, 0xb2, 0x2a, 0x29, 0xbe, 0xb4, 0x06, 0x1b, 0xda, 0x66, 0xb6, 0x74, 0x7e, 0x14
            };
            // Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            crypter.SetKey(key);
            byte[] data = {
                0x5b, 0x68, 0x62, 0x9f, 0xeb, 0x86, 0x06, 0xf9, 0xa6, 0x66, 0x76, 0x70, 0xb7, 0x5b, 0x38, 0xa5, 
                0xb4, 0x83, 0x2d, 0x0f, 0x26, 0xe1, 0xab, 0x7d, 0xa3, 0x32, 0x49, 0xde, 0x7d, 0x4a, 0xfc, 0x48, 
                0xe7, 0x13, 0xac, 0x64, 0x6a, 0xce, 0x36, 0xe8, 0x72, 0xad, 0x5f, 0xb8, 0xa5, 0x12, 0x42, 0x8a, 
                0x6e, 0x21, 0x36, 0x4b, 0x0c, 0x37, 0x4d, 0xf4, 0x55, 0x03, 0x47, 0x3c, 0x52, 0x42, 0xa2, 0x53

            };

            // string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            // Console.WriteLine(answerString.Replace("-", ""));
            // Console.WriteLine("Iv is " + crypter.ByteToMsg(crypter.GetIv()).Replace("-", ""));
            // Console.WriteLine();
            // var encryptBytes = crypter.Encrypt(data);
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
            var key = crypter.GenerateKey();
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
            testCbc();
            // testCfb();
            // testOfb();
        }
    }
};