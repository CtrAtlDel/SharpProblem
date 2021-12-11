using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace task4
{
    public class Matrix
    {
        private int size; // строка
        private int[,] data;

        public Matrix(int size)
        {
            this.size = size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    data[i, j] = 0;
                }
            }
        }

        public static void MatrixMultiplication(Matrix left, Matrix right, Matrix result) // O(n^3)
        {
            if (left.size != right.size) throw new ArgumentException("Incorrect size of matrix");
            // var result = new Matrix(left.line, right.col);
            for (var i = 0; i < left.size; i++)
            {
                for (var j = 0; j < right.size; j++)
                {
                    result.data[i, j] = 0;
                    for (var k = 0; k < left.size; k++)
                    {
                        result.data[i, j] += left.data[i, k] * right.data[k, j];
                    }
                }
            }
        }

        //умножение матрицы на матрицу 
        public static void MatrixMultiplicationUsingTread(Matrix left, Matrix right, Matrix result)
        {
            if (left.size != right.size) throw new ArgumentException("Incorrect size of matrix");
            Parallel.ForEach(Enumerable.Range(0, left.size),
                k =>
                {
                    Parallel.ForEach(Enumerable.Range(0, left.size),
                        i =>
                        {
                            for (int j = 0; j < left.size; j++)
                            {
                                result.data[i, j] += left.data[k, j] * right.data[j, i];
                            }
                        });
                });
        }

        // умножение матрицы на число
        public static void MatrixMultiplicationNumber(Matrix matrix, Matrix result, int number)
        {
            Parallel.ForEach(Enumerable.Range(0, matrix.size),
                i =>
                {
                    var x = i / matrix.size;
                    var y = i % matrix.size;
                    result.data[x, y] = number * matrix.data[x, y];
                }
            );
        }

        public static void Input(Matrix matrix)
        {
        }

        public static void PrintMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.size; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    Console.Write($"{matrix.data[i, j]}");
                }
                Console.WriteLine();
            }
        }
    }
}