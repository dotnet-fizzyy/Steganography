using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Stegano.Algorithm;

namespace Stegano.Model
{
    class HideAproshModel
    {

        private Document wordDoc;
        private Random rand;
        private string pathToModifiedFile;
        public HideAproshModel(string pathToFile)
        {
            pathToModifiedFile = pathToFile;
            wordDoc = new Document(pathToFile);
            rand = new Random();
        }

        public bool HideInformation(char[] messageInBit, bool isVisibleColor, bool isRandom, string zeroBitSpacing, string soloBitSpacing) // апрош
        {
            try
            {
                DocumentBuilder documentBuilder = new DocumentBuilder(wordDoc);

                DocumentHelper.CutParagraphToRun(ref wordDoc, ref documentBuilder);

                SetHiding(isRandom, isVisibleColor, documentBuilder, messageInBit, zeroBitSpacing, soloBitSpacing);

                wordDoc.Save(pathToModifiedFile);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void SetHiding(bool isRandom, bool isVisibleColor, DocumentBuilder documentBuilder,
            char[] messageInBits,string zeroBitSpacing, string soloBitSpacing) 
        {
            if (isRandom)
            {
                int part = wordDoc.GetChildNodes(NodeType.Run, true).Count / messageInBits.Length;
                for (int i = 0; i < messageInBits.Length; i++)
                {
                    var randomPosition = rand.Next(part * i + 1, part * (i + 1));
                   
                    documentBuilder.MoveTo(wordDoc.GetChildNodes(NodeType.Run, true)[randomPosition]);
                    if (messageInBits[i] == '1')
                    {
                        ((Run) documentBuilder.CurrentNode).Font.Spacing = Double.Parse(soloBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture);
                        ((Run) documentBuilder.CurrentNode).Font.Color = isVisibleColor
                            ? Color.Green
                            : ((Run) documentBuilder.CurrentNode).Font.Color;
                    }

                    else
                    {
                        ((Run)documentBuilder.CurrentNode).Font.Spacing = Double.Parse(zeroBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture);
                        ((Run)documentBuilder.CurrentNode).Font.Color = isVisibleColor
                            ? Color.Red
                            : ((Run)documentBuilder.CurrentNode).Font.Color;
                    }
                }
            }
            else
            {
                for (int i = 0; i < messageInBits.Length; i++)
                {
                    documentBuilder.MoveTo(wordDoc.GetChildNodes(NodeType.Run, true)[i]);
                    if (messageInBits[i] == '1')
                    {
                        ((Run)documentBuilder.CurrentNode).Font.Spacing = Double.Parse(soloBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture);
                        ((Run)documentBuilder.CurrentNode).Font.Color = isVisibleColor
                            ? Color.Green
                            : ((Run)documentBuilder.CurrentNode).Font.Color;
                    }

                    else
                    {
                        ((Run)documentBuilder.CurrentNode).Font.Spacing = Double.Parse(zeroBitSpacing.Split(' ')[1], CultureInfo.InvariantCulture);
                        ((Run)documentBuilder.CurrentNode).Font.Color = isVisibleColor
                            ? Color.Red
                            : ((Run)documentBuilder.CurrentNode).Font.Color;
                    }
                }

            }
        }
    }
}
