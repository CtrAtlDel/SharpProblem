using CourseWork.Protobuf.Matrix;

namespace CourseWork.Client
{
    public static class Mapper
    {
        public static Matrix Map(double[,] matrix)
        {
            var result = new Matrix
            {
                DimX = matrix.GetLength(0),
                DimY = matrix.GetLength(1)
            };
            for (var i = 0; i < result.DimX; ++i)
            {
                result.Lines.Add(new Matrix.Types.Line());
                for (var j = 0; j < result.DimY; ++j)
                {
                    result.Lines[i].Columns.Add(matrix[i,j]);
                }
            }
            return result;
        }
    }
}