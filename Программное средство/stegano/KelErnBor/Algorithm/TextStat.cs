using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model
{
    public class TextStat
    {
        public static int HowMuchLettersICanHide(string pathToFile)
        {
            try
            {
                int counterLetters = 0;

                Document originalDocument = new Document(pathToFile);
                NodeCollection paragraphs = originalDocument.GetChildNodes(NodeType.Paragraph, true);

                foreach (Paragraph parag in paragraphs)
                {
                    counterLetters += parag.Range.Text.Length;
                }

                return counterLetters > 0 ? counterLetters : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static Task<Dictionary<string, int>> GetFontStat(string pathToFile)
        {
            try
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                Document document = new Document(pathToFile);
                DocumentBuilder documentBuilder = new DocumentBuilder(document);
                string fontName = "";

                foreach (Run run in document.GetChildNodes(NodeType.Run, true))
                {
                    fontName = run.Font.Name;

                    if (!dictionary.ContainsKey(fontName))
                    {
                        dictionary.Add(fontName, 1);
                    }

                    dictionary[fontName]++;
                }

                return Task.FromResult(dictionary);
            }
            catch
            {
                return null;
            }
        }
    }
}
