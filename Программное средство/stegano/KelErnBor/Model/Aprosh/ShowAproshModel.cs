using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Stegano.Algorithm;
using The_Cyclic_Code_WPF.Classes;

namespace Stegano.Model.Aprosh
{
    class ShowAproshModel
    {
        private Document wordDoc;

        public ShowAproshModel(string pathToFile)
        {
            wordDoc = new Document(pathToFile);
        }

        public static string RemoveAdditBits(string value)
        {
            
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value.Length; i += 7)
            {
                The_Cyclic_Code_WPF.Classes.Decoder decoder = new The_Cyclic_Code_WPF.Classes.Decoder(value.Substring(i, 4),value);
                decoder.deCode();
                sb.Append(decoder.print().Substring(0, 4));
            }
                

            return sb.ToString();
        }

        public Task<string> FindInformation(string zeroBitSpacing, string soloBitSpacing)
        {
            string foundedMessageInDocument = string.Empty;

            DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);

            DocumentHelper.CutParagraphToRun(ref wordDoc, ref documentBuilder);

            foreach (Run run in wordDoc.GetChildNodes(NodeType.Run, true))
            {


                if (run.Font.Spacing == Double.Parse(soloBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture))
                    foundedMessageInDocument += "1";
                if (run.Font.Spacing == Double.Parse(zeroBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture))
                    foundedMessageInDocument += "0";

            }
            
            return Task.FromResult(foundedMessageInDocument);
        }

    }
}
