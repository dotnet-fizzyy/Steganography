using GalaSoft.MvvmLight;
using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;
using Stegano.View;

namespace Stegano.ViewModel
{
    public class ShowWindowViewModel : ViewModelBase
    {
        public RelayCommand OpenMainWindowCommand { get; private set; }
        public ShowWindowViewModel()
        {
            OpenMainWindowCommand = new RelayCommand(OpenMainWindow);
        }
        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
