using MatrixSolve;
using Microsoft.VisualBasic.CompilerServices;

string PATHfirst = "/Users/ivankvasov/gitSharp/SharpProblem/MatrixSolve/MatrixSolve/Matrix.csv";
string PATHsecond = "/Users/ivankvasov/gitSharp/SharpProblem/MatrixSolve/MatrixSolve/Matrix2.csv";
Parser parser = new Parser();
parser.size = 3;
parser.ReadCsv(PATHfirst, PATHsecond);