using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Stegano.Model.ColorSteg;
using Microsoft.Win32;
using System.Linq;

namespace Stegano.ViewModel.ColorSteg
{
    public class ShowColorViewModel : BaseShowViewModel
    {
        #region Properties


        //private string pathToDoc;
        //public string PathToDoc
        //{
        //    get { return pathToDoc; }
        //    set
        //    {
        //        pathToDoc = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string rsaFile;
        //public string RsaFile
        //{
        //    get { return rsaFile; }
        //    set
        //    {
        //        rsaFile = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private CheckBoxModel rsaOpenCheckBox;
        //public CheckBoxModel RsaOpenCheckBox
        //{
        //    get { return rsaOpenCheckBox; }
        //    set
        //    {
        //        rsaOpenCheckBox = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        public CheckBoxModel SmartHidingCheckBox { get; set; }

        //public CheckBoxModel AttributeHiding { get; set; }

        //private string searchedText;
        //public string SearchedText
        //{
        //    get { return searchedText; }
        //    private set
        //    {
        //        searchedText = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string cryptdeText;
        //public string CryptedText
        //{
        //    get { return cryptdeText; }
        //    set
        //    {
        //        cryptdeText = value;
        //        RaisePropertyChanged();
        //    }
        //}


        #endregion

        #region RelayCommands

        //public RelayCommand OpenPrivateKeyRelayCommand { get; private set; }
        //public RelayCommand OpenDocumentRelayCommand { get; private set; }
        //public RelayCommand OpenForDecodeRelayCommand { get; set; }

        #endregion

        #region VARS

        //private OpenFileDialog openFileDialog;

        //private string pathToDirOrigFile;
        //private string filenameOrigFile;

        //private int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public ShowColorViewModel()
        {
            DecodeUIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {
            //OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
            //OpenPrivateKeyRelayCommand = new RelayCommand(OpenPrivateKeyFile);
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);
        }



        private void DecodeUIInit()
        {
            //rsaOpenCheckBox = new CheckBoxModel(true, false);
            //AdditionalBitsCheckBox = new CheckBoxModel(true,false);
            SmartHidingCheckBox = new CheckBoxModel(true,false);
            //AttributeHiding = new CheckBoxModel(true, false);
        }
        #endregion

        #region RelayMethods

        //private void OpenDocument()
        //{
        //    if (OpenFileDialog(openFileDialog) != null)
        //    {
        //        PathToDoc = openFileDialog.FileName;
        //    }
        //}


        //private void OpenPrivateKeyFile()
        //{
        //    if (OpenFileDialog(openFileDialog) != null)
        //    {
        //        RsaFile = openFileDialog.FileName;
        //    }
        //}
        private async void OpenForDecode()
        {
            try
            {
                if (string.IsNullOrEmpty(PathToDoc))
                {
                    ShowMetroMessageBox("Информация", "Загрузите файл для извлечения");
                    return;
                }

                CryptedText = "";
                SearchedText = "";

                Stopwatch.Start();
                ShowColorModel codeModel = new ShowColorModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation(SmartHidingCheckBox.IsChecked);

                //foundedBitsInDoc = AdditionalBitsCheckBox.IsChecked
                //    ? ShowColorModel.RemoveAdditBits(foundedBitsInDoc)
                //    : foundedBitsInDoc;

                SearchedText = SelectedCodMethod == null ? SearchedText = Converter.BinaryToString(foundedBitsInDoc) : foundedBitsInDoc;

                if (SelectedCodMethod != null)
                {
                    EncodedText = SearchedText;

                    SearchedText = Converter.BinaryToString(SelectedCodMethod.DeCoding(SearchedText));
                }

                if (SelectedHashMethod != null)
                {
                    if (string.IsNullOrEmpty(HashFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с хэшем!");
                        return;
                    }

                    var isHashSame = SelectedHashMethod.VerifyHash(SearchedText, HashFile);

                    if (!isHashSame)
                    {
                        ShowMetroMessageBox("Информация", "Не валидный хэш!");
                        return;
                    }
                }

                if (SelectedCryptMethod != null)
                {
                    CryptedText = SearchedText;

                    if (string.IsNullOrEmpty(CryptFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                        return;
                    }

                    SearchedText = SelectedCryptMethod?.Decrypt(SearchedText, CryptFile) ?? SearchedText;

                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Информация", "Ключ не подходит.");
                        return;
                    }
                }

                //if (RsaOpenCheckBox.IsChecked)
                //{
                //    if (string.IsNullOrEmpty(RsaFile))
                //    {
                //        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                //        return;
                //    }

                //    CryptedText = SearchedText;
                //    SearchedText = await Converter.RsaDecryptor(SearchedText, RsaFile);
                //    if (string.IsNullOrEmpty(SearchedText))
                //    {
                //        ShowMetroMessageBox("Информация", "Ключ не подходит.");
                //        return;
                //    }
                //}

                Stopwatch.Stop();
                TimeForDerypting = Math.Round(Stopwatch.Elapsed.TotalSeconds, 2).ToString() + " сек.";

                if (SearchedText.Length > 0)
                {
                    ShowMetroMessageBox("Информация", "Извлечение информации из файла " + PathToDoc.Split('\\').LastOrDefault() + " прошло успешно.");
                }
                else
                    ShowMetroMessageBox("Информация", "Файл " + PathToDoc.Split('\\').LastOrDefault() + " не содержит скрытой информации.");
                
            }
            catch (Exception e)
            {
                ShowMetroMessageBox("Информация", e.Message + "\n " + e.InnerException + "\n" + "\n" + e.Source);
            }
        }
        #endregion




        //private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
        //{
        //    try
        //    {
        //        openFileDialog.Filter = "Все файлы|*.*";
        //        openFileDialog.FilterIndex = 1;
        //        openFileDialog.RestoreDirectory = true;

        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            return openFileDialog;
        //        }

        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        ShowMetroMessageBox("Error", exception.Message);
        //    }

        //    return null;
        //}

        //private void ShowMetroMessageBox(string title, string message)
        //{
        //    var mm = new ModernDialog
        //    {
        //        Title = title,
        //        Content = message
        //    };

        //    mm.ShowDialog();
        //}
    }
}
