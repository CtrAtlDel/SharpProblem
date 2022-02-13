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

    internal class Programm
    {
        public static void Main(string[] args)
        {
        }
    }
};