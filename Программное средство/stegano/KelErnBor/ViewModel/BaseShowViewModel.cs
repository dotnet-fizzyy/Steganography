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

        private string hashFile;
        public string HashFile
        {
            get { return hashFile; }
            set
            {
                hashFile = value;
                RaisePropertyChanged();
            }
        }

        private string cryptFile;
        public string CryptFile
        {
            get { return cryptFile; }
            set
            {
                cryptFile = value;
                RaisePropertyChanged();
            }
        }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }
        public ObservableCollection<object> FontStats { get; set; }

        public ObservableCollection<ICod> CodMethods { get; set; }
        public ICod SelectedCodMethod { get; set; }

        public ObservableCollection<ICrypt> CryptMethods { get; set; }
        public ICrypt SelectedCryptMethod { get; set; }

        public ObservableCollection<IHash> HashMethods { get; set; }
        public IHash SelectedHashMethod { get; set; }

        #endregion

        #region RelayCommands

        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand OpenPrivateKeyRelayCommand { get; set; }
        public RelayCommand OpenForDecodeRelayCommand { get; set; }
        public RelayCommand OpenHashDocument { get; set; }

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
            CryptMethodsInit();
            HashMethodsInit();
        }

        protected string messageTransformation(string message)
        {
            bool? isHashValid = SelectedHashMethod?.VerifyHash(message, hashFile);

            if (isHashValid != null && isHashValid == false)
            {
                return null;
            }

            message = SelectedCodMethod?.DeCoding(message) ?? message;
            message = SelectedCryptMethod?.Decrypt(message, CryptFile) ?? message;

            return message;
        }

        private void CodMethodsInit()
        {
            CodMethods = new ObservableCollection<ICod>();
            CodMethods.Add(new CyclicCod(11, "10011"));
            CodMethods.Add(new HammingCod(16, false));
            CodMethods.Add(new HammingCod(16, true));
        }

        private void CryptMethodsInit()
        {
            CryptMethods = new ObservableCollection<ICrypt>
            {
                new AES(),
                new RSA(),
                new TwoFish()
            };
        }

        private void HashMethodsInit()
        {
            HashMethods = new ObservableCollection<IHash>
            {
                new SHA512(),
                new MD5(),
            };
        }

        private void RelayInit()
        {
            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
            OpenHashDocument = new RelayCommand(OpenHashFile);
            OpenPrivateKeyRelayCommand = new RelayCommand(OpenPrivateKeyFile);
        }
        #endregion

        #region RelayMethods

        private async void OpenDocument()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                PathToDoc = openFileDialog.FileName;

                FontStats?.Clear();
                int count = TextStat.HowMuchLettersICanHide(PathToDoc);
                var stats = await TextStat.GetFontStat(PathToDoc);
                foreach (var st in stats)
                {
                    FontStats?.Add(new FontInfo(st.Key, st.Value, count));
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

        protected void OpenHashFile()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                HashFile = openFileDialog.FileName;
            }
        }

        protected void OpenPrivateKeyFile()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                CryptFile = openFileDialog.FileName;
            }
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
