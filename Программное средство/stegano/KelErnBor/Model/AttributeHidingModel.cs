using Aspose.Words;
using Stegano.Algorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model
{
    public class AttributeHidingModel
    {
        private Document wordDoc;
        private Random rand;
        private string pathToModifiedFile;
        public AttributeHidingModel(string pathToFile)
        {
            pathToModifiedFile = pathToFile;
            wordDoc = new Document(pathToFile);
            rand = new Random();
        }

        public Task<bool> HideInformation(char[] messageInBits, bool isVisibleColor, bool isEncoded, bool isEncrypted)
        {
            try
            {
                //Подготовка документа к сокрытию
                DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);

                DocumentHelper.CutParagraphToRun(ref wordDoc, ref documentBuilder);

                wordDoc.UpdateFields();
                wordDoc.UpdatePageLayout();

                SetHiding(wordDoc, isVisibleColor, messageInBits, isEncoded, isEncrypted);

                wordDoc.Save(pathToModifiedFile);


                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }


        private void SetHiding(Document doc, bool isVisibleColor, char[] messageInBits, bool isEncoded, bool isEncrypted)
        {
            var message = isEncoded || string.IsNullOrEmpty(Converter.BinaryToString(new string(messageInBits))) ? new string(messageInBits) : Converter.BinaryToString(new string(messageInBits));
            AttributeHiding attributeHiding = new AttributeHiding(doc, isEncrypted, isVisibleColor, pathToModifiedFile);
            attributeHiding.HideInfoInAttribute(message);
        }

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

                return (counterLetters / 8) > 0 ? counterLetters / 8 : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static string AddAdditionalBits(string messageInBits)
        {

            StringBuilder sb = new StringBuilder();
            //string additionalBits = "110";
            for (int i = 0; i < messageInBits.Length; i += 4)
                sb.Append(messageInBits.Substring(i, 4) + GethVeryfiedBits(messageInBits.Substring(i, 4)));
            //sb.Append(messageInBits.Substring(i, 4));

            return sb.ToString();
        }

        private static string GethVeryfiedBits(string bitWord)
        {
            string genPolinom = "1011";

            var pow = (int)Math.Round(Math.Log((double)bitWord.Length, 2)) + 1;
            if ((int)Math.Pow(2, pow) < bitWord.Length + pow)
            {
                pow -= 1;
            }

            /////
            List<int?> inputPolinomList = new List<int?>();
            for (int i = 0; i < bitWord.Length; i++)
            {
                if (bitWord[i] == '1')
                    inputPolinomList.Add(bitWord.Length - 1 - i);
            }

            List<int?> genPolinomList = new List<int?>();
            for (int i = 0; i < genPolinom.Length; i++)
            {
                if (genPolinom[i] == '1')
                    genPolinomList.Add(genPolinom.Length - 1 - i);
            }
            /////
            string newInputPolinom = String.Empty;
            int j = 0;
            while (inputPolinomList.Count() > j)
            {
                inputPolinomList[j] += pow;
                newInputPolinom += inputPolinomList[j].ToString() + " ";
                j++;
            }
            /////
            List<int?> verSym = ModDiv(inputPolinomList, genPolinomList);

            return GetBitView(verSym);
        }

        public static List<int?> ModDiv(List<int?> firstPol, List<int?> secondPol)
        {
            List<int?> result = new List<int?>();
            List<int?> temp = new List<int?>();

            int i = 0;
            while (firstPol.Count() > i)
            {
                temp.Add(firstPol[i]);
                i++;
            }
            i = 0;
            result = temp;
            while (temp.Count() != 0 && temp.Max().Value - secondPol.Max().Value >= 0)
            {
                temp = new List<int?>(Sum(temp, Mul(temp.Max().Value - secondPol.Max().Value, secondPol)));
                result = temp;
            }

            return result;

        }

        public static List<int?> Sum(List<int?> firstPol, List<int?> secondPol)
        {
            List<int?> result = new List<int?>();


            result = firstPol.Except(secondPol).ToList<int?>();
            result = result.Concat(secondPol.Except(firstPol).ToList<int?>()).ToList<int?>();
            result.Sort();
            result.Reverse();

            return result;

        }
        public static List<int?> Mul(int? firstCoef, List<int?> secondPol)
        {
            List<int?> result = new List<int?>();
            int i = 0;


            while (secondPol.Count() > i)
            {
                result.Add(secondPol[i] + firstCoef);
                i++;

            }

            return result;

        }

        private static string GetBitView(List<int?> polinom)
        {
            string bitView = String.Empty;
            if (polinom.Count == 0)
                return "000";
            int lenght = polinom.Max().Value;
            for (int i = lenght; i > -1; i--)
            {
                if (polinom.Any(p => p.Value == i))
                {
                    bitView += "1";

                }
                else
                {
                    bitView += "0";
                }
            }
            if (bitView.Length == 1)
                bitView = bitView.Insert(0, "00");
            if (bitView.Length == 2)
                bitView = bitView.Insert(0, "0");

            return bitView;
        }
    }
}
