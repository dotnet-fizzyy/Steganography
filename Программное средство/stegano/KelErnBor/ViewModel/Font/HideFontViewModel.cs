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
using System.Collections.ObjectModel;
using Stegano.Algorithm.Aditional_Coding;

namespace Stegano.ViewModel.Font
{
    public class HideFontViewModel : BaseHideViewModel
    {
        #region Properties

        public string OneFontName { get; set; }
        public string ZeroFontName { get; set; }

        private string sourceString = string.Empty;

        #endregion

        #region Constructor and Initializers

        public HideFontViewModel()
        {
            FontStats = new ObservableCollection<object>();
            HideInformationRelayCommand = new RelayCommand(HideInformation);
        }

        #endregion

        #region RelayMethods
       

        private async void HideInformation()
        {
            if (textForHide.Length > 0)
            {
                sourceString = textForHide;
                TimeForCrypting = string.Empty;

                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;

                Stopwatch.Start();
                sourceString = SelectedCryptMethod?.Encrypt(sourceString, pathToDirOrigFile) ?? sourceString;

                var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null ? sourceString : Converter.BinaryToString(sourceString));
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    MD5.SaveHash(pathToDirOrigFile, hash); //Mocked until base class will not be implemented
                }

                sourceString = SelectedCodMethod?.Coding(SelectedCryptMethod != null ? sourceString : Converter.StringToBinary(sourceString)) ?? sourceString;

                HideFontModel codeModel = new HideFontModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(sourceString.ToCharArray(), CurrentShift, RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked, OneFontName, ZeroFontName);

                Stopwatch.Stop();
                TimeForCrypting = Math.Round(Stopwatch.Elapsed.TotalSeconds, 2).ToString() + " сек.";

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
    }
}
