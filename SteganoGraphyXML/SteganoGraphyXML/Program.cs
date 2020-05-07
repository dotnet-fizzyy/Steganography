using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace SteganoGraphyXML
{
    class Program
    {
        private const string SourcePath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\LB-4.docx";
        private const string DestinationPath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\Aspose.docx";
        private const string XMLPath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\test2.xml";
        private const int BlockSize = 10;

        static void Main(string[] args)
        {
            Document doc = new Document(SourcePath);
            var nodes = doc.GetChildNodes(NodeType.Run, true);

            Random rand = new Random();

            Console.Write("Enter info: ");
            string sourceInfo = Console.ReadLine();

            //RSA crypt
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            byte[] plainText = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(sourceInfo)));
            byte[] encryptedTextInBytes = RSAEncode.Encryption(plainText, RSA.ExportParameters(false), false);

            string encryptedText = Convert.ToBase64String(encryptedTextInBytes);
            Console.WriteLine("Encrypted string: " + encryptedText);

            double amountOfBlocks = Math.Ceiling(Convert.ToDouble(encryptedText.Length) / Convert.ToDouble(BlockSize));
            int counter = 0;
            List<int> repeatedParagraphs = new List<int>(); 

            for (int i = 0; i < amountOfBlocks; i++)
            {
                int randomParagraph = 0;
                do
                {
                    randomParagraph = rand.Next(0, nodes.Count);
                }
                while (repeatedParagraphs.Contains(randomParagraph));
                repeatedParagraphs.Add(randomParagraph);

                int shouldTake = i == amountOfBlocks - 1 ? encryptedText.Length - counter : BlockSize;
                string part = encryptedText.Substring(counter, shouldTake);

                var node = doc.GetChildNodes(NodeType.Run, true)[randomParagraph];
                DocumentBuilder documentBuilder = new DocumentBuilder(doc);
                documentBuilder.MoveTo(node);
                ((Run)documentBuilder.CurrentNode).Font.NameBi = part;
                ((Run)documentBuilder.CurrentNode).Font.NameFarEast = "secret" + i;
                counter += BlockSize;
            }

            doc.Save(DestinationPath, SaveFormat.Docx);

            Document doc2 = new Document(DestinationPath);
            doc2.Save(XMLPath);

            XmlDocument document = new XmlDocument();
            document.Load(XMLPath);

            var nodesWithMessage = FindNodes(document, "w:fareast");
            var messages = nodesWithMessage.Select(node => node.Attributes["w:cs"]?.Value);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var message in messages)
            {
                stringBuilder.Append(message);
            }

            var decMessage = stringBuilder.ToString();
            var decryptedBytes = Convert.FromBase64String(decMessage);

            byte[] decryptedText = RSAEncode.Decryption(decryptedBytes, RSA.ExportParameters(true), false);
            string originalInfo = Encoding.UTF8.GetString(decryptedText);

            Console.WriteLine("RSA decrypt message: " + originalInfo);
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
