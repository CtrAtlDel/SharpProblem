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
            int[] array = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

            var spanSource = new Span<int>(array);
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    int length = array.Length - i * 5;
                    var newSpan = spanSource.Slice(i * 5, length);
                    Console.WriteLine(newSpan[i]);
                }
            }
        }
    }
};