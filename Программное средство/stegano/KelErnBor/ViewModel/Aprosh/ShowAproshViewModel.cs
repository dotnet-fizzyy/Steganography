using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model.Aprosh;
using Stegano.Model.ColorSteg;
using Stegano.Model;
using Microsoft.Win32;

namespace Stegano.ViewModel.Aprosh
{
    public class ShowAproshViewModel : BaseShowViewModel
    {
       
        public string ZeroBitSpacing { get; set; }
        public string SoloBitSpacing { get; set; }

        public ShowAproshViewModel()
        {
            DecodeUIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);
        }

        private void DecodeUIInit()
        {
            AdditionalBitsCheckBox = new CheckBoxModel(true, false);
        }              

        private async void OpenForDecode()
        {
            try
            {
                if (string.IsNullOrEmpty(PathToDoc))
                {
                    ShowMetroMessageBox("Информация", "Загрузите файл для дешифровки");
                    return;
                }

                CryptedText = string.Empty;
                SearchedText = string.Empty;
                TimeForDerypting = string.Empty;

                Stopwatch.Restart();
                ShowAproshModel codeModel = new ShowAproshModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation(ZeroBitSpacing, SoloBitSpacing);
                SearchedText = SelectedCodMethod == null ? SearchedText = Converter.BinaryToString(foundedBitsInDoc) : foundedBitsInDoc;

                if (SelectedCodMethod != null)
                {
                    EncodedText = SearchedText;

                    SearchedText = Converter.BinaryToString(SelectedCodMethod.DeCoding(SearchedText));
                }

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

                if (SelectedCryptMethod != null)
                {
                    CryptedText = SearchedText;

                    if (string.IsNullOrEmpty(CryptFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                        return;
                    }

                    SearchedText = SelectedCryptMethod?.Decrypt(SearchedText, CryptFile) ?? SearchedText;

                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Информация", "Ключ не подходит.");
                        return;
                    }
                }

                Stopwatch.Stop();
                TimeForDerypting = Math.Round(Stopwatch.Elapsed.TotalSeconds, 2).ToString() + " сек.";

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
    }
}
