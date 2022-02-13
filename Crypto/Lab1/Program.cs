using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab
{
    public class Modes
    {
        public const string ECB = "ECB";
        public const string CBC = "CBC";
        public const string CFB = "CFB";
        public const string OFB = "CFB";
        public const string CT = "CT";
        public const string PKS7 = "PKS7";
        public const string NON = "NON";
    }

    public class Const
    {
        public const int SizeBytes = 128; // 128 bytes max
        public const int SizeMode = 5; // count of modes
    }

    internal class Programm
    {
        public static void Main(string[] args)
        {
        }
    }
};