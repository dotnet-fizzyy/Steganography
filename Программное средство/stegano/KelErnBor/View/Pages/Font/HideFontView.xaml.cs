using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Stegano.View.Pages.Font
{
    /// <summary>
    /// Логика взаимодействия для HideFontView.xaml
    /// </summary>
    public partial class HideFontView : UserControl
    {
        public HideFontView()
        {
            InitializeComponent();

            var fontsCollection = new InstalledFontCollection();
            var ff = fontsCollection.Families;

            foreach (var item in ff)
            {
                oneFontName.Items.Add(item.Name);
                zeroFontName.Items.Add(item.Name);
            }
        }
        private void HidenTextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
