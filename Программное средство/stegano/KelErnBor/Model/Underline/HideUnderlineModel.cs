using Aspose.Words;
using Stegano.Algorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Underline
{
    class HideUnderlineModel
    {

        private Document wordDoc;
        private Random rand;
        private string pathToModifiedFile;
        public HideUnderlineModel(string pathToFile)
        {
            pathToModifiedFile = pathToFile;
            wordDoc = new Document(pathToFile);
            rand = new Random();
        }

        public Task<bool> HideInformation(char[] messageInBits, bool isRandomHiding, bool isVisibleColor)
        {
            try
            {
                //Подготовка документа к сокрытию
                DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);

                DocumentHelper.CutParagraphToRun(ref wordDoc, ref documentBuilder);

                SetHiding(isRandomHiding, isVisibleColor, documentBuilder, messageInBits);


                wordDoc.UpdateFields();
                wordDoc.UpdatePageLayout();
                wordDoc.Save(pathToModifiedFile);


                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }


        private void SetHiding(bool isRandom, bool isVisibleColor, DocumentBuilder documentBuilder, char[] messageInBits)
        {
            if (isRandom)
            {
                int part = wordDoc.GetChildNodes(NodeType.Run, true).Count / messageInBits.Length;
                for (int i = 0; i < messageInBits.Length; i++)
                {
                    var randomPosition = rand.Next(part * i + 1, part * (i + 1));

                    documentBuilder.MoveTo(wordDoc.GetChildNodes(NodeType.Run, true)[randomPosition]);

                    setNodeOption(i);
                }
            }
            else
            {
                for (int i = 0; i < messageInBits.Length; i++)
                {
                    documentBuilder.MoveTo(wordDoc.GetChildNodes(NodeType.Run, true)[i]);

                    setNodeOption(i);
                }
            }

            void setNodeOption(int i)
            {

                if (isVisibleColor)
                {
                    ((Run)documentBuilder.CurrentNode).Font.Color = messageInBits[i] == '1'
                        ? ColorTranslator.FromHtml("#0459ED")
                        : ColorTranslator.FromHtml("#ed0459");
                }

                ((Run)documentBuilder.CurrentNode).Font.UnderlineColor = Color.White;
                ((Run)documentBuilder.CurrentNode).Font.Underline = messageInBits[i] == '1'
                        ? Aspose.Words.Underline.DotDotDashHeavy
                        : Aspose.Words.Underline.WavyHeavy;

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
