using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Words;

namespace Stegano.Algorithm
{
    class DocumentHelper
    {
        public static string CopyFile(string pathToDirectory, string fileName)
        {
            try
            {
                string newFileName = string.Empty;

                if (fileName.Contains(".docx"))
                    newFileName = fileName.Insert(fileName.Length - 5, "_CHANGED");
                else if (fileName.Contains(".doc") || fileName.Contains(".pdf"))
                    newFileName = fileName.Insert(fileName.Length - 4, "_CHANGED");

                string pathToOriginalFile = string.Concat(pathToDirectory, fileName);
                string pathToNewFile = string.Concat(pathToDirectory, newFileName);

                byte[] byteArray = File.ReadAllBytes(pathToOriginalFile);

                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    File.WriteAllBytes(pathToNewFile, stream.ToArray());
                }

                return pathToNewFile;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void CutParagraphToRun(ref Document wordDoc,ref DocumentBuilder documentBuilder) // разбиваешь слова на буквы
        {
            wordDoc.UnlinkFields();
            foreach (Paragraph par in wordDoc.GetChildNodes(NodeType.Paragraph, true))
            {
                documentBuilder.MoveTo(par);

                if (documentBuilder.CurrentParagraph.Runs.Count > 0)
                {
                    List<Run> runList = new List<Run>();
                    foreach (Run run in documentBuilder.CurrentParagraph.Runs)
                    {
                        if (run.Text.Contains("PAGE \\* MERGEFORMAT"))
                            break;
                        foreach (var letter in run.Text)
                        {
                            Run newRun = run.Clone(true) as Run;
                            if (newRun != null)
                            {
                                newRun.Text = letter.ToString();

                                runList.Add(newRun);
                            }
                        }

                    }

                    documentBuilder.CurrentParagraph.RemoveAllChildren();

                    foreach (var updRun in runList)
                    {
                        documentBuilder.CurrentParagraph.AppendChild(updRun);
                    }
                }
            }
        }
    }
}
