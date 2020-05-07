using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Cyclic_Code_WPF.Classes
{
    public class Decoder
    {
        private Polynom polynomCls;              // Вспомогательный класс для работы с циклическим кодом

        private string messageString;            // Информационное сообщение
        private string polynomString;            // Порождающий полином
        private string receiveMessageString;     // Принятое информационное сообщение

        private bool isError;
        private int errorIndex;

        private List<int> receiveMessagePolynom; // Информационное сообщение
        private List<int> generatingPolynom;     // Порождающий полином

        private List<int> syndromPolynom;        // Синдром для нахождения ошибки в принятом сообщении
        private List<int> syndromErrorPolynom;   // Синдром для проверки, в каком бите ошибка
        private List<int> vectorErrorPolynom;    // Вектор ошибок
        private List<int> resultMessagePolynom;  // Исправленное сообщение

        public Decoder(string message, string receivedMessage)
        {
            this.polynomCls = new Polynom(message);

            this.messageString = message;
            this.polynomString = "1011";
            this.receiveMessageString = receivedMessage;

            this.isError = false;
            this.errorIndex = -1;

            this.receiveMessagePolynom = new List<int>();
            this.generatingPolynom = new List<int>();
            this.syndromPolynom = new List<int>();
            this.syndromErrorPolynom = new List<int>();
            this.vectorErrorPolynom = new List<int>();
            this.resultMessagePolynom = new List<int>();
        }

        public void deCode()
        {
            receiveMessagePolynom = polynomCls.convertStringToPolynom(receiveMessageString);
            generatingPolynom = polynomCls.convertStringToPolynom(polynomString);
            syndromPolynom = polynomCls.getCheckBits(receiveMessagePolynom, generatingPolynom);

            isError = polynomCls.syndromIsError(receiveMessagePolynom, generatingPolynom);

            if (isError)
            {
                int receiveMessageLength = messageString.Length + polynomCls.getR();
                for (int i = 0; i < receiveMessageLength; i++)
                {
                    if (vectorErrorPolynom.Count() > 0)
                        vectorErrorPolynom = new List<int>();

                    vectorErrorPolynom.Add(receiveMessageLength - 1 - i);

                    syndromErrorPolynom = polynomCls.ModDiv(vectorErrorPolynom, generatingPolynom);

                    if (polynomCls.Sum(syndromPolynom, syndromErrorPolynom).Count() == 0)
                    {
                        errorIndex = i;
                        break;
                    }
                }

                resultMessagePolynom = polynomCls.Sum(vectorErrorPolynom, receiveMessagePolynom);
            }
            else
                resultMessagePolynom = receiveMessagePolynom;
        }

        public int getR()
        {
            return polynomCls.getR();
        }

        public string print()
        {
            StringBuilder tempStringForPrint = new StringBuilder();

            tempStringForPrint.Append("Принятое сообщение: ");
            tempStringForPrint.Append(receiveMessageString);
            tempStringForPrint.AppendLine();

            tempStringForPrint.Append("Принятое сообщение: ");
            tempStringForPrint.Append(polynomCls.printInPolynomView(receiveMessagePolynom));
            tempStringForPrint.AppendLine();

            tempStringForPrint.Append("Синдром: ");
            tempStringForPrint.Append(polynomCls.printInBitsView(syndromPolynom, polynomCls.getR()));
            tempStringForPrint.AppendLine();

            if (isError)
            {
                tempStringForPrint.Append("Синдром: ");
                tempStringForPrint.Append(polynomCls.printInPolynomView(syndromPolynom));
                tempStringForPrint.AppendLine();

                tempStringForPrint.Append("Найдена ОШИБКА: " + (errorIndex + 1) + " бит");
                tempStringForPrint.AppendLine();

                tempStringForPrint.Append("Вектор ошибок: ");
                tempStringForPrint.Append(polynomCls.printInBitsView(vectorErrorPolynom, receiveMessageString.Length));
                tempStringForPrint.AppendLine();

                tempStringForPrint.Append("Вектор ошибок: ");
                tempStringForPrint.Append(polynomCls.printInPolynomView(vectorErrorPolynom));
                tempStringForPrint.AppendLine();
            }
            else
                tempStringForPrint.Append("Ошибок не найдено").AppendLine();

            tempStringForPrint.Append("Информационное сообщение: ");
            tempStringForPrint.Append(polynomCls.printInBitsView(resultMessagePolynom, receiveMessageString.Length));
            tempStringForPrint.AppendLine();

            tempStringForPrint.Append("Информационное сообщение: ");
            tempStringForPrint.Append(polynomCls.printInPolynomView(resultMessagePolynom));
            tempStringForPrint.AppendLine();

            return polynomCls.printInBitsView(resultMessagePolynom, receiveMessageString.Length);
        }
    }
}