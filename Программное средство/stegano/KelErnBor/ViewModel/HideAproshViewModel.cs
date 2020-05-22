using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;

namespace Stegano.ViewModel
{
    public class HideAproshViewModel:ViewModelBase
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

        public string ZeroBitSpacing { get; set; }
        public string SoloBitSpacing { get; set; }

        public CheckBoxModel RandomCheckBox { get; set; }

        public CheckBoxModel RSACheckBox { get; set; }

        public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        public CheckBoxModel VisibleColorCheckBox { get; set; }



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

        #endregion


        #region VARS

        private OpenFileDialog openFileDialog;

        private string pathToDirOrigFile;
        private string filenameOrigFile;

        private int maxLettersIsCanHide;
        #endregion

        #region RelayCommands

        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand HideInformationRelayCommand { get; private set; }

        #endregion

        #region Constructor and Initializers

        public HideAproshViewModel()
        {
            UIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {
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
            AdditionalBitsCheckBox = new CheckBoxModel();
            RandomCheckBox = new CheckBoxModel();
            VisibleColorCheckBox = new CheckBoxModel();
        }


        #endregion


        #region RelayCommands
        private void OpenDocument()
        {
            if (OpenFileDialog(openFileDialog) != null)
            {
                FullPathToOrigFile = openFileDialog.FileName;
                filenameOrigFile = openFileDialog.SafeFileName;
                pathToDirOrigFile = fullPathToOrigFile.Substring(0, fullPathToOrigFile.Length - filenameOrigFile.Length);

                CountLettersIsCanHide = HideColorModel.HowMuchLettersICanHide(fullPathToOrigFile).ToString();
                maxLettersIsCanHide = Int32.Parse(countLettersIsCanHide);
                if (Int32.Parse(countLettersIsCanHide) > 0)
                {
                    IsTextForHideEnabled = true;
                    IsHideInformationButtonEnabled = true;

                    RandomCheckBox.IsEnabled = true;
                    RSACheckBox.IsEnabled = true;
                    AdditionalBitsCheckBox.IsEnabled = true;
                    VisibleColorCheckBox.IsEnabled = true;
                }
                else
                {
                    ShowMetroMessageBox("Ошибка", "В данном файле невозможно скрыть информацию.");
                }
            }
        }

        private void HideInformation()
        {
            if (textForHide.Length > 0)
            {
                
                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;

                textForHide = (RSACheckBox.IsChecked)
                    ? Converter.RsaCryptor(TextForHide, pathToDirOrigFile)
                    : Converter.StringToBinary(TextForHide);

                textForHide = (AdditionalBitsCheckBox.IsChecked)
                    ? HideColorModel.AddAdditionalBits(textForHide)
                    : textForHide;

                HideAproshModel codeModel = new HideAproshModel(pathToNewFile);
                isSuccesful = codeModel.HideInformation(textForHide.ToCharArray(), VisibleColorCheckBox.IsChecked, RandomCheckBox.IsChecked, ZeroBitSpacing, SoloBitSpacing);

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
                openFileDialog.Filter = "Файлы Microsoft Word|*.doс;*.doсx; | Все файлы|*.*";
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
            var dialog = new ModernDialog()
            {
                Title = title,
                Content = message
            };

            dialog.ShowDialog();
        }
    }
}
