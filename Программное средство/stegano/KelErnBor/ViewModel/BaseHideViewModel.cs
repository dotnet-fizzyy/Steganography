using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Stegano.Model.Font;
using Microsoft.Win32;
using Aspose.Words.Lists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Words;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Stegano.Algorithm.Aditional_Coding;
using Stegano.Interfaces;

namespace Stegano.ViewModel
{
    public class BaseHideViewModel : ViewModelBase
    {
        #region Properties

        protected string fullPathToOrigFile;
        public string FullPathToOrigFile
        {
            get { return "Путь к файлу: " + fullPathToOrigFile; }
            set { fullPathToOrigFile = value; RaisePropertyChanged(); }
        }

        public int maxShift;
        public int MaxShift
        {
            get => maxShift - Convert.ToInt32(countLettersForHide);
            set
            {
                maxShift = value;
                RaisePropertyChanged();
            }
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

        protected string textForHide = "";
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

        public CheckBoxModel VisibleColorCheckBox { get; set; }

        public CheckBoxModel SmartHidingCheckBox { get; set; }

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

        public int CurrentShift { get; set; }
        public ObservableCollection<object> FontStats { get; set; }

        public ObservableCollection<ICod> CodMethods { get; set; }

        public ICod SelectedCodMethod { get; set; }

        #endregion

        #region RelayCommands
        public RelayCommand OpenDocumentRelayCommand { get; private set; }
        public RelayCommand HideInformationRelayCommand { get; protected set; }

        #endregion

        #region VARS

        protected OpenFileDialog openFileDialog;

        protected string pathToDirOrigFile;
        protected string filenameOrigFile;

        protected int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public BaseHideViewModel()
        {
            openFileDialog = new OpenFileDialog();


            CodMethodsInit();
            RelayInit();
            UIInit();
        }
        private void UIInit()
        {
            FullPathToOrigFile = "";
            MaxShift = 1;
            CountLettersIsCanHide = 0.ToString();
            CountLettersForHide = 0.ToString();

            TextForHide = "";

            KeyStatusColor = new SolidColorBrush(Colors.Black);
            KeyStatus = "";

            IsTextForHideEnabled = false;
            IsHideInformationButtonEnabled = false;

            VisibleColorCheckBox = new CheckBoxModel();
            RandomCheckBox = new CheckBoxModel();
            SmartHidingCheckBox = new CheckBoxModel();
        }
        private void RelayInit()
        {
            OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
        }

        private void CodMethodsInit()
        {
            CodMethods = new ObservableCollection<ICod>();
            CodMethods.Add(new CyclicCod());
            CodMethods.Add(new HammingCod(16, false));
            CodMethods.Add(new HammingCod(16, true));
        }
        #endregion

        #region Methods
        protected string messageTransformation(string message)
        {
            //
            //сюда нужно добавить шифрование и хэширование
            //
            message = SelectedCodMethod?.Coding(message) ?? message;

            return message;
        }
        protected async void OpenDocument()
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
                MaxShift = count;
                count /= 8;
                CountLettersIsCanHide = count.ToString();
                maxLettersIsCanHide = Int32.Parse(countLettersIsCanHide);

                if (Int32.Parse(countLettersIsCanHide) > 0)
                {
                    IsTextForHideEnabled = true;
                    IsHideInformationButtonEnabled = true;

                    RandomCheckBox.IsEnabled = true;
                    VisibleColorCheckBox.IsEnabled = true;
                    SmartHidingCheckBox.IsEnabled = true;
                }
                else
                {
                    ShowMetroMessageBox("Ошибка", "В данном файле невозможно скрыть информацию.");
                }
            }
        }

        protected void TextForHideChanged(string text)
        {
            textForHide = text;
            CountLettersForHide = textForHide.Length.ToString();
            int letterWeigth = 0;
            letterWeigth = SelectedCodMethod != null ? Int32.Parse(countLettersForHide) * 4 : Int32.Parse(countLettersForHide);
            CountLettersIsCanHide = (maxLettersIsCanHide - letterWeigth).ToString();
        }
        protected OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
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

        protected void ShowMetroMessageBox(string title, string message)
        {
            var dialog = new ModernDialog()
            {
                Title = title,
                Content = message
            };

            dialog.ShowDialog();
        }
        #endregion

    } 
}
