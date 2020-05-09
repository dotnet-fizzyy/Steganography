using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        private const int AccessibleMessageLenghtForRSA = 100;

        private enum BlockSizes
        {
            SmallAmountOfParaghraphs = 10,
            MiddleAmountOfParaghraphs = 100,
            BigAmountOfParaghraphs = 200
        }

        public AttributeHiding(string pathToFile, bool shouldEncrypt, bool shouldHightlight)
        {
            document = new Document(pathToFile);
            this.shouldEncrypt = shouldEncrypt;
            this.shouldHightlight = shouldHightlight;
            this.pathToFile = pathToFile;
        }

        private bool BlockMapper(int amountOfParagraphs, int textLength)
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
                if (textLength >= AccessibleMessageLenghtForRSA || amountOfParagraphs < (int)BlockSizes.SmallAmountOfParaghraphs) return false;

                if (amountOfParagraphs > 10 || amountOfParagraphs < 100)
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

            var isPossibleToHide = BlockMapper(documentNodes.Count, textForHiding.Length);

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

            GetHiddenInfoInAttribute();

            return true;
        }

        public string GetHiddenInfoInAttribute()
        {
            document.Save(pathToFile, SaveFormat.WordML);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pathToFile);

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
