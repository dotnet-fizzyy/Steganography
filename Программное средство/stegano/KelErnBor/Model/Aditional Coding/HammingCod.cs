using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Aditional_Coding
{
    public class HammingCod : ICod
    {
        public string Cod(string messageStrBin)
        {
            int[] messageBin = new int[messageStrBin.Length];

            return "";
        }

        public string DeCod(string messageStrBin)
        {

            ShowMetroMessageBox("HammingCod", "HammingCod");
            return "";
        }
        public override string ToString()
        {
            return "Кодирование при помощи модифицированного кода Хемминга";
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
    }
}
