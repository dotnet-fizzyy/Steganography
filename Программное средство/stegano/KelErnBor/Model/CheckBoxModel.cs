using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Stegano.Annotations;

namespace Stegano.Model
{
    public class CheckBoxModel : ObservableObject
    {
        public CheckBoxModel()
        {
            IsEnabled = false;
            IsChecked = false;
        }

        public CheckBoxModel(bool isEnabled, bool isChecked)
        {
            IsEnabled = isEnabled;
            IsChecked = isChecked;
        }
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }
    }  
}