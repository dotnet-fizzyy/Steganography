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


                string pathToNewFile = DocumentHelper.CopyFile(pathToDirOrigFile, filenameOrigFile);
                bool isSuccesful = false;

                textForHide = SelectedCryptMethod?.Encrypt(textForHide, pathToDirOrigFile) ?? textForHide;

                textForHide = SelectedCodMethod?.Coding(SelectedCryptMethod != null ? textForHide : Converter.StringToBinary(TextForHide)) ?? TextForHide;


                if (SelectedCryptMethod == null && SelectedCodMethod == null)
                {
                    textForHide = Converter.StringToBinary(textForHide);
                }

                HideUnderlineModel codeModel = new HideUnderlineModel(pathToNewFile);
                isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked);


                var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null || SelectedCodMethod != null ? TextForHide : Converter.BinaryToString(TextForHide)) ?? TextForHide;
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    MD5.SaveHash(pathToDirOrigFile, hash);
                }

                if (isSuccesful)
                {
                    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);

                }
                else
                {
                    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
                }
                //// ShowMetroMessageBox(textForHide, Converter.StringToBinary(TextForHide));

                //MessageBox.Show(textForHide);

                //textForHide = messageTransformation(Converter.StringToBinary(TextForHide), out string hash);
                //MessageBox.Show(textForHide);

                ////textForHide = SelectedCryptMethod?.Encrypt(textForHide, pathToDirOrigFile)
                ////    ?? textForHide;



                ////textForHide = SelectedCodMethod?.Coding(SelectedCryptMethod != null
                ////    ? textForHide : Converter.StringToBinary(TextForHide))
                ////    ?? TextForHide;
                ////MessageBox.Show(textForHide);



                //HideUnderlineModel codeModel = new HideUnderlineModel(pathToNewFile);
                //isSuccesful = await codeModel.HideInformation(textForHide.ToCharArray(), RandomCheckBox.IsChecked, VisibleColorCheckBox.IsChecked);
                //MessageBox.Show(textForHide);


                ////var hash = SelectedHashMethod?.GetHash(SelectedCryptMethod == null || SelectedCodMethod != null ? TextForHide : Converter.BinaryToString(TextForHide)) ?? TextForHide;
                ////if (!string.IsNullOrWhiteSpace(hash))
                ////{
                ////    MD5.SaveHash(pathToDirOrigFile, hash); //Mocked until base class will not be implemented
                ////}

                //if (isSuccesful)
                //{
                //    ShowMetroMessageBox("Информация", "Скрытие информации прошло успешно.\n\nПуть к измененному файлу: " + pathToNewFile);
                //}
                //else
                //{
                //    ShowMetroMessageBox("Информация", "Во время выполнения произошла ошибка.");
                //}

            }
            else
            {
                ShowMetroMessageBox("Ошибка", "Введите текст.");
            }
           
        }


        #endregion


        

       
        
    }
}
