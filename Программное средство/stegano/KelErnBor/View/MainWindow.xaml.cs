using FirstFloor.ModernUI.Windows.Controls;


namespace Stegano.View
{
    public partial class MainWindow: ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowWindow showWindow = new ShowWindow(){Margin = this.Margin};
            showWindow.Show();
            this.Close();
        }
    }
}
