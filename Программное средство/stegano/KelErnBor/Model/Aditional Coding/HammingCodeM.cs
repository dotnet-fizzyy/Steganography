using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Aditional_Coding
{
    public class HammingCodeM : ICod
    {
        public string Cod(string input)
        {
            ShowMetroMessageBox("HammingCodeM", "HammingCodeM");
            return "";
        }

        public string DeCod(string input)
        {
            ShowMetroMessageBox("HammingCodeM", "HammingCodeM");
            return "";
        }
        public override string ToString()
        {
            return "Кодирование при помощи кода Хемминга";
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
