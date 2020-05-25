using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Stegano.Model.Font;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using Stegano.Algorithm.Aditional_Coding;
using Stegano.Interfaces;

namespace Stegano.ViewModel
{
    public class BaseShowViewModel : ViewModelBase
    {
        #region Properties

        private string pathToDoc;
        public string PathToDoc
        {
            get { return pathToDoc; }
            set
            {
                pathToDoc = value;
                RaisePropertyChanged();
            }
        }

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            protected set
            {
                searchedText = value;
                RaisePropertyChanged();
            }
        }

        private string cryptdeText;
        public string CryptedText
        {
            get { return cryptdeText; }
            set
            {
                cryptdeText = value;
                RaisePropertyChanged();
            }
        }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }
        public ObservableCollection<object> FontStats { get; set; }

        public ObservableCollection<ICod> CodMethods { get; set; }
        public ICod SelectedCodMethod { get; set; }



        #endregion

        #region RelayCommands

        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand OpenForDecodeRelayCommand { get; set; }

        #endregion

        #region VARS

        protected OpenFileDialog openFileDialog;

        protected string pathToDirOrigFile;
        protected string filenameOrigFile;

        protected int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers
        public BaseShowViewModel()
        {
            openFileDialog = new OpenFileDialog();

            RelayInit();
            CodMethodsInit();
        }

        protected string messageTransformation(string message)
        {
            message = SelectedCodMethod?.DeCoding(message) ?? message;
            //
            //сюда нужно добавить шифрование и хэширование
            //
            return message;
        }

        private void CodMethodsInit()
        {
            CodMethods = new ObservableCollection<ICod>();
            CodMethods.Add(new CyclicCod());
            CodMethods.Add(new HammingCod(16, false));
            CodMethods.Add(new HammingCod(16, true));
        }

        private void RelayInit()
        {
            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
        }
        #endregion

        #region RelayMethods

        private async void OpenDocument()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                PathToDoc = openFileDialog.FileName;

                FontStats.Clear();
                int count = TextStat.HowMuchLettersICanHide(PathToDoc);
                var stats = await TextStat.GetFontStat(PathToDoc);
                foreach (var st in stats)
                {
                    FontStats.Add(new FontInfo(st.Key, st.Value, count));
                }
            }
        }

        protected OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
        {
            try
            {
                openFileDialog.Filter = "Все файлы|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog;
                }

                return null;
            }
            catch (Exception exception)
            {
                ShowMetroMessageBox("Error", exception.Message);
            }

            return null;
        }
        protected void ShowMetroMessageBox(string title, string message)
        {
            var mm = new ModernDialog
            {
                Title = title,
                Content = message
            };

            mm.ShowDialog();
        }

        #endregion
    }
}
