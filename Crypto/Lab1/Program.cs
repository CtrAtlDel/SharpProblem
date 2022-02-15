using System;
using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLab
{
    internal class Programm
    {
        public static void Main(string[] args)
        {
            var provider = new AesCryptoServiceProvider();
            byte[] trt = provider.IV;
            
        }
    }
};