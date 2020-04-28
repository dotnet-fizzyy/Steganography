using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SteganoGraphyXML
{
    class Program
    {
        private const string SourcePath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\LB-4.docx";
        private const string DestinationPath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\Aspose.docx";
        private const string XMLPath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\test2.xml";

        static void Main(string[] args)
        {
            Document doc = new Document(SourcePath);
            var nodes = doc.GetChildNodes(NodeType.Run, true);

            Random rand = new Random();
            int randomParagraph = rand.Next(0, nodes.Count);

            Console.Write("Enter info: ");
            string sourceInfo = Console.ReadLine();

            var node = doc.GetChildNodes(NodeType.Run, true)[randomParagraph];
            DocumentBuilder documentBuilder = new DocumentBuilder(doc);
            documentBuilder.MoveTo(node);
            ((Run)documentBuilder.CurrentNode).Font.NameBi = sourceInfo;
            ((Run)documentBuilder.CurrentNode).Font.NameFarEast = "secret";
            doc.Save(DestinationPath, SaveFormat.Docx);
            doc.Save(XMLPath, SaveFormat.WordML);

            XmlDocument document = new XmlDocument();
            document.Load(XMLPath);
            var foundNode = FindNode(document, "w:fareast");
            if (foundNode != null)
            {
                Console.WriteLine("Found info: " + foundNode.Attributes["w:cs"].Value);
            }
        }

        private static XmlNode FindNode(XmlDocument doc, string attributeName)
        {
            var items = doc.GetElementsByTagName("w:rPr").Cast<XmlNode>().ToList();
            var childNodes = items.Select(node => node.ChildNodes.Cast<XmlNode>().FirstOrDefault(elem => elem.Name == "w:rFonts")).Where(node => node != null);
            return childNodes.FirstOrDefault(item => item.Attributes[attributeName]?.Value == "secret");
        }
    }
}
