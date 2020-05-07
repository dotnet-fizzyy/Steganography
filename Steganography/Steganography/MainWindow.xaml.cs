using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Steganography
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestLibrary();
        }

        private void TestLibrary()
        {
            Document doc = new Document(@"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\LB-4.docx");
            doc.Save(@"D:\Универ\3 Курс\2 семестр\Курсач ЗИ\Aspose2.docx", SaveFormat.Docx);
        }
    }
}
