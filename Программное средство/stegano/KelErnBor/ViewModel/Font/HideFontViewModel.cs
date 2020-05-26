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
                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;
                
                ShowMetroMessageBox(textForHide, Converter.StringToBinary(TextForHide));

                //textForHide = (RSACheckBox.IsChecked)
                //    ? Converter.RsaCryptor(TextForHide, pathToDirOrigFile)
                //    : Converter.StringToBinary(TextForHide);

                //textForHide = SelectedCodMethod?.Coding(Converter.StringToBinary(TextForHide)) ?? Converter.StringToBinary(TextForHide);
                textForHide = messageTransformation(Converter.StringToBinary(TextForHide), out string hash);

                HideFontModel codeModel = new HideFontModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), CurrentShift, RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked, OneFontName, ZeroFontName);

                if (isSuccesful)
                {
                    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);
                }
                else
                {
                    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
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
