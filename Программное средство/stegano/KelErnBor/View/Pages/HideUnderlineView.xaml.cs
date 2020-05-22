using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stegano.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для HideKatyaView.xaml
    /// </summary>
    public partial class HideUnderlineView : UserControl
    {
        public HideUnderlineView()
        {
            InitializeComponent();

           
        }

        private void HidenTextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
