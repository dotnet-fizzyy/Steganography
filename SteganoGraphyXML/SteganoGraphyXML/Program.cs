using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static int BlockSize = 10;
        private static bool shouldEncrypt;


        private const string TestSourcePath = @"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\testDoc.docx";

        static void Main(string[] args)
        {
            Document doc = new Document(TestSourcePath);
            var nodes = doc.GetChildNodes(NodeType.Run, true);

            Random rand = new Random();

            Console.Write("Enter info: ");
            string sourceInfo = Console.ReadLine();
            Console.Write("Encrypt ?: ");
            shouldEncrypt = bool.Parse(Console.ReadLine());

            string sha = SHA512(sourceInfo);
            var canEncrypt = BlockMapper(nodes.Count, sourceInfo.Length);

            //RSA crypt
            string encryptedText = sourceInfo;
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            if (shouldEncrypt)
            {
                byte[] plainText = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(sourceInfo)));
                byte[] encryptedTextInBytes = RSAEncode.Encryption(plainText, RSA.ExportParameters(false), false);

                encryptedText = Convert.ToBase64String(encryptedTextInBytes);
            }
            Console.WriteLine("Encrypted string: " + encryptedText);

            int amountOfBlocks = (int)Math.Ceiling(Convert.ToDouble(encryptedText.Length) / Convert.ToDouble(BlockSize));
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
                if (i == 0)
                {
                    ((Run)documentBuilder.CurrentNode).Font.NameOther = shouldEncrypt.ToString();
                }
                ((Run)documentBuilder.CurrentNode).Font.Color = ColorTranslator.FromHtml("#ed0459");
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
            string originalInfo = decMessage;
            if (shouldEncrypt)
            {
                var decryptedBytes = Convert.FromBase64String(decMessage);

                byte[] decryptedText = RSAEncode.Decryption(decryptedBytes, RSA.ExportParameters(true), false);
                originalInfo = Encoding.UTF8.GetString(decryptedText);
            }

            Console.WriteLine("RSA decrypt message: " + originalInfo);
        }

        private static IEnumerable<XmlNode> FindNodes(XmlDocument doc, string attributeName)
        {
            var items = doc.GetElementsByTagName("w:rPr").Cast<XmlNode>().ToList();
            var childNodes = items.Select(node => node.ChildNodes.Cast<XmlNode>().FirstOrDefault(elem => elem.Name == "w:rFonts")).Where(node => node != null);
            var nodes = childNodes.Where(item => item.Attributes[attributeName] != null && item.Attributes[attributeName].Value.Contains("secret")).OrderBy(attr => int.Parse(attr.Attributes[attributeName].Value.Remove(0, 6)));

           var isEncrypted = nodes.FirstOrDefault(node => bool.TryParse(node.Attributes["w:h-ansi"]?.Value, out shouldEncrypt));
           
            return nodes;
        }

        public static string SHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private static bool BlockMapper(double amountOfParagraphs, int textLength)
        {
            if (!shouldEncrypt)
            {
                if (amountOfParagraphs < 10)
                {
                    BlockSize = 20;
                    if (textLength > amountOfParagraphs * BlockSize) return false;

                    return true;
                }
                else if (amountOfParagraphs > 10 || amountOfParagraphs < 100)
                {
                    BlockSize = 15;
                    if (textLength > amountOfParagraphs * BlockSize) return false;

                    return true;
                }
                else
                {
                    BlockSize = 10;
                    if (textLength > amountOfParagraphs * BlockSize) return false;

                    return true;
                }
            }
            else
            {
                if (textLength >= 100 || amountOfParagraphs < 10) return false;

                if (amountOfParagraphs > 10 || amountOfParagraphs < 100)
                {
                    BlockSize = 15;
                    if (textLength > amountOfParagraphs * BlockSize) return false;

                    return true;
                }
                else
                {
                    BlockSize = 10;
                    if (textLength > amountOfParagraphs * BlockSize) return false;

                    return true;
                }
            }
        }
    }
}
