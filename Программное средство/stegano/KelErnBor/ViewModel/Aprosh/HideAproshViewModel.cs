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
using Stegano.Model.ColorSteg;
using Stegano.Model.Aprosh;

namespace Stegano.ViewModel.Aprosh
{
    public class HideAproshViewModel : BaseHideViewModel
    {
        public string ZeroBitSpacing { get; set; }
        public string SoloBitSpacing { get; set; }

        private string sourceString = string.Empty;

       

        public HideAproshViewModel()
        {
            UIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {           
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
        }


        private void HideInformation()
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

                textForHide = SelectedCryptMethod?.Encrypt(textForHide, pathToDirOrigFile) ?? textForHide;

                textForHide = SelectedCodMethod?.Coding(SelectedCryptMethod != null ? textForHide : Converter.StringToBinary(TextForHide)) ?? TextForHide;


                if (SelectedCryptMethod == null && SelectedCodMethod == null)
                {
                    textForHide = Converter.StringToBinary(textForHide);
                }

                HideAproshModel codeModel = new HideAproshModel(pathToNewFile);
                isSuccesful = codeModel.HideInformation(textForHide.ToCharArray(), VisibleColorCheckBox.IsChecked, RandomCheckBox.IsChecked, ZeroBitSpacing, SoloBitSpacing);


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
            }
            else
            {
                ShowMetroMessageBox("Ошибка", "Введите текст.");
            }         
        }       
    }
}

