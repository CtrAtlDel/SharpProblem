using System;
using System.Buffers;
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

        static void testEcb()
        {
            Console.Out.WriteLine("Test ECB");
            var crypter = new Crypto();
            crypter.SetMode("ECB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.Out.WriteLine("Key: ");
            printArray(key);
            // Console.WriteLine("Key is " + crypter.ByteToMsg(key).Replace("-", ""));
            byte[] data =
                crypter.MsgToByte(
                    "0ec770233009113871qrrtq4508731450987130485178ce7"); //"f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329");
            Console.Out.WriteLine("Data in bytes:");
            printArray(data);
            crypter.SetKey(key);
            var answer = crypter.Encrypt(data);
            Console.Out.WriteLine("Encrypt data: ");
            printArray(answer);
            var answerString = crypter.ByteToMsg(crypter.Encrypt(data));

            var cryptString = crypter.Encrypt(data);

            var decryptByte = crypter.Decrypt(cryptString);
            string decryptString = crypter.ByteToMsg(decryptByte);
            Console.Out.WriteLine("Decrypt data: ");
            printArray(decryptByte);
            Console.WriteLine();
        }

        static void testCbc()
        {
            Console.Out.WriteLine("Test CBC");
            var crypter = new Crypto();
            crypter.SetMode("CBC");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.Out.WriteLine("Key:");
            printArray(key);
            byte[] data =
                crypter.MsgToByte(
                    "0ec770233009113871rtq450873145098q7130485178ce897"); //IV in the begining
            var spanData = new Span<byte>(data);
            // spanData = spanData.Slice(Const.AesIvSize, spanData.Length);
            var newStr = crypter.ByteToMsg(spanData.ToArray());
            var newStrShort = newStr.Remove(0, Const.AesIvSize);
            Console.Out.WriteLine("Input Str: " + newStr);
            
            Console.Out.WriteLine("Data: ");
            printArray(data);
            crypter.SetKey(key);
            Console.WriteLine();
            var newData = crypter.Encrypt(data);
            Console.Out.WriteLine("Encrypt: ");
            printArray(newData);
            var decryptByte = crypter.Decrypt(newData);
            var IV = crypter.GetIv();
            Console.Out.WriteLine("Decrypt: ");
            printArray(decryptByte);
            Console.Out.WriteLine("My output: " + crypter.ByteToMsg(decryptByte));
            byte[] libEncrypt = EncryptStringToBytes_Aes(newStrShort, key, IV);
            string libDecrypt = DecryptStringFromBytes_Aes(libEncrypt, key, IV);
            Console.Out.WriteLine("Lib encrypt: ");
            printArray(libEncrypt);
            Console.Out.WriteLine("Lib decrypt: ");
            Console.Out.WriteLine(libDecrypt);
            Console.Out.WriteLine("Library output: " + libDecrypt);

            // Console.Out.WriteLine("Test CBC");
            // var crypter = new Crypto();
            // crypter.SetMode("CBC");
            // byte[] key =
            // {
            //     0x14, 0x0b, 0x41, 0xb2, 0x2a, 0x29, 0xbe, 0xb4, 0x06, 0x1b, 0xda, 0x66, 0xb6, 0x74, 0x7e, 0x14
            // };
            // crypter.SetKey(key);
            // byte[] data =
            // {
            //     0x5b, 0x68, 0x62, 0x9f, 0xeb, 0x86, 0x06, 0xf9, 0xa6, 0x66, 0x76, 0x70, 0xb7, 0x5b, 0x38, 0xa5,
            //     0xb4, 0x83, 0x2d, 0x0f, 0x26, 0xe1, 0xab, 0x7d, 0xa3, 0x32, 0x49, 0xde, 0x7d, 0x4a, 0xfc, 0x48,
            //     0xe7, 0x13, 0xac, 0x64, 0x6a, 0xce, 0x36, 0xe8, 0x72, 0xad, 0x5f, 0xb8, 0xa5, 0x12, 0x42, 0x8a,
            //     0x6e, 0x21, 0x36, 0x4b, 0x0c, 0x37, 0x4d, 0xf4, 0x55, 0x03, 0x47, 0x3c, 0x52, 0x42, 0xa2, 0x53
            // };
            // byte[] data2 =
            // {
            //     0x4c, 0xa0, 0x0f, 0xf4, 0xc8, 0x98, 0xd6, 0x1e, 0x1e, 0xdb, 0xf1, 0x80, 0x06, 0x18, 0xfb, 0x28, 
            //     0x28, 0xa2, 0x26, 0xd1, 0x60, 0xda, 0xd0, 0x78, 0x83, 0xd0, 0x4e, 0x00, 0x8a, 0x78, 0x97, 0xee, 
            //     0x2e, 0x4b, 0x74, 0x65, 0xd5, 0x29, 0x0d, 0x0c, 0x0e, 0x6c, 0x68, 0x22, 0x23, 0x6e, 0x1d, 0xaa, 
            //     0xfb, 0x94, 0xff, 0xe0, 0xc5, 0xda, 0x05, 0xd9, 0x47, 0x6b, 0xe0, 0x28, 0xad, 0x7c, 0x1d, 0x81
            //
            // };
            //
            // var IV = new Span<byte>(data);
            // var decryptBytes = crypter.Decrypt(data, IV.Slice(0, Const.AesMsgSize).ToArray());
            // var decryptBytes2 = crypter.Decrypt(data2, IV.Slice(0, Const.AesMsgSize).ToArray());
            // Console.Out.WriteLine("Decrypt:");
            // Console.Out.WriteLine(crypter.ByteToMsg(decryptBytes));
            // Console.Out.WriteLine("Decrypt:");
            // Console.Out.WriteLine(crypter.ByteToMsg(decryptBytes2));
            // Console.Out.WriteLine("");
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        static void testCfb()
        {
            Console.Out.WriteLine("Test CFB");
            var crypter = new Crypto();
            crypter.SetMode("CFB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.Out.WriteLine("Key:");
            printArray(key);
            byte[] data =
                crypter.MsgToByte(
                    "0ec770233009113871qrrtq4508731450987130485178ce71");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine();
            var newData = crypter.Encrypt(data);
            var decryptByte = crypter.Decrypt(newData);
            Console.Out.WriteLine("Data: ");
            printArray(data);
            Console.Out.WriteLine("Encrypt: ");
            printArray(newData);
            Console.Out.WriteLine("Decrypt: ");
            printArray(decryptByte);
        }

        static void testOfb()
        {
            Console.Out.WriteLine("Test OFB");
            var crypter = new Crypto();
            crypter.SetMode("OFB");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.Out.WriteLine("Key:");
            printArray(key);
            byte[] data =
                crypter.MsgToByte(
                    "0ec770233009113871qrrtq4508731450987130485178ce7");
            crypter.SetKey(key);
            string answerString = crypter.ByteToMsg(crypter.Encrypt(data));
            Console.WriteLine();
            var newData = crypter.Encrypt(data);
            var decryptByte = crypter.Decrypt(newData);
            Console.Out.WriteLine("Data: ");
            printArray(data);
            Console.Out.WriteLine("Encrypt: ");
            printArray(newData);
            Console.Out.WriteLine("Decrypt: ");
            printArray(decryptByte);
        }

        static void testCtr()
        {
            Console.Out.WriteLine("Test CTR");
            var crypter = new Crypto();
            crypter.SetMode("CTR");
            var key = crypter.GenerateRandom(Const.AesMsgSize);
            Console.Out.WriteLine("Key:");
            printArray(key);
            byte[] data =
                crypter.MsgToByte(
                    "0ec77023300tq4508731450987130485178ce7");
            crypter.SetKey(key);

            var newData = crypter.Encrypt(data);
            var decryptByte = crypter.Decrypt(newData);
            Console.Out.WriteLine("Data: ");
            printArray(data);
            Console.Out.WriteLine("Encrypt: ");
            printArray(newData);
            Console.Out.WriteLine("Decrypt: ");
            printArray(decryptByte);

            // Console.Out.WriteLine("Test CTR");
            // var crypter = new Crypto();
            // crypter.SetMode("CTR");
            // byte[] key =
            //     {0x36, 0xf1, 0x83, 0x57, 0xbe, 0x4d, 0xbd, 0x77, 0xf0, 0x50, 0x51, 0x5c, 0x73, 0xfc, 0xf9, 0xf2};
            // crypter.SetKey(key);
            // byte[] data =
            // {
            //     0x69, 0xdd, 0xa8, 0x45, 0x5c, 0x7d, 0xd4, 0x25, 0x4b, 0xf3, 0x53, 0xb7, 0x73, 0x30, 0x4e, 0xec, 
            //     0x0e, 0xc7, 0x70, 0x23, 0x30, 0x09, 0x8c, 0xe7, 0xf7, 0x52, 0x0d, 0x1c, 0xbb, 0xb2, 0x0f, 0xc3, 
            //     0x88, 0xd1, 0xb0, 0xad, 0xb5, 0x05, 0x4d, 0xbd, 0x73, 0x70, 0x84, 0x9d, 0xbf, 0x0b, 0x88, 0xd3, 
            //     0x93, 0xf2, 0x52, 0xe7, 0x64, 0xf1, 0xf5, 0xf7, 0xad, 0x97, 0xef, 0x79, 0xd5, 0x9c, 0xe2, 0x9f, 
            //     0x5f, 0x51, 0xee, 0xca, 0x32, 0xea, 0xbe, 0xdd, 0x9a, 0xfa, 0x93, 0x29
            // };
            // Console.Out.WriteLine("Data:");
            // printArray(data);
            // byte[] data2 =
            // {
            //     0x77, 0x0b, 0x80, 0x25, 0x9e, 0xc3, 0x3b, 0xeb, 0x25, 0x61, 0x35, 0x8a, 0x9f, 0x2d, 0xc6, 0x17, 
            //     0xe4, 0x62, 0x18, 0xc0, 0xa5, 0x3c, 0xbe, 0xca, 0x69, 0x5a, 0xe4, 0x5f, 0xaa, 0x89, 0x52, 0xaa, 
            //     0x0e, 0x31, 0x1b, 0xde, 0x9d, 0x4e, 0x01, 0x72, 0x6d, 0x31, 0x84, 0xc3, 0x44, 0x51
            //
            // };
            // // Console.Out.WriteLine("Data:");
            // var spanData = new Span<byte>(data);
            // var spanData2 = new Span<byte>(data2);
            // var decryptData = crypter.Decrypt(data, spanData.Slice(0, Const.AesMsgSize).ToArray());
            // var decryptData2 = crypter.Decrypt(data2, spanData.Slice(0, Const.AesMsgSize).ToArray());
            // Console.Out.WriteLine("Encrypt:");
            // // Console.Out.WriteLine(crypter.ByteToMsg(decryptData));
            // printArray(data);
            // Console.Out.WriteLine("Decrypt:");
            // printArray(decryptData);
            // // Console.Out.WriteLine(crypter.ByteToMsg(decryptData2));
        }

        public static void Main(string[] args)
        {
            var crypter = new Crypto();
            testEcb();
            testCbc();
            testCfb();
            testOfb();
            testCtr();
        }
    }
};