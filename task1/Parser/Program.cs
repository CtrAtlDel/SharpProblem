using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //D:\forTask1\file.txt
            // Console.WriteLine("Input path");
            var path = "D:\\forTask1\\file.txt"; //Console.ReadLine();
            // Console.WriteLine($"Str: {path}");
            int res = sumWords(path);
            int sumLines = sumLine(path, 0);
            int sumwords = sumWordInString(path);
            Console.WriteLine($"Sum of words in file: {sumwords}");
            Console.WriteLine($"Sum words in first string:  {sumwords} ");
            Console.WriteLine($"Sum of line:  {sumLines}");
        }

        public static int sumWords(string filename)
        {
            var counter = 0;
            foreach (string line in System.IO.File.ReadLines(@filename)) // walk line
            {
                string str = line;
                str = str.Trim();
                string[] textArray = str.Split(new char[] {' '});
                counter += textArray.Length;
                // Console.WriteLine($"Words: {textArray.Length}");
            }

            return counter;
        }

        public static int sumLine(string filename, int numberOfLine)
        {
            int counter = 0;
            int index = 0;
            foreach (string line in System.IO.File.ReadLines(@filename)) // walk line
            {
                if (index == numberOfLine)
                {
                    string str = line;
                    str = str.Trim();
                    string[] textArray = str.Split(new char[] {' '});
                    counter += textArray.Length;
                    break;
                }

                index++;
            }
            return counter;
        }

        public static int sumWordInString(string filename)
        {
            var counter = 0;
            foreach (string line in System.IO.File.ReadLines(@filename)) // walk line
            {
                counter++;
            }
            return counter;
        }
        // public static string[] wichOne(string filename)
        // {
        //     // string pattern = @"(\d+)|(\-+)|(;+)";
        //     // var result = Regex.Matches(str, pattern);
        //     // string[] output = (from Match match in result select match.value).ToArray();
        //     // return;
        // }
    }
}