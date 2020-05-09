using FirstFloor.ModernUI.Windows.Controls;

namespace Stegano.View
{
    /// <summary>
    /// Логика взаимодействия для ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : ModernWindow
    {
        public ShowWindow()
        {
            InitializeComponent();
        }

        private void Close(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
