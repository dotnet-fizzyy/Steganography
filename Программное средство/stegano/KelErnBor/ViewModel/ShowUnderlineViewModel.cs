using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using Stegano.Model.Aditional_Coding;
using Stegano.Interfaces;

namespace Stegano.ViewModel
{
    public class ShowUnderlineViewModel : ViewModelBase
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

        public CheckBoxModel AesOpenCheckBox { get; set; }

        public CheckBoxModel TwoFishCheckBox { get; set; }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        public CheckBoxModel SmartHidingCheckBox { get; set; }

        public CheckBoxModel HashingSHA512 { get; set; }

        public CheckBoxModel HashingMD5 { get; set; }


        public CheckBoxModel ShifrElGamalCheckBox { get; set; }

        private string searchedText1;
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

        public string SearchedText1
        {
            get { return searchedText1; }
            private set
            {
                searchedText1 = value;
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

        public string OneFontName { get; set; }
        public string ZeroFontName { get; set; }
        public ObservableCollection<object> FontStats { get; set; }

        public ObservableCollection<ICod> CodMethods { get; set; }
        public ICod SelectedCodMethod { get; set; }

        public ObservableCollection<ICrypt> CryptMethods { get; set; }
        public ICrypt SelectedCryptMethod { get; set; }

        public ObservableCollection<IHash> HashMethods { get; set; }
        public IHash SelectedHashMethod { get; set; }


        #endregion

        #region RelayCommands

        public RelayCommand OpenPrivateKeyRelayCommand { get; private set; }
        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand OpenForDecodeRelayCommand { get; set; }
        public RelayCommand OpenHashDocument { get; set; }

        #endregion

        #region VARS

        private OpenFileDialog openFileDialog;

        private string pathToDirOrigFile;
        private string filenameOrigFile;

        private int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public ShowUnderlineViewModel()
        {
            DecodeUIInit();
            FontStats = new ObservableCollection<object>();

            openFileDialog = new OpenFileDialog();

            RelayInit();
            CodMethodsInit();
            CryptMethodsInit();
            HashMethodsInit();
        }

        private void CodMethodsInit()
        {
            CodMethods = new ObservableCollection<ICod>
            {
                new CyclicCod(),
                new HammingCod(16, false),
                new HammingCod(16, true),
            };
        }

        private void CryptMethodsInit()
        {
            CryptMethods = new ObservableCollection<ICrypt>
            {
                new AES(),
                new RSA(),
                new TwoFish(),
                new ShifrElGamal()
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
            OpenPrivateKeyRelayCommand = new RelayCommand(OpenPrivateKeyFile);
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);
            OpenHashDocument = new RelayCommand(OpenHashFile);
        }



        private void DecodeUIInit()
        {
            rsaOpenCheckBox = new CheckBoxModel(true, false);
            AdditionalBitsCheckBox = new CheckBoxModel(true, false);
            SmartHidingCheckBox = new CheckBoxModel(true, false);

           // ShifrElGamalCheckBox = new CheckBoxModel(true, false);

            
            HashingSHA512 = new CheckBoxModel(true, false);
            HashingMD5 = new CheckBoxModel(true, false);
            AesOpenCheckBox = new CheckBoxModel(true, false);
            TwoFishCheckBox = new CheckBoxModel(true, false);

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

        private void OpenHashFile()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                HashFile = openFileDialog.FileName;
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
                    ShowMetroMessageBox("Информация", "Загрузите файл для извлечения");
                    return;
                }

                CryptedText = "";
               
                SearchedText1 = "";
                SearchedText = "";

                ShowUnderlineModel codeModel = new ShowUnderlineModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation();
                


                //foundedBitsInDoc = AdditionalBitsCheckBox.IsChecked
                //     ? ShowUnderlineModel.RemoveAdditBits(foundedBitsInDoc)
                //     : foundedBitsInDoc;


                //foundedBitsInDoc = ShifrElGamalCheckBox.IsChecked
                //   ? ShowUnderlineModel.ShowUnderlineElGamal(foundedBitsInDoc)
                //   : foundedBitsInDoc;





                SearchedText = Converter.BinaryToString(foundedBitsInDoc);


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

                if (SelectedCodMethod != null)
                {
                    SearchedText = Converter.BinaryToString(SelectedCodMethod.DeCoding(SearchedText));
                }
               
                if (SelectedCryptMethod != null)
                {
                   
                    
                    if (string.IsNullOrEmpty(RsaFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                        return;
                    }

                    SearchedText = SelectedCryptMethod?.Decrypt(SearchedText, RsaFile) ?? SearchedText;

                    

                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Информация", "Ключ не подходит.");
                        return;
                    }
                }

                
                if (RsaOpenCheckBox.IsChecked)
                {
                    if (string.IsNullOrEmpty(RsaFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                        return;
                    }

                    CryptedText = SearchedText;
                    SearchedText = await Converter.RsaDecryptor(SearchedText, RsaFile);
                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Информация", "Ключ не подходит.");
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
                ShowMetroMessageBox("Error", exception.Message);
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