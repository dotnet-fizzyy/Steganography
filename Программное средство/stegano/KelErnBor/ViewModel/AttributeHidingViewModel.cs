using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Algorithm.Aditional_Coding;
using Stegano.Model;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using Stegano.Interfaces;
using Stegano.Model.ColorSteg;

namespace Stegano.ViewModel
{
    public class AttributeHidingViewModel : BaseHideViewModel
    {
        #region Properties

        //private string fullPathToOrigFile;
        //public string FullPathToOrigFile
        //{
        //    get { return "Путь к файлу: " + fullPathToOrigFile; }
        //    set { fullPathToOrigFile = value; RaisePropertyChanged(); }
        //}

        //private string countLettersIsCanHide;
        //public string CountLettersIsCanHide
        //{
        //    get { return $"Количество символов, которые можно скрыть: {countLettersIsCanHide}"; }
        //    set
        //    {
        //        countLettersIsCanHide = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string textForHide = "";
        //public string TextForHide
        //{
        //    get { return textForHide; }
        //    set
        //    {
        //        TextForHideChanged(value);
        //        RaisePropertyChanged();
        //    }
        //}


        //private string countLettersForHide;
        //public string CountLettersForHide
        //{
        //    get { return $"Введено символов: {countLettersForHide}"; }
        //    set
        //    {
        //        countLettersForHide = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string keyStatus;
        //public string KeyStatus
        //{
        //    private get { return keyStatus; }
        //    set
        //    {
        //        keyStatus = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private SolidColorBrush keyStatusColor;
        //public SolidColorBrush KeyStatusColor
        //{
        //    get { return keyStatusColor; }
        //    set
        //    {
        //        keyStatusColor = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private bool isTextForHideEnabled;
        //public bool IsTextForHideEnabled
        //{
        //    get { return isTextForHideEnabled; }
        //    set
        //    {
        //        isTextForHideEnabled = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private string sourceString = string.Empty;


        //public CheckBoxModel RSACheckBox { get; set; }

        //public CheckBoxModel AESCheckBox { get; set; }

        //public CheckBoxModel TwoFishCheckBox { get; set; }

        //public CheckBoxModel AdditionalBitsCheckBox { get; set; }

        //public CheckBoxModel VisibleColorCheckBox { get; set; }

        //public CheckBoxModel HashingSHA512CheckBox { get; set; }

        //public CheckBoxModel HashingMD5CheckBox { get; set; }


        //private bool isHideInformationButtonEnabled;
        //public bool IsHideInformationButtonEnabled
        //{
        //    get { return isHideInformationButtonEnabled; }
        //    set
        //    {
        //        isHideInformationButtonEnabled = value;
        //        RaisePropertyChanged();
        //    }
        //}

        #endregion

        #region RelayCommands

        //public RelayCommand OpenDocumentRelayCommand { get; private set; }
        //public RelayCommand HideInformationRelayCommand { get; private set; }

        //public ObservableCollection<ICod> CodMethods { get; set; }
        //public ICod SelectedCodMethod { get; set; }

        //public ObservableCollection<ICrypt> CryptMethods { get; set; }
        //public ICrypt SelectedCryptMethod { get; set; }

        //public ObservableCollection<IHash> HashMethods { get; set; }
        //public IHash SelectedHashMethod { get; set; }

        //#endregion

        //#region VARS

        //private OpenFileDialog openFileDialog;

        //private string pathToDirOrigFile;
        //private string filenameOrigFile;

        //private int maxLettersIsCanHide;
        #endregion

        #region Constructor and Initializers

        public AttributeHidingViewModel()
        {
            UIInit();
            openFileDialog = new OpenFileDialog();
            RelayInit();
            //CodMethodsInit();
            //CryptMethodsInit();
            //HashMethodsInit();
        }

        private void RelayInit()
        {
            //OpenDocumentRelayCommand = new RelayCommand(OpenDocument);
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

            //RSACheckBox = new CheckBoxModel();
            //VisibleColorCheckBox = new CheckBoxModel();
            //AdditionalBitsCheckBox = new CheckBoxModel();
            //HashingMD5CheckBox = new CheckBoxModel();
            //HashingSHA512CheckBox = new CheckBoxModel();
            //AESCheckBox = new CheckBoxModel();
            //TwoFishCheckBox = new CheckBoxModel();
        }

        //private void CodMethodsInit()
        //{
        //    CodMethods = new ObservableCollection<ICod>
        //    {
        //        new CyclicCod(),
        //        new HammingCod(16, false),
        //        new HammingCod(16, true),
        //    };
        //}

        //private void CryptMethodsInit()
        //{
        //    CryptMethods = new ObservableCollection<ICrypt>
        //    {
        //        new AES(),
        //        new RSA(),
        //        new TwoFish()
        //    };
        //}

        //private void HashMethodsInit()
        //{
        //    HashMethods = new ObservableCollection<IHash>
        //    {
        //        new SHA512(),
        //        new MD5(),
        //    };
        //}

        #endregion

        #region RelayMethods
        //private void OpenDocument()
        //{
        //    if (OpenFileDialog(openFileDialog) != null)
        //    {
        //        FullPathToOrigFile = openFileDialog.FileName;
        //        filenameOrigFile = openFileDialog.SafeFileName;
        //        pathToDirOrigFile = fullPathToOrigFile.Substring(0, fullPathToOrigFile.Length - filenameOrigFile.Length);

        //        CountLettersIsCanHide = HideColorModel.HowMuchLettersICanHide(fullPathToOrigFile).ToString();
        //        maxLettersIsCanHide = Int32.Parse(countLettersIsCanHide);
        //        if (Int32.Parse(countLettersIsCanHide) > 0)
        //        {
        //            IsTextForHideEnabled = true;
        //            IsHideInformationButtonEnabled = true;

        //            //HashingMD5CheckBox.IsEnabled = true;
        //            //RSACheckBox.IsEnabled = true;
        //            //AdditionalBitsCheckBox.IsEnabled = true;
        //            //VisibleColorCheckBox.IsEnabled = true;
        //            //HashingSHA512CheckBox.IsEnabled = true;
        //            //AESCheckBox.IsEnabled = true;
        //            //TwoFishCheckBox.IsEnabled = true;
        //        }
        //        else
        //        {
        //            ShowMetroMessageBox("Ошибка", "В данном файле невозможно скрыть информацию.");
        //        }
        //    }
        //}

        private async void HideInformation()
        {
            if (textForHide.Length > 0)
            {
                sourceString = textForHide;

                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;

                sourceString = SelectedCryptMethod?.Encrypt(sourceString, pathToDirOrigFile) ?? sourceString;

                sourceString = SelectedCodMethod?.Coding(SelectedCryptMethod != null ? sourceString : Converter.StringToBinary(sourceString)) ?? sourceString;

                AttributeHidingModel codeModel = new AttributeHidingModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(sourceString.ToCharArray(), VisibleColorCheckBox.IsChecked, SelectedCodMethod != null, SelectedCryptMethod != null);

                var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null || SelectedCodMethod != null ? sourceString : Converter.BinaryToString(sourceString)) ?? sourceString;
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    MD5.SaveHash(pathToDirOrigFile, hash); //Mocked until base class will not be implemented
                }

                if (isSuccesful)
                {
                    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);
                    KeyStatus = (SelectedCryptMethod != null) ? "сгенерирован" : "выключен";
                    KeyStatusColor = (SelectedCryptMethod != null) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
                    KeyStatus = (SelectedCryptMethod != null) ? "ошибка генерации" : "выключен RSA";
                    KeyStatusColor = (SelectedCryptMethod != null) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
                }
            }
            else
            {
                ShowMetroMessageBox("Ошибка", "Введите текст.");
            }
            //TextForHide = String.Empty;
        }


        #endregion


        //private void TextForHideChanged(string text)
        //{
        //    textForHide = text;
        //    CountLettersForHide = textForHide.Length.ToString();
        //    int letterWeigth = 0;
        //    if (AdditionalBitsCheckBox != null)
        //        letterWeigth = AdditionalBitsCheckBox.IsChecked ? Int32.Parse(countLettersForHide) * 4 : Int32.Parse(countLettersForHide);
        //    CountLettersIsCanHide = (maxLettersIsCanHide - letterWeigth).ToString();
        //}

        //private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
        //{
        //    try
        //    {
        //        openFileDialog.Filter = "Файлы Microsoft Word|*.doc;*.docx; | Все файлы|*.*";
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
        //    var dialog = new ModernDialog()
        //    {
        //        Title = title,
        //        Content = message
        //    };

        //    dialog.ShowDialog();
        //}
    }
}
