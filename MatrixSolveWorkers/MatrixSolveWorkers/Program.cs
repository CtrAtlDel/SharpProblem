using System.ComponentModel;
using MatrixSolve;



string path1 = "/Users/ivankvasov/gitSharp/SharpProblem/MatrixSolveWorkers/MatrixSolveWorkers/Matrix.csv";
string path2 = "/Users/ivankvasov/gitSharp/SharpProblem/MatrixSolveWorkers/MatrixSolveWorkers/Matrix2.csv";

Parser parser = new Parser();

// parser.ReadCsv(path1, path2);
List<int> test1 = new List<int>();
List<int> test2 = new List<int>();
for(int i = 1; i < 10; ++i) {
    test1.Add(i);
    test2.Add(i);
}
List<int> test3 = Workers.Multiplication(test1, test2);

foreach (var VARIABLE in test3)
{
    Console.WriteLine($"{VARIABLE}");
}
Console.WriteLine($"Sum: is {Workers.Sum(test3)}");