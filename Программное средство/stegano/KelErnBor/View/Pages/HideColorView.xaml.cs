using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для HideColorView.xaml
    /// </summary>
    public partial class HideColorView : UserControl
    {
        public HideColorView()
        {
            InitializeComponent();
        }

        private void HidenTextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
