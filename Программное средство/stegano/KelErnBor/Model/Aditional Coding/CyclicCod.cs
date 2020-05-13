using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Aditional_Coding
{
    public class CyclicCod : ICod
    {
        public string Cod(string input)
        {
            ShowMetroMessageBox("CyclicCod", "CyclicCod");
            return "";
        }

        public string DeCod(string input)
        {
            ShowMetroMessageBox("CyclicCod", "CyclicCod");
            return "";
        }

        public override string ToString()
        {
            return "Кодирование при помощи Циклического кода";
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
