//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using FirstFloor.ModernUI.Windows.Controls;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Command;
//using Stegano.Algorithm;
//using Stegano.Model;
//using Microsoft.Win32;
//using Aspose.Words.Lists;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Aspose.Words;
//using System.Collections.ObjectModel;
//using System.Windows.Data;
//using Stegano.Interfaces;
//using Stegano.Model.Aditional_Coding;

//namespace Stegano.ViewModel
//{
//    public class HideUnderlineViewModel : ViewModelBase
//    {
//        #region Properties

//        private string fullPathToOrigFile;
//        public string FullPathToOrigFile
//        {
//            get { return "Путь к файлу: " + fullPathToOrigFile; }
//            set { fullPathToOrigFile = value; RaisePropertyChanged(); }
//        }

//        private string countLettersIsCanHide;
//        public string CountLettersIsCanHide
//        {
//            get { return $"Количество символов, которые можно скрыть: {countLettersIsCanHide}"; }
//            set
//            {
//                countLettersIsCanHide = value;
//                RaisePropertyChanged();
//            }
//        }

//        private string textForHide = "";
//        public string TextForHide
//        {
//            get { return textForHide; }
//            set
//            {
//                TextForHideChanged(value);
//                RaisePropertyChanged();
//            }
//        }


//        private string countLettersForHide;
//        public string CountLettersForHide
//        {
//            get { return $"Введено символов: {countLettersForHide}"; }
//            set
//            {
//                countLettersForHide = value;
//                RaisePropertyChanged();
//            }
//        }

//        private string keyStatus;
//        public string KeyStatus
//        {
//            private get { return keyStatus; }
//            set
//            {
//                keyStatus = value;
//                RaisePropertyChanged();
//            }
//        }

//        private SolidColorBrush keyStatusColor;
//        public SolidColorBrush KeyStatusColor
//        {
//            get { return keyStatusColor; }
//            set
//            {
//                keyStatusColor = value;
//                RaisePropertyChanged();
//            }
//        }

//        private bool isTextForHideEnabled;
//        public bool IsTextForHideEnabled
//        {
//            get { return isTextForHideEnabled; }
//            set
//            {
//                isTextForHideEnabled = value;
//                RaisePropertyChanged();
//            }
//        }

//        private string sourceString = string.Empty;

//        public CheckBoxModel RandomCheckBox { get; set; }

//        public CheckBoxModel RSACheckBox { get; set; }

//        public CheckBoxModel AdditionalBitsCheckBox { get; set; }

//        public CheckBoxModel VisibleColorCheckBox { get; set; }

//        public CheckBoxModel SmartHidingCheckBox { get; set; }

//        public CheckBoxModel AttributeHidingCheckBox { get; set; }

//        //
//        public CheckBoxModel ShifrElGamalCheckBox { get; set; }

//        public ObservableCollection<IHash> HashMethods { get; set; }
//        public IHash SelectedHashMethod { get; set; }

//        public ObservableCollection<ICod> CodMethods { get; set; }
//        public ICod SelectedCodMethod { get; set; }

//        public ObservableCollection<ICrypt> CryptMethods { get; set; }
//        public ICrypt SelectedCryptMethod { get; set; }


//        public CheckBoxModel AESCheckBox { get; set; }

//        public CheckBoxModel TwoFishCheckBox { get; set; }

//        public CheckBoxModel HashingSHA512CheckBox { get; set; }

//        public CheckBoxModel HashingMD5CheckBox { get; set; }


//        private bool isHideInformationButtonEnabled;
//        public bool IsHideInformationButtonEnabled
//        {
//            get { return isHideInformationButtonEnabled; }
//            set
//            {
//                isHideInformationButtonEnabled = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string OneFontName { get; set; }
//        public string ZeroFontName { get; set; }

//        public ObservableCollection<object> FontStats { get; set; }


//        // public Dictionary<string, int> FontStats = new Dictionary<string, int>() { { "Time454", 2}, { "Timet54", 2 }, };

//        #endregion

//        #region RelayCommands

//        public RelayCommand OpenDocumentRelayCommand { get; private set; }
//        public RelayCommand HideInformationRelayCommand { get; private set; }

//        #endregion

//        #region VARS

//        private OpenFileDialog openFileDialog;

//        private string pathToDirOrigFile;
//        private string filenameOrigFile;

//        private int maxLettersIsCanHide;
//        #endregion

//        #region Constructor and Initializers

//        public HideUnderlineViewModel()
//        {
//            UIInit();
//            FontStats = new ObservableCollection<object>();
//            openFileDialog = new OpenFileDialog();

//            RelayInit();

//            HashMethodsInit();
//            CodMethodsInit();
//            CryptMethodsInit();
//        }

//        private void RelayInit()
//        {
//            OneFontName = "Arial";
//            ZeroFontName = "Cartana";
//            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
//            HideInformationRelayCommand = new RelayCommand(HideInformation);
//        }

//        private void UIInit()
//        {
//            FullPathToOrigFile = "";
//            CountLettersIsCanHide = 0.ToString();
//            CountLettersForHide = 0.ToString();

//            TextForHide = "";

//            KeyStatusColor = new SolidColorBrush(Colors.Black);
//            KeyStatus = "";

//            IsTextForHideEnabled = false;
//            IsHideInformationButtonEnabled = false;


//            RandomCheckBox = new CheckBoxModel();
//            SmartHidingCheckBox = new CheckBoxModel();
//            AttributeHidingCheckBox = new CheckBoxModel();

//            ShifrElGamalCheckBox = new CheckBoxModel();

//            RSACheckBox = new CheckBoxModel();
//            VisibleColorCheckBox = new CheckBoxModel();
//            AdditionalBitsCheckBox = new CheckBoxModel();
//            HashingMD5CheckBox = new CheckBoxModel();
//            HashingSHA512CheckBox = new CheckBoxModel();
//            AESCheckBox = new CheckBoxModel();
//            TwoFishCheckBox = new CheckBoxModel();
//        }

//        private void CodMethodsInit()
//        {
//            CodMethods = new ObservableCollection<ICod>
//            {
//                new CyclicCod(),
//                new HammingCod(16, false),
//                new HammingCod(16, true),
//            };
//        }

//        private void CryptMethodsInit()
//        {
//            CryptMethods = new ObservableCollection<ICrypt>
//            {
//                new AES(),
//                new RSA(),
//                new TwoFish()
//            };
//        }

//        private void HashMethodsInit()
//        {
//            HashMethods = new ObservableCollection<IHash>
//            {
//                new SHA512(),
//                new MD5(),
//            };
//        }

//        #endregion

//        #region RelayMethods
//        private async void OpenDocument()
//        {
//            if (OpenFileDialog(openFileDialog) != null)
//            {
//                FullPathToOrigFile = openFileDialog.FileName;
//                filenameOrigFile = openFileDialog.SafeFileName;
//                pathToDirOrigFile = fullPathToOrigFile.Substring(0, fullPathToOrigFile.Length - filenameOrigFile.Length);

//                FontStats.Clear();
//                int count = TextStat.HowMuchLettersICanHide(fullPathToOrigFile);
//                var stats = await TextStat.GetFontStat(fullPathToOrigFile);
//                foreach (var st in stats)
//                {
//                    FontStats.Add(new FontInfo(st.Key, st.Value, count));
//                }

//                count /= 8;
//                CountLettersIsCanHide = count.ToString();
//                maxLettersIsCanHide = Int32.Parse(countLettersIsCanHide);

//                if (Int32.Parse(countLettersIsCanHide) > 0)
//                {
//                    IsTextForHideEnabled = true;
//                    IsHideInformationButtonEnabled = true;

//                    RandomCheckBox.IsEnabled = true;
//                    SmartHidingCheckBox.IsEnabled = true;
//                    AttributeHidingCheckBox.IsEnabled = true;

//                    ShifrElGamalCheckBox.IsEnabled = true;

//                    HashingMD5CheckBox.IsEnabled = true;
//                    RSACheckBox.IsEnabled = true;
//                    AdditionalBitsCheckBox.IsEnabled = true;
//                    VisibleColorCheckBox.IsEnabled = true;
//                    HashingSHA512CheckBox.IsEnabled = true;
//                    AESCheckBox.IsEnabled = true;
//                    TwoFishCheckBox.IsEnabled = true;

//                }
//                else
//                {
//                    ShowMetroMessageBox("Ошибка", "В данном файле невозможно скрыть информацию.");
//                }
//            }
//        }

//        private async void HideInformation()
//        {
//            if (textForHide.Length > 0)
//            {
//                sourceString = textForHide;

//                if (SmartHidingCheckBox.IsChecked)
//                {
//                    ShowMetroMessageBox("Предупреждение", "При выбранном умном скрытии автоматически будет включена псевдорандомизация, \n\tа визуальное выделение отключено!\n");
//                    RandomCheckBox.IsChecked = true;
//                    VisibleColorCheckBox.IsChecked = false;
//                }
//                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
//                bool isSuccesful = false;

//                textForHide = (RSACheckBox.IsChecked)
//                    ? Converter.RsaCryptor(TextForHide, pathToDirOrigFile)
//                    : Converter.StringToBinary(TextForHide);

//                textForHide = (AdditionalBitsCheckBox.IsChecked)
//                    ? HideUnderlineModel.AddAdditionalBits(textForHide)
//                    : textForHide;


//                textForHide = (ShifrElGamalCheckBox.IsChecked)
//                 ? HideUnderlineModel.HideUnderlineElGamal(textForHide)
//                 : Converter.StringToBinary(TextForHide);



//                textForHide = SelectedCryptMethod?.Encrypt(textForHide, pathToDirOrigFile) ?? textForHide;

//                textForHide = SelectedCodMethod?.Coding(SelectedCryptMethod != null ? textForHide : Converter.StringToBinary(TextForHide)) ?? TextForHide;

//                AttributeHidingModel codeModel = new AttributeHidingModel(pathToNewFile);
//                isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), VisibleColorCheckBox.IsChecked, SelectedCodMethod != null, SelectedCryptMethod != null);

//                var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null || SelectedCodMethod != null ? TextForHide : Converter.BinaryToString(TextForHide)) ?? TextForHide;
//                if (!string.IsNullOrWhiteSpace(hash))
//                {
//                    MD5.SaveHash(pathToDirOrigFile, hash); //Mocked until base class will not be implemented
//                }

//                if (isSuccesful)
//                {
//                    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);
//                    KeyStatus = (RSACheckBox.IsChecked == true) ? "сгенерирован" : "выключен RSA";
//                    KeyStatusColor = (RSACheckBox.IsChecked == true) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Black);
//                }
//                else
//                {
//                    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
//                    KeyStatus = (RSACheckBox.IsChecked == true) ? "ошибка генерации" : "выключен RSA";
//                    KeyStatusColor = (RSACheckBox.IsChecked == true) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
//                }
//            }
//            else
//            {
//                ShowMetroMessageBox("Ошибка", "Введите текст.");
//            }
//            //TextForHide = String.Empty;
//        }


//        #endregion


//        private void TextForHideChanged(string text)
//        {
//            textForHide = text;
//            CountLettersForHide = textForHide.Length.ToString();
//            int letterWeigth = 0;
//            if (AdditionalBitsCheckBox != null)
//                letterWeigth = AdditionalBitsCheckBox.IsChecked ? Int32.Parse(countLettersForHide) * 4 : Int32.Parse(countLettersForHide);
//            CountLettersIsCanHide = (maxLettersIsCanHide - letterWeigth).ToString();
//        }

//        private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
//        {
//            try
//            {
//                openFileDialog.Filter = "Файлы Microsoft Word|*.doc;*.docx; | Все файлы|*.*";
//                openFileDialog.FilterIndex = 1;
//                openFileDialog.RestoreDirectory = true;

//                if (openFileDialog.ShowDialog() == true)
//                {
//                    return openFileDialog;
//                }

//                return null;
//            }
//            catch (Exception exception)
//            {
//                ShowMetroMessageBox("Error", exception.Message);
//            }

//            return null;
//        }

//        private void ShowMetroMessageBox(string title, string message)
//        {
//            var dialog = new ModernDialog()
//            {
//                Title = title,
//                Content = message
//            };

//            dialog.ShowDialog();
//        }
//    }
//}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;
using Aspose.Words.Lists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Words;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Stegano.Interfaces;
using Stegano.Model.Aditional_Coding;

namespace Stegano.ViewModel
{
    public class HideUnderlineViewModel : ViewModelBase
    {
        #region Properties

        private string fullPathToOrigFile;
        public string FullPathToOrigFile
        {
            get { return "Путь к файлу: " + fullPathToOrigFile; }
            set { fullPathToOrigFile = value; RaisePropertyChanged(); }
        }

        private string countLettersIsCanHide;
        public string CountLettersIsCanHide
        {
            get { return $"Количество символов, которые можно скрыть: {countLettersIsCanHide}"; }
            set
            {
                countLettersIsCanHide = value;
                RaisePropertyChanged();
            }
        }

        private string textForHide = "";
        public string TextForHide
        {
            get { return textForHide; }
            set
            {
                TextForHideChanged(value);
                RaisePropertyChanged();
            }
        }


        private string countLettersForHide;
        public string CountLettersForHide
        {
            get { return $"Введено символов: {countLettersForHide}"; }
            set
            {
                countLettersForHide = value;
                RaisePropertyChanged();
            }
        }

        private string keyStatus;
        public string KeyStatus
        {
            private get { return keyStatus; }
            set
            {
                keyStatus = value;
                RaisePropertyChanged();
            }
        }

        private SolidColorBrush keyStatusColor;
        public SolidColorBrush KeyStatusColor
        {
            get { return keyStatusColor; }
            set
            {
                keyStatusColor = value;
                RaisePropertyChanged();
            }
        }

        private bool isTextForHideEnabled;
        public bool IsTextForHideEnabled
        {
            get { return isTextForHideEnabled; }
            set
            {
                isTextForHideEnabled = value;
                RaisePropertyChanged();
            }
        }

        private string sourceString = string.Empty;

        public CheckBoxModel RandomCheckBox { get; set; }

        public CheckBoxModel RSACheckBox { get; set; }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        public CheckBoxModel VisibleColorCheckBox { get; set; }

        public CheckBoxModel SmartHidingCheckBox { get; set; }

        public CheckBoxModel AttributeHidingCheckBox { get; set; }
           
        public CheckBoxModel AESCheckBox { get; set; }

        public CheckBoxModel TwoFishCheckBox { get; set; }

        public CheckBoxModel HashingSHA512CheckBox { get; set; }

        public CheckBoxModel HashingMD5CheckBox { get; set; }

        public CheckBoxModel ShifrElGamalCheckBox { get; set; }


        private bool isHideInformationButtonEnabled;
        public bool IsHideInformationButtonEnabled
        {
            get { return isHideInformationButtonEnabled; }
            set
            {
                isHideInformationButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string OneFontName { get; set; }
        public string ZeroFontName { get; set; }

        public ObservableCollection<object> FontStats { get; set; }


        // public Dictionary<string, int> FontStats = new Dictionary<string, int>() { { "Time454", 2}, { "Timet54", 2 }, };

        #endregion

        #region RelayCommands

        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand HideInformationRelayCommand { get; private set; }

        public ObservableCollection<ICod> CodMethods { get; set; }
        public ICod SelectedCodMethod { get; set; }

        public ObservableCollection<ICrypt> CryptMethods { get; set; }
        public ICrypt SelectedCryptMethod { get; set; }

        public ObservableCollection<IHash> HashMethods { get; set; }
        public IHash SelectedHashMethod { get; set; }

        #endregion

        #region VARS

        private OpenFileDialog openFileDialog;

        private string pathToDirOrigFile;
        private string filenameOrigFile;

        private int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public HideUnderlineViewModel()
        {
            UIInit();
            FontStats = new ObservableCollection<object>();
            openFileDialog = new OpenFileDialog();

            RelayInit();
            CodMethodsInit();
            CryptMethodsInit();
            HashMethodsInit();
        }

        private void RelayInit()
        {
            OneFontName = "Arial";
            ZeroFontName = "Cartana";
            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
            HideInformationRelayCommand = new RelayCommand(HideInformation);
        }

        private void UIInit()
        {
            FullPathToOrigFile = "";
            CountLettersIsCanHide = 0.ToString();
            CountLettersForHide = 0.ToString();

            TextForHide = "";

            KeyStatusColor = new SolidColorBrush(Colors.Black);
            KeyStatus = "";

            IsTextForHideEnabled = false;
            IsHideInformationButtonEnabled = false;

            RSACheckBox = new CheckBoxModel();
            VisibleColorCheckBox = new CheckBoxModel();
            AdditionalBitsCheckBox = new CheckBoxModel();
            RandomCheckBox = new CheckBoxModel();
            SmartHidingCheckBox = new CheckBoxModel();
            AttributeHidingCheckBox = new CheckBoxModel();
            HashingMD5CheckBox = new CheckBoxModel();
            HashingSHA512CheckBox = new CheckBoxModel();
            AESCheckBox = new CheckBoxModel();
            TwoFishCheckBox = new CheckBoxModel();

            ShifrElGamalCheckBox = new CheckBoxModel();

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
                new ShifrElGamal(),
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


        #endregion

        #region RelayMethods
        private async void OpenDocument()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                FullPathToOrigFile = openFileDialog.FileName;
                filenameOrigFile = openFileDialog.SafeFileName;
                pathToDirOrigFile = fullPathToOrigFile.Substring(0, fullPathToOrigFile.Length - filenameOrigFile.Length);

                FontStats.Clear();
                int count = TextStat.HowMuchLettersICanHide(fullPathToOrigFile);
                var stats = await TextStat.GetFontStat(fullPathToOrigFile);
                foreach (var st in stats)
                {
                    FontStats.Add(new FontInfo(st.Key, st.Value, count));
                }

                count /= 8;
                CountLettersIsCanHide = count.ToString();
                maxLettersIsCanHide = Int32.Parse(countLettersIsCanHide);

                if (Int32.Parse(countLettersIsCanHide) > 0)
                {
                    IsTextForHideEnabled = true;
                    IsHideInformationButtonEnabled = true;

                    RandomCheckBox.IsEnabled = true;
                    RSACheckBox.IsEnabled = true;
                    AdditionalBitsCheckBox.IsEnabled = true;
                    VisibleColorCheckBox.IsEnabled = true;
                    SmartHidingCheckBox.IsEnabled = true;
                    AttributeHidingCheckBox.IsEnabled = true;

                    HashingMD5CheckBox.IsEnabled = true;
                    HashingSHA512CheckBox.IsEnabled = true;
                    AESCheckBox.IsEnabled = true;
                    TwoFishCheckBox.IsEnabled = true;

                    ShifrElGamalCheckBox.IsEnabled = true;

                }
                else
                {
                    ShowMetroMessageBox("Ошибка", "В данном файле невозможно скрыть информацию.");
                }
            }
        }

        private async void HideInformation()
        {
            if (textForHide.Length > 0)
            {
                sourceString = textForHide;

                if (SmartHidingCheckBox.IsChecked)
                {
                    ShowMetroMessageBox("Предупреждение", "При выбранном умном скрытии автоматически будет включена псевдорандомизация, \n\tа визуальное выделение отключено!\n");
                    RandomCheckBox.IsChecked = true;
                    VisibleColorCheckBox.IsChecked = false;
                }

                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;

                
               
                //textForHide = (RSACheckBox.IsChecked)
                //    ? Converter.RsaCryptor(TextForHide, pathToDirOrigFile)
                //    : Converter.StringToBinary(TextForHide);

                //MessageBox.Show("ee"+textForHide);

                //textForHide = (AdditionalBitsCheckBox.IsChecked)
                //    ? HideUnderlineModel.AddAdditionalBits(textForHide)
                //    : textForHide;

                //textForHide = Converter.StringToBinary(textForHide);

              

                //MessageBox.Show("ee1" + textForHide);

                //textForHide = (ShifrElGamalCheckBox.IsChecked)
                // ? HideUnderlineModel.HideUnderlineElGamal(textForHide)
                // : Converter.StringToBinary(TextForHide);

               //MessageBox.Show("text"+textForHide);


                textForHide = SelectedCryptMethod?.Encrypt(textForHide, pathToDirOrigFile)
                    ?? textForHide;

                //MessageBox.Show("криптаt" + textForHide);

                textForHide = SelectedCodMethod?.Coding(SelectedCryptMethod != null 
                    ? textForHide : Converter.StringToBinary(TextForHide)) 
                    ?? TextForHide;
                MessageBox.Show(textForHide);

                //AttributeHidingModel codeModel = new AttributeHidingModel(pathToNewFile);
                //isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), VisibleColorCheckBox.IsChecked, SelectedCodMethod != null, SelectedCryptMethod != null);

                HideUnderlineModel codeModel = new HideUnderlineModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked, OneFontName, ZeroFontName);


                var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null || SelectedCodMethod != null ? TextForHide : Converter.BinaryToString(TextForHide)) ?? TextForHide;
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    MD5.SaveHash(pathToDirOrigFile, hash); //Mocked until base class will not be implemented
                }

                if (isSuccesful)
                {
                    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);
                    KeyStatus = (RSACheckBox.IsChecked == true) ? "сгенерирован" : "выключен RSA";
                    KeyStatusColor = (RSACheckBox.IsChecked == true) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
                    KeyStatus = (RSACheckBox.IsChecked == true) ? "ошибка генерации" : "выключен RSA";
                    KeyStatusColor = (RSACheckBox.IsChecked == true) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
                }

               
            }
            else
            {
                ShowMetroMessageBox("Ошибка", "Введите текст.");
            }
            //TextForHide = String.Empty;
        }


        #endregion


        private void TextForHideChanged(string text)
        {
            textForHide = text;
            CountLettersForHide = textForHide.Length.ToString();
            int letterWeigth = 0;
            if (AdditionalBitsCheckBox != null)
                letterWeigth = AdditionalBitsCheckBox.IsChecked ? Int32.Parse(countLettersForHide) * 4 : Int32.Parse(countLettersForHide);
            CountLettersIsCanHide = (maxLettersIsCanHide - letterWeigth).ToString();
        }

        private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
        {
            try
            {
                openFileDialog.Filter = "Файлы Microsoft Word|*.doc;*.docx; | Все файлы|*.*";
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
            var dialog = new ModernDialog()
            {
                Title = title,
                Content = message
            };

            dialog.ShowDialog();
        }
    }
}