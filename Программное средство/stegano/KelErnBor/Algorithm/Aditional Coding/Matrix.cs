using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Algorithm.Aditional_Coding
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
        string infWord;
        string generatPolinom;
        string codWord;
        string[] canonicalMatrix;
        private string[] checkMatrix;

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

        public Matrix(int k, string generatPolinom)
        {
            this.isMdf = false;
            this.K = k;
            this.r = generatPolinom.Length - 1;
            this.generatPolinom = generatPolinom;
        }

        public string BitStrToPolinom(string bit)
        {
            string polinom = "";
            int position;
            for (int i = 0; i < bit.Length; i++)
            {
                position = bit.Length - 1 - i;
                if (bit[i] == '1')
                {
                    polinom += "x" + position;
                    if (i != bit.Length - 1)
                        polinom += " + ";
                }

            }
            return polinom;
        }
        private string LeftShift(string bit, int count)
        {
            return bit + new string('0', count);
        }

        private string RightShift(string bit, int count)
        {
            return new string('0', count) + bit.Substring(0, bit.Length - count);
        }

        private string PolinomDivision(string dividend, string divider)
        {
            if (dividend.Length - dividend.IndexOf('1') < divider.Length || !dividend.Contains('1'))
            {
                while (dividend.Length >= divider.Length)
                    dividend = dividend.Substring(1, dividend.Length - 1);
                return dividend;
            }
            else
                return PolinomDivision(SumBinStrings(dividend, CorrectBinValue(divider, dividend.IndexOf('1'), dividend.Length - dividend.IndexOf('1') - divider.Length)), divider);
        }
        private string SumBinStrings(string firstStr, string SecondStr)
        {
            string result = "";
            for (int i = 0; i < firstStr.Length; i++)
            {
                if (firstStr[i] == SecondStr[i])
                    result += "0";
                else
                    result += "1";
            }
            return result;
        }
        public string FindeSindrom(string yn)
        {
            return PolinomDivision(yn, generatPolinom);
        }
        private void ShowMetroMessageBox(string title, string message)
        {
            var dialog = new ModernDialog()
            {
                Title = title,
                Content = message
            };

            dialog.ShowDialog();
        }
        public string CorrectValue(string strVal, string sindrom)
        {
            //ShowMetroMessageBox(strVal, sindrom);
            if (sindrom.Count(c => c == '1') == 0)
            {
                return strVal;
            }
            else
            {
                if (checkMatrix is null)
                {
                    GeneratMatrix();
                    CheckMatrix();

                }

                string E = new string('0',K);
                for (int i = 0; i < K; i++)
                    if (checkMatrix[i] == sindrom)
                    {
                        char[] ar = E.ToCharArray();
                        ar[i] = '1';

                        E = new string(ar);

                        return SumBinStrings(strVal, E);
                    }

                return strVal;
            }
        }

        public void GeneratMatrix()
        {
            int k = K;
            int n = N;
            string lineValue = generatPolinom + new string('0', k - 1);
            string[] matrix = new string[k];

            for (int i = 0; i < k; i++)
                matrix[i] = RightShift(lineValue, i);

            Console.WriteLine("\nGENERATE MATRIX:\n");
            foreach (string i in matrix)
            {
                Console.WriteLine(i);
            }

            //canonical form
            for (int i = 0; i < k; i++)
            {
                string str = matrix[i].Remove(i, 1);
                str = str.Insert(i, "0");

                while (str.IndexOf('1') < k || !str.Contains('1'))
                {
                    str = SumBinStrings(str, matrix.First(st => st.IndexOf('1') == str.IndexOf('1')));
                }

                str = str.Remove(i, 1);
                matrix[i] = str.Insert(i, "1");

            }

            canonicalMatrix = matrix;
            Console.WriteLine("\nGENERATE MATRIX CANONICAL:\n");
            foreach (string i in matrix)
            {
                Console.WriteLine(i);
            }
        }

        public void CheckMatrix()
        {
            int k = K;
            int n = N;
            int r = n - k;
            string[] matrix = new string[n];

            for (int i = 0; i < k; i++)
            {
                for (int j = k; j < n; j++)
                    matrix[i] += "" + canonicalMatrix[i][j];
            }

            for (int i = 0; i < r; i++)
            {
                matrix[k + i] = new string('0', i) + '1' + new string('0', r - i - 1);
            }

            Console.WriteLine("\nCHECK MATRIX:\n");
            foreach (var str in matrix)
                Console.WriteLine(str);
            checkMatrix = matrix;
        }
        public string GeneratPolinom { get => generatPolinom; set => generatPolinom = value; }
        public string CodWord => infWord + PolinomDivision(LeftShift(infWord, generatPolinom.Length - 1), generatPolinom);
        public string InfWord { get => infWord; set => infWord = value; }

        private string CorrectBinValue(string str, int l, int r)
        {
            int i = 0;
            while (i++ < l)
                str = str.Insert(0, "0");
            i = 0;
            while (i++ < r)
                str += "0";
            return str;
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
