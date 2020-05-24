using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aspose.Words;

namespace Stegano.Algorithm
{
    public class AttributeHiding
    {
        private int blockSize = 10;
        private readonly bool shouldEncrypt;
        private readonly Document document;
        private readonly bool shouldHightlight;
        private readonly string pathToFile;

        private enum BlockSizes
        {
            SmallAmountOfParaghraphs = 10,
            MiddleAmountOfParaghraphs = 100,
            BigAmountOfParaghraphs = 200
        }

        public AttributeHiding(string pathToFile)
        {
            this.document = new Document(pathToFile);
            this.pathToFile = pathToFile;
        }

        public AttributeHiding(Document doc, bool shouldEncrypt, bool shouldHightlight, string pathToFile)
        {
            document = doc;
            this.shouldEncrypt = shouldEncrypt;
            this.shouldHightlight = shouldHightlight;
            this.pathToFile = pathToFile;
        }

        private bool CompareNodesAndStringLength(int amountOfParagraphs, int textLength)
        {
            if (!shouldEncrypt)
            {
                if (amountOfParagraphs < (int)BlockSizes.SmallAmountOfParaghraphs)
                {
                    blockSize = 20;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
                else if (amountOfParagraphs > (int)BlockSizes.SmallAmountOfParaghraphs || amountOfParagraphs < (int)BlockSizes.MiddleAmountOfParaghraphs)
                {
                    blockSize = 15;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
                else
                {
                    blockSize = 10;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
            }
            else
            {
                if (amountOfParagraphs < (int)BlockSizes.SmallAmountOfParaghraphs)
                {
                    blockSize = 20;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
                else if (amountOfParagraphs > 10 || amountOfParagraphs < 100)
                {
                    blockSize = 15;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
                else
                {
                    blockSize = 10;
                    if (textLength > amountOfParagraphs * blockSize) return false;

                    return true;
                }
            }
        }

        public bool HideInfoInAttribute(string textForHiding)
        {
            DocumentBuilder documentBuilder = new DocumentBuilder(document);
            var documentNodes = document.GetChildNodes(NodeType.Run, true);

            var isPossibleToHide = CompareNodesAndStringLength(documentNodes.Count, textForHiding.Length);
            if (!isPossibleToHide)
            { 
                return false; 
            }

            var rand = new Random();

            int amountOfBlocks = (int)Math.Ceiling(Convert.ToDouble(textForHiding.Length) / Convert.ToDouble(blockSize));
            int counter = 0;
            List<int> repeatedParagraphs = new List<int>();

            for (int i = 0; i < amountOfBlocks; i++)
            {
                int randomParagraph;
                do
                {
                    randomParagraph = rand.Next(0, documentNodes.Count);
                }
                while (repeatedParagraphs.Contains(randomParagraph));
                repeatedParagraphs.Add(randomParagraph);

                int shouldTake = i == amountOfBlocks - 1 ? textForHiding.Length - counter : blockSize;
                string part = textForHiding.Substring(counter, shouldTake);

                var node = documentNodes[randomParagraph];
                documentBuilder.MoveTo(node);
                ((Run)documentBuilder.CurrentNode).Font.NameBi = part;
                ((Run)documentBuilder.CurrentNode).Font.NameFarEast = "secret" + i;

                ((Run)documentBuilder.CurrentNode).Font.Color = shouldHightlight ? ColorTranslator.FromHtml("#ed0459") : ((Run)documentBuilder.CurrentNode).Font.Color;
                counter += blockSize;
            }

            document.Save(pathToFile, SaveFormat.Docx);

            return true;
        }

        public string GetHiddenInfoInAttribute()
        {
            var fileSavePlace = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp.xml";
            document.Save(fileSavePlace);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileSavePlace);

            var nodesWithMessage = FindNodes(xmlDocument, "w:fareast");
            var messages = nodesWithMessage.Select(node => node.Attributes["w:cs"]?.Value);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var message in messages)
            {
                stringBuilder.Append(message);
            }

            return stringBuilder.ToString();
        }

        private static IEnumerable<XmlNode> FindNodes(XmlDocument doc, string attributeName)
        {
            var items = doc.GetElementsByTagName("w:rPr").Cast<XmlNode>().ToList();
            var childNodes = items.Select(node => node.ChildNodes.Cast<XmlNode>().FirstOrDefault(elem => elem.Name == "w:rFonts")).Where(node => node != null);
            var nodes = childNodes.Where(item => item.Attributes[attributeName] != null && item.Attributes[attributeName].Value.Contains("secret")).OrderBy(attr => int.Parse(attr.Attributes[attributeName].Value.Remove(0, 6)));

            return nodes;
        }
    }
}
