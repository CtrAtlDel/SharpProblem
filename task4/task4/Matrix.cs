using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading;

namespace task4
{
    public class Matrix
    {
        private int line; // строка
        private int col; // столбец
        private int[][] data;

        public Matrix(int line, int col)
        {
            this.line = line;
            this.col = col;
            for (var i = 0; i < line; i++)
            {
                data[i] = new int[col];
            }

            for (int i = 0; i < line; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    data[i][j] = 0;
                }
            }
        }

        public static void MatrixMultiplication(Matrix left, Matrix right, Matrix result) // O(n^3)
        {
            if (left.col != right.line) throw new ArgumentException("Incorrect size of matrix");
            // var result = new Matrix(left.line, right.col);
            for (var i = 0; i < left.col; i++)
            {
                for (var j = 0; j < right.line; j++)
                {
                    result.data[i][j] = 0;
                    for (var k = 0; k < left.line; k++)
                    {
                        result.data[i][j] += left.data[i][k] * right.data[k][j];
                    }
                }
            }
            // return result;
        }

        public static void MatrixMultiplicationUsingTread(Matrix left, Matrix right, Matrix result)
        {
            if (left.col != right.line) throw new ArgumentException("Incorrect size of matrix");
            var wg = new CountdownEvent(left.line);
            var threads = Enumerable.Repeat(0, left.line).Select(
                i => new Thread(
                    () =>
                    {
                        for (int k = 0; k < left.line ; k++)
                        {
                            var x = i / left.col;
                            var y = i % left.col;
                        }
                    }
                )
            );
            // foreach (var thread in threads)
            // {
            //     thread.Start();
            // }
            // wg.Wait();
        }

        // умножение матрицы на число
        public static void MatrixMultiplicationNumber(Matrix matrix, Matrix result, int number)
        {
            var wg = new CountdownEvent(matrix.line * matrix.col);
            List<int> list = new List<int>();
            list.Add(1);
            var threads = Enumerable.Range(0, matrix.col * matrix.line).Select(
                i =>
                    new Thread(
                        () =>
                        {
                            
                        }
                    )
            );
            foreach (var thread in threads)
            {
                thread.Start();
            }

            wg.Wait();
        }

        public static void PrintMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.line; i++)
            {
                for (int j = 0; j < matrix.col; j++)
                {
                    Console.Write($"{matrix.data[i][j]}");
                }
                Console.WriteLine();
            }
        }
    }
}