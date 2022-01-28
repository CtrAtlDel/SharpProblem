using System.IO.Enumeration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CourseWork.Protobuf.Matrix;

namespace CourseWork.Worker
{
    public static class Algorithms
    {
        public static Matrix Multiplication(PairMatrix ms)
        {
            var left = ms.Left;
            var right = ms.Right;

            var dimX = left.DimX;
            var dimY = left.DimY;
            var dim = dimX;

            var result = new Matrix
            {
                DimX = dimX,
                DimY = dimY
            };

            for (int i = 0; i < dim; i++) //зануляем матрицу
            {
                result.Lines.Add(new Matrix.Types.Line());
                for (int j = 0; j < dim; j++)
                {
                    result.Lines[i].Columns.Add(0);
                }
            }
            
            Parallel.ForEach(Enumerable.Range(0, dim), k =>
            {
                Parallel.ForEach(Enumerable.Range(0, dim), i =>
                {
                    for (int j = 0; j < dim; j++)
                    {
                        var leftValue = left.Lines[k].Columns[j];
                        var rightValue = right.Lines[j].Columns[i];
                        var previous = result.Lines[k].Columns[i];
                        result.Lines[k].Columns[i] = previous + leftValue * rightValue;
                    }
                });
            });

            return result;
        }

        public static Matrix Sum(PairMatrix ms)
        {
            var left = ms.Left;
            var right = ms.Right;

            if (left.DimX != right.DimX || left.DimY != right.DimY)
            {
                // TODO
            }

            var dimX = left.DimX;
            var dimY = left.DimY;

            var result = new Matrix
            {
                DimX = dimX,
                DimY = dimY
            };

            for (var i = 0; i < dimX; ++i)
            {
                result.Lines.Add(new Matrix.Types.Line());
                for (var j = 0; j < dimY; ++j)
                {
                    var x = left.Lines[i].Columns[j];
                    var y = right.Lines[i].Columns[j];
                    result.Lines[i].Columns.Add(x + y);
                }
            }

            return result;
        }
    }
}