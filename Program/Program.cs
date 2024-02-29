using System;

namespace Program
{
    abstract class Program
    {
        static void Main()
        {
            double[,] matrix = {
                {4, 3, -1},
                {-2, -4, 5},
                {1, 2, 6}
            };

            int number = matrix.GetLength(0);

            double[,] lower = new double[number, number];
            double[,] upper = new double[number, number];

            LUDecomposition(matrix, lower, upper);

            Console.WriteLine("Lower triangular matrix:");
            PrintMatrix(lower);

            Console.WriteLine("\nUpper triangular matrix:");
            PrintMatrix(upper);
        }

        private static void LUDecomposition(double[,] matrix, double[,] lower, double[,] upper)
        {
            int number = matrix.GetLength(0);

            for (int i = 0; i < number; i++)
            {
                // Upper triangular matrix
                for (int k = i; k < number; k++)
                {
                    double sum = 0;
                
                    for (int j = 0; j < i; j++)
                    {
                        sum += lower[i, j] * upper[j, k];
                    }
                
                    upper[i, k] = matrix[i, k] - sum;
                }

                // Lower triangular matrix
                for (int k = i; k < number; k++)
                {
                    if (i == k)
                    {
                        lower[i, i] = 1; 
                    }
                    else
                    {
                        double sum = 0;
                    
                        for (int j = 0; j < i; j++)
                        {
                            sum += lower[k, j] * upper[j, i];
                        }
                    
                        lower[k, i] = (matrix[k, i] - sum) / upper[i, i];
                    }
                }
            }
        }

        private static void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j]:F2}\t");
                }
            
                Console.WriteLine();
            }
        }
    }
}