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
            byte[] rer = Array.Empty<byte>();
            byte[] com = {1,2,3,4,5,};
            var result = rer.Concat(com);
            foreach (var it in result)
            {
                Console.WriteLine(it);
            }
        }
    }
};