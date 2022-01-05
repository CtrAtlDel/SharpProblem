using System;
using System.Linq;

namespace task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string PATH = "/Users/ivankvasov/gitSharp/SharpProblem/task4/task4/Matrix.csv";
            Parser parser = new Parser();
            parser.ParseCsv(PATH);
        }
    }
}