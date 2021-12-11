using System;
using System.Linq;

namespace task4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] array = new int[10, 10];
            var threads = Enumerable.Range(0, 10);
            foreach (var VARIABLE in threads)
            {
                Console.WriteLine($"{VARIABLE}");
            }
            Console.Write($"{array.Length}");
        }
    }
}