using System;

namespace CourseWork.Client
{
    public static class Generator
    {
        private static Random _random = new();
        
        public static double[,] Gen(int x, int y)
        {
            var result = new double[x, y];
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    result[i, j] = _random.NextDouble() * 50;
                }
            }
            return result;
        } 
    }
}