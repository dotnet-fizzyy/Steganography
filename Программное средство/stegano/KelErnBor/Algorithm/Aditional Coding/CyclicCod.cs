using FirstFloor.ModernUI.Windows.Controls;
using Stegano.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Algorithm.Aditional_Coding
{
    public class CyclicCod : ICod
    {
        Matrix matrix;
        public CyclicCod(int k, string generatPolinom)
        {
            matrix = new Matrix(k, generatPolinom);
        }

        public string Coding(string message)
        {
            var result = new StringBuilder();
            int i = 0;
            int wordsCount = (int)Math.Ceiling((decimal)(message.Length / matrix.K))+1;
            int newLength = wordsCount * matrix.K;

            message = message.PadRight(newLength, '0');

            while (i < wordsCount)
            {
                ShowMetroMessageBox(Cod(message.Substring(matrix.K * i, matrix.K)),i.ToString());
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
            int Count = result.Length - (result.Length % 16);

            return result.ToString().Substring(0, Count);
        }

        public string Cod(string messageStrBin)
        {
            matrix.InfWord = messageStrBin;

            return matrix.CodWord;
        }

        public string DeCod(string messageStrBin)
        {
            return matrix.CorrectValue(messageStrBin, matrix.FindeSindrom(messageStrBin)).Substring(0,matrix.K);
        }


        public override string ToString() => "Кодирование при помощи циклического кода";
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
