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
using Stegano.Algorithm.Aditional_Coding;
using Stegano.Model.Font;
using Stegano.Model.Underline;

namespace Stegano.ViewModel.Underline
{
    public class HideUnderlineViewModel : BaseHideViewModel
    {

        public HideUnderlineViewModel()
        {
            HideInformationRelayCommand = new RelayCommand(HideInformation);

            UIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
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
        }

        private void RelayInit()
        {
            HideInformationRelayCommand = new RelayCommand(HideInformation);
        }

        private string sourceString = string.Empty;


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

                HideUnderlineModel codeModel = new HideUnderlineModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked);
                
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

        }


        #endregion






    }
}