using System;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model.Font;
using System.Collections.ObjectModel;

namespace Stegano.ViewModel.Font
{
    public class ShowFontViewModel : BaseShowViewModel
    {
        #region Properties
        public string OneFontName { get; set; }
        public string ZeroFontName { get; set; }

        #endregion


        #region Constructor and Initializers

        public ShowFontViewModel() : base()
        {
            FontStats = new ObservableCollection<object>();
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);
        }

        #endregion

        #region RelayMethods

        private async void OpenForDecode()
        {
            try
            {
                if (string.IsNullOrEmpty(PathToDoc))
                {
                    ShowMetroMessageBox("Информация", "Загрузите файл для извлечения");
                    return;
                }

                CryptedText = string.Empty;
                SearchedText = string.Empty;
                TimeForDerypting = string.Empty;

                Stopwatch.Start();
                ShowFontModel codeModel = new ShowFontModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation(OneFontName, ZeroFontName);

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
        #endregion
    }
}
