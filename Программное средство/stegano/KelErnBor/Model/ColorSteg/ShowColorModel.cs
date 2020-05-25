using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Stegano.Algorithm;

namespace Stegano.Model.ColorSteg
{
    class ShowColorModel
    {
        private Document wordDoc;

        public ShowColorModel(string pathToFile)
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

        public  Task<string> FindInformation(bool isSmartHiding)
        {
            string foundedMessageInDocument = string.Empty;

            DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);

            DocumentHelper.CutParagraphToRun(ref wordDoc, ref documentBuilder);

            if (isSmartHiding)
            {
                foundedMessageInDocument = FindCommonDocColor();
            }
            else
            {
                foreach (Run run in wordDoc.GetChildNodes(NodeType.Run, true))
                {

                    if (run.Font.Color == ColorTranslator.FromHtml("#000001"))
                        foundedMessageInDocument += "1";
                    if (run.Font.Color == ColorTranslator.FromHtml("#000002"))
                        foundedMessageInDocument += "0";

                    if (run.Font.Color == ColorTranslator.FromHtml("#0459ED"))
                        foundedMessageInDocument += "1";
                    if (run.Font.Color == ColorTranslator.FromHtml("#ed0459"))
                        foundedMessageInDocument += "0";

                }
            }

            return Task.FromResult(foundedMessageInDocument);
        }

        private string FindCommonDocColor()
        {
            var color = ((Run) wordDoc.GetChildNodes(NodeType.Run, true)[wordDoc.GetChildNodes(NodeType.Run, true).Count / 2]).Font.Color; //перевод из RGB->16->10
            var decBaseColor = Convert.ToInt32(Converter.ArgbToHex(color),16);
            string message = string.Empty;
            foreach (Run run in wordDoc.GetChildNodes(NodeType.Run, true))
            {
                if (decBaseColor + 1 == Convert.ToInt32(Converter.ArgbToHex(run.Font.Color),16))
                    message += "1";
                else if (decBaseColor + 2 == Convert.ToInt32(Converter.ArgbToHex(run.Font.Color),16))
                    message += "0";
           
            }

            return message;
        }
    }
}
