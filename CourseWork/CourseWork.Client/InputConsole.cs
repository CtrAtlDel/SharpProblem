using System;
using CourseWork.Client;

namespace ConsoleApplication1
{
    class Matrix
    {
        private int dim;
        private int[,] arrays;

        public Matrix()
        {
        }

        public int Dim
        {
            get { return dim; }
            set
            {
                if (value > 0) dim = value;
            }
        }

        public Matrix(int dim)
        {
            this.dim = dim;
            arrays = new int[this.dim, this.dim];
        }

        public int this[int i, int j]
        {
            get { return arrays[i, j]; }
            set { arrays[i, j] = value; }
        }

        public void WriteMat()
        {
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Console.WriteLine("Input elem of matrix{0}:{1}", i, j);
                    arrays[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
        }


        public void ReadMatrix()
        {
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Console.Write(arrays[i, j] + "\t");
                }

                Console.WriteLine();
            }
        }

        public static Matrix AlphaMultiplication(Matrix a, int ch)
        {
            Matrix resMass = new Matrix(a.Dim);
            for (int i = 0; i < a.Dim; i++)
            {
                for (int j = 0; j < a.Dim; j++)
                {
                    resMass[i, j] = a[i, j] * ch;
                }
            }

            return resMass;
        }

        ~Matrix()
        {
        }
    }
}

namespace CourseWork.Client
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    public class InputConsole
    {
        public static Pair<double[,], double[,]> InputMatrix()
        {
            Console.WriteLine("Input size of square matrix: ");
            int dim = 0;
            do
            {
                dim = Int32.Parse(Console.ReadLine());
            } while (dim <= 0);

            var matrix1 = new double[dim, dim];
            var matrix2 = new double[dim, dim];
            Console.WriteLine("Input first matrix: ");
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Console.WriteLine("Input element [{0}][{1}] ", i, j);
                    Console.Write("-->");
                    matrix1[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            Console.WriteLine("Input second matrix: ");
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Console.WriteLine("Input element [{0}][{1}] ", i, j);
                    Console.Write("-->");
                    matrix2[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            var pair = new Pair<double[,], double[,]>(matrix1, matrix2);

            return pair;
        }
    }
}