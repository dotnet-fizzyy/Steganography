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

                CryptedText = "";
                SearchedText = "";
                ShowFontModel codeModel = new ShowFontModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation(OneFontName,ZeroFontName);

                foundedBitsInDoc = messageTransformation(foundedBitsInDoc);
                
                SearchedText = Converter.BinaryToString(foundedBitsInDoc);

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
