using Aspose.Words;
using FirstFloor.ModernUI.Windows.Controls;
using Stegano.Algorithm;
using Stegano.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Algorithm.Aditional_Coding
{
    public class HammingCod : ICod
    {
        Matrix matrix;
        public bool IsMdf { get; set; }
        public HammingCod(int k,bool isMdf)
        {
            IsMdf = isMdf;
            matrix = new Matrix(k,isMdf);
        }

        public string Coding(string message)
        {
            var result = new StringBuilder();
            int i = 0;
            int wordsCount = (int)Math.Ceiling((decimal)(message.Length / matrix.K));
            int newLength = wordsCount * matrix.K;

            message = message.PadRight(newLength, '0');

            while (i < wordsCount)
            {
                result.Append(Cod(message.Substring(matrix.K * i++, matrix.K)));
            }

            return Matrix.ReadByCols(result.ToString(), matrix.N, wordsCount);
        }

        public string DeCoding(string message)
        {
            var result = new StringBuilder();
            int i = 0;
            int wordsCount = (int)Math.Ceiling((decimal)(message.Length / matrix.N));
            int newLength = wordsCount * matrix.N;
            message = message.PadRight(newLength, '0');

            message = Matrix.ReadByCols(message, wordsCount, matrix.N);

            while (i < wordsCount)
            {
                result.Append(DeCod(message.Substring(matrix.N * i++, matrix.N)));
            }

            return result.ToString();
        }

        public string Cod(string messageStrBin)
        {
            int[] messageBin = new int[messageStrBin.Length];
            for(int i = 0; i < messageBin.Length; i++)
            {
                messageBin[i] = messageStrBin[i];
            }

            int[] Xr = Matrix.MultiplyMatrixAndVector(matrix.Matrix_H, messageBin);

            for (int i = 0; i < Xr.Length; i++)
            {
                messageStrBin += Xr[i]+"";
            }

            return messageStrBin;
        }

        public string DeCod(string messageStrBin)
        {
            int c;
            var res = new StringBuilder();
            int[] messageBin = new int[messageStrBin.Length];
            for (int i = 0; i < messageBin.Length; i++)
            {
                messageBin[i] = messageStrBin[i];
            }

            int[] sindr = Matrix.MultiplyMatrixAndVector(matrix.Matrix_H, messageBin);

            if (sindr.Count(i => i == 1) > 0)
            {
                ShowMetroMessageBox($"HammingCod {(IsMdf ? "M" : "")}", "Есть ошибка в кодовой последовательности");

                if (IsMdf && sindr.Count(i => i == 1) % 2 == 0)
                    ShowMetroMessageBox($"HammingCod {(IsMdf ? "M" : "")}", "Есть 2 ошибки (четное количество) в кодовой последовательности");
                else
                    ShowMetroMessageBox($"HammingCod {(IsMdf ? "M" : "")}", "Есть 1 ошибка (нечетное количество) в кодовой последовательности");

                c = findErrorCollumn(matrix.Matrix_H, sindr);
                if (c  == -1)
                {
                    ShowMetroMessageBox($"HammingCod {(IsMdf ? "M" : "")}", "Не удалось исправить ошибку в кодовой последовательности");
                }
                else
                {
                    int[] E = new int[messageBin.Length];
                    E[c] = 1;
                    messageBin = Matrix.SummBinArray(messageBin, E);
                }
            }

            for (int i = 0; i < matrix.K; i++)
            {
                res.Append((char)messageBin[i]);
            }
            return res.ToString();
        }

        int findErrorCollumn(int[][] mat, int[] s)
        {
            int row;
            int column;

            for (column = 0; column < mat[0].Length; column++)
            {
                for (row = 0; row < mat.Length; row++)
                    if (mat[row][column] != s[row])
                        break;
                if (row == mat.Length)
                    break;
            }
            if (column == mat[0].Length)
            {
                column = -1;
            }
            return column;
        }

        public override string ToString() => IsMdf
                                             ? "Кодирование при помощи модифицированного кода Хемминга"
                                             : "Кодирование при помощи кода Хемминга";

        private void ShowMetroMessageBox(string title, string message)
        {
            var dialog = new ModernDialog()
            {
                Title = title,
                Content = message
            };

            dialog.ShowDialog();
        }
    }
}
