using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aspose.Words;
using FirstFloor.ModernUI.Windows.Controls;
using Stegano.Algorithm;

namespace Stegano.Model.Underline
{
    class ShowUnderlineModel
    {
        private Document wordDoc;
        private const string FileName = "privateKey.txt";
        public ShowUnderlineModel(string pathToFile)
        {
            wordDoc = new Document(pathToFile);
        }
               
        public static string RemoveAdditBits(string value)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value.Length; i += 7)
            {
                The_Cyclic_Code_WPF.Classes.Decoder decoder = new The_Cyclic_Code_WPF.Classes.Decoder(value.Substring(i, 4), value.Substring(i, 7));
                decoder.deCode();
                var temp = decoder.print();
                if (temp.Length == 3)
                {
                    temp += "0";
                }
                sb.Append(temp.Substring(0, 4));
            }

            return sb.ToString();
        }

        public Task<string> FindInformation()
        {
            string foundedMessageInDocument = "";

            DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);
            
            foreach (Run run in wordDoc.GetChildNodes(NodeType.Run, true))
            {
                if (run.Font.Underline == Aspose.Words.Underline.DotDotDashHeavy)
                    foundedMessageInDocument += "1";
                if (run.Font.Underline == Aspose.Words.Underline.WavyHeavy)
                    foundedMessageInDocument += "0";
            }

            
            return Task.FromResult(foundedMessageInDocument);
        }
    }
}
