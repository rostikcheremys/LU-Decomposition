using System;

namespace Program
{
    abstract class Program
    {
        public static void Main()
        {
            double[,] matrix = {
                {3, 2, 1},
                {3, 1, 4},
                {5, 8, 1}
            };

            double[] vectorB = { 10, 12, 18 };
            
            int number = matrix.GetLength(0);
            
            double[] xi = new double[number];
            double[,] lower = new double[number, number];
            double[,] upper = new double[number, number];

            Console.WriteLine("Initial data:\n3*x1 + 2*x2 + x3 = 10 \n3*x1 + x2 + 4*x3 = 12\n5*x1 + 8*x2 + x3 = 18\n");
            LU_Decomposition(matrix, lower, upper);
            
            Console.WriteLine("Lower triangular matrix:");
            PrintMatrix(lower);

            Console.WriteLine("\nUpper triangular matrix:");
            PrintMatrix(upper);
            
            Console.WriteLine("\nSolution X:");
            SolveSystem(lower, upper, vectorB, xi);

            Console.WriteLine("\nSolution:");
            CheckSolution(number, matrix, vectorB, xi);
        }

        private static void LU_Decomposition(double[,] matrix, double[,] lower, double[,] upper)
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
        
        private static void SolveSystem(double[,] lower, double[,] upper, double[] vectorB, double[] xi)
        {
            int number = lower.GetLength(0);

            // Stage 1: Solve LY = B => Y
            double[] y = new double[number];

            for (int i = 0; i < number; i++)
            {
                double sum = 0;

                for (int j = 0; j < i; j++)
                {
                    sum += lower[i, j] * y[j];
                }

                y[i] = (vectorB[i] - sum) / lower[i, i];
            }

            // Stage 2: Solve UX = Y => X
            double[] x = new double[number];

            for (int i = number - 1; i >= 0; i--)
            {
                double sum = 0;
                
                for (int j = i + 1; j < number; j++)
                {
                    sum += upper[i, j] * x[j];
                }
                
                x[i] = (y[i] - sum) / upper[i, i];
            }
            
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine($"X[{i + 1}] = {x[i]:F2}");
                xi[i] += x[i];
            }
        }
        
        private static void CheckSolution(int number, double[,] matrix, double[] vectorB, double[] xi)
        {
            bool isCheckSolutions  = true;

            for (int i = 0; i < number; i++)
            {
                double result = 0;

                for (int j = 0; j < number; j++)
                {
                    result += matrix[i, j] * xi[j];
                }

                Console.WriteLine($"Equation {i + 1} = {result} ");
                
                if (Math.Abs(vectorB[i] - result) > 1e-10) isCheckSolutions = false;
            }

            Console.WriteLine(isCheckSolutions
                ? "\nAll solutions match the right-hand side vector B. Everything is correct!"
                : "\nSolutions do not match the right-hand side vector B.");
        }
    }
}
