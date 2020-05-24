using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Aditional_Coding
{
    public class Matrix
    {
        public int[][] Matrix_P;
        public int[][] Matrix_I;
        public int[][] Matrix_H;
        int k;
        int r;
        int R;
        bool isMdf;
        public int N => isMdf ? k + R : k + r; 

        public int K { get => k; private set => k = value; }

        public Matrix(int k, bool isMdf)
        {
            this.K = k;
            this.r = (int)Math.Ceiling(Math.Log(k, 2)) + 1;
            this.R = r + 1;
            this.isMdf = isMdf;
            Matrix_P = generateMatrix_P();
            Matrix_I = generateMatrix_I();
            Matrix_H = isMdf
                        ? generateMatrix_Hd4()
                        : generateMatrix_H();

        }

        private int[][] generateMatrix_P()
        {
            int[][] matrixP = new int[r][];
            for (int i = 0; i < r; i++)
                matrixP[i] = new int[K];

            string[] arrayColumnValue = new string[K];
            string colValue;
            int maxColumnWeiht = (int)Math.Pow(2.0, (double)r) - 1;

            Random random = new Random();
            for (int i = 0, j = 3; i < K; i++, j++)
            {
                colValue = Convert.ToString(j, 2);
                colValue = CorrectBinValue(colValue, r);
                if (arrayColumnValue.Contains(colValue) || !CheakWeight(colValue))
                    i--;
                else
                    arrayColumnValue[i] = colValue;
            }

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    matrixP[i][j] = int.Parse(arrayColumnValue[j][i] + "");
                }
            }

            return matrixP;
        }
        private string CorrectBinValue(string str, int r)
        {
            while (str.Length < r)
                str = str.Insert(0, "0");
            return str;
        }

        public static string ReadByCols(string word,int cols, int rows)
        {
            var result = new StringBuilder();
            for(int i = 0; i < cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    result.Append(word[cols * j +i]);
                }
            }
            return result.ToString();
        }

        public static bool CheakWeight(string str)
        {
            return str.Count(i => i == '1') >= 2;
        }

        private int[][] generateMatrix_I()
        {
            int[][] matrix_I = new int[r][];

            for (int i = 0; i < r; i++)
                matrix_I[i] = new int[r];

            for (int i = 0; i < r; i++)
                matrix_I[i][i] = 1;

            return matrix_I;
        }
        int[][] generateMatrix_H()
        {
            int[][] matrix_H = new int[r][];
            for (int i = 0; i < r; i++)
                matrix_H[i] = new int[r + K];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    matrix_H[i][j] = Matrix_P[i][j];
                }
            }
            for (int i = 0, j = K; i < r; i++, j++)
            {
                matrix_H[i][j] = 1;
            }

            return matrix_H;
        }

        int[][] generateMatrix_Hd4()
        {
            int[][] matrix_H = new int[R][];
            for (int i = 0; i < R; i++)
                matrix_H[i] = new int[R + K];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    matrix_H[i][j] = Matrix_P[i][j];
                }
            }

            int val;
            for (int i = 0; i < K; i++)
            {
                val = 1;
                for (int j = 0; j < r; j++)
                    val += Matrix_P[j][i];
                matrix_H[r][i] = val % 2;
            }

            for (int i = 0, j = K; i < R; i++, j++)
            {
                matrix_H[i][j] = 1;
            }

            return matrix_H;
        }

        public static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                    Console.Write(matrix[i][j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintMatrix(int[][] matrix, int sP, int Ep)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (j >= sP && j <= Ep)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(matrix[i][j] + " ");

                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintMatrix(int[] matrix, int sP, int Ep)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i >= sP && i <= Ep)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(matrix[i] + " ");

                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\n");
        }

        public static void PrintMatrix(int[] matrix, int sP1, int Ep1, int sP2, int Ep2)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i >= sP1 && i <= Ep1 || i >= sP2 && i <= Ep2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(matrix[i] + " ");

                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\n");
        }

        public static void PrintMatrix(int[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write(matrix[i] + " ");
            }
            Console.WriteLine("\n");

        }
        public static int[] MultiplyMatrixAndVector(int[][] mA, int[] vB)
        {
            int[] newVector = new int[mA.Length];

            for (int i = 0; i < mA.Length; i++)
            {
                for (int j = 0; j < vB.Length; j++)
                    newVector[i] += mA[i][j] * vB[j];
                newVector[i] = newVector[i] % 2;
            }

            return newVector;
        }

        public static int[] SummBinArray(int[] A, int[] B)
        {
            int[] resultArray = new int[A.Length];
            for (int i = 0; i < A.Length; i++)
                resultArray[i] = A[i] == B[i] ? 0 : 1;
            return resultArray;
        }
    }

}
