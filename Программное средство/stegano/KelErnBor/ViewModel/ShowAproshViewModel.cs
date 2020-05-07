using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;

namespace Stegano.ViewModel
{
    public class ShowAproshViewModel : ViewModelBase
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

        private string rsaFile;
        public string RsaFile
        {
            get { return rsaFile; }
            set
            {
                rsaFile = value;
                RaisePropertyChanged();
            }
            }

        private CheckBoxModel rsaOpenCheckBox;
        public CheckBoxModel RsaOpenCheckBox
        {
            get { return rsaOpenCheckBox; }
            set
            {
                rsaOpenCheckBox = value;
                RaisePropertyChanged();
            }
            }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        public string ZeroBitSpacing { get; set; }
        public string SoloBitSpacing { get; set; }
        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            private set
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


        #endregion

            #region RelayCommands

        public RelayCommand OpenPrivateKeyRelayCommand { get; private set; }
        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand OpenForDecodeRelayCommand { get; set; }

        #endregion

        #region VARS

        private OpenFileDialog openFileDialog;

        private string pathToDirOrigFile;
        private string filenameOrigFile;

        private int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public ShowAproshViewModel()
        {
            DecodeUIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {
            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
            OpenPrivateKeyRelayCommand = new RelayCommand(OpenPrivateKeyFile);
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);
        }



        private void DecodeUIInit()
        {
            rsaOpenCheckBox = new CheckBoxModel(true, false);
            AdditionalBitsCheckBox = new CheckBoxModel(true, false);
        }
        #endregion

        #region RelayMethods

        private void OpenDocument()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                PathToDoc = openFileDialog.FileName;
            }
        }


        private void OpenPrivateKeyFile()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                RsaFile = openFileDialog.FileName;
            }
        }
        private async void OpenForDecode()
        {
            try
            {
                if (string.IsNullOrEmpty(PathToDoc))
                {
                    ShowMetroMessageBox("Information", "Загрузите файл для дешифровки");
                    return;
                }

                CryptedText = "";
                SearchedText = "";
                ShowAproshModel codeModel = new ShowAproshModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation(ZeroBitSpacing, SoloBitSpacing);

                foundedBitsInDoc = AdditionalBitsCheckBox.IsChecked
                    ? ShowModel.RemoveAdditBits(foundedBitsInDoc)
                    : foundedBitsInDoc;

                SearchedText = Converter.BinaryToString(foundedBitsInDoc);

                if (RsaOpenCheckBox.IsChecked)
                {
                    if (string.IsNullOrEmpty(RsaFile))
                    {
                        ShowMetroMessageBox("Information", "Нет файла с приватным ключом!");
                        return;
                    }

                    CryptedText = SearchedText;
                    SearchedText = await Converter.RsaDecryptor(SearchedText, RsaFile);
                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Information", "Ключ не подходит.");
                        return;
                    }
                }

                if (SearchedText.Length > 0)
                {
                    ShowMetroMessageBox("Информация", "Извлечение информации из файла " + openFileDialog.SafeFileName + " прошло успешно.");
                }
                else
                    ShowMetroMessageBox("Информация", "Файл " + openFileDialog.SafeFileName + " не содержит скрытой информации.");

            }
            catch (Exception e)
            {
                ShowMetroMessageBox("Информация", e.Message + "\n " + e.InnerException + "\n" + "\n" + e.Source);
            }
        }
        #endregion




        private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
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
                ShowMetroMessageBox("Ошибка", exception.Message);
            }

            return null;
        }

        private void ShowMetroMessageBox(string title, string message)
        {
            var mm = new ModernDialog
            {
                Title = title,
                Content = message
            };

            mm.ShowDialog();
        }
    }
}
