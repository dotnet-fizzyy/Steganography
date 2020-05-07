using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Cyclic_Code_WPF.Classes
{
    class Polynom
    {
        private int r;

        public Polynom(string message)
        {
            this.r = calculateR(message);
        }

        private int calculateR(string message)
        {
            double temp = Math.Log((double)(message.Length), (double)(2)) + 1;

            if (temp == Math.Truncate(temp))
                return (int)Math.Truncate(temp);
            else if (temp > Math.Truncate(temp))
                return (int)Math.Truncate(temp) + 1;

            return 0;
        }

        public int getR()
        {
            return r;
        }

        public List<int> convertStringToPolynom(string message)
        {
            List<int> polynom = new List<int>();

            for (int i = 0; i < message.Count(); i++)
            {
                if (message.Substring(i, 1) == "1")
                    polynom.Add(message.Count() - 1 - i);
            }

            return polynom;
        }

        public List<int> convertPolynomToFullPolynom(List<int> polynom)
        {
            for (int i = 0; i < polynom.Count; i++)
                polynom[i] += r;

            return polynom;
        }

        public List<int> ModDiv(List<int> messagePolynom, List<int> generatingPolynom)
        {
            while (messagePolynom.Count() != 0 && messagePolynom.Max() - generatingPolynom.Max() >= 0)
                messagePolynom = new List<int>(Sum(messagePolynom, Mul(generatingPolynom, messagePolynom.Max() - generatingPolynom.Max())));

            return messagePolynom;
        }

        public List<int> Sum(List<int> firstPolynom, List<int> secondPolynom)
        {
            List<int> result = new List<int>();

            result = firstPolynom.Except(secondPolynom).ToList<int>();
            result = result.Concat(secondPolynom.Except(firstPolynom)).ToList<int>();
            result.Sort();
            result.Reverse();

            return result;
        }

        private List<int> Mul(List<int> polynom, int coefficient)
        {
            List<int> result = new List<int>();

            int i = 0;
            while (polynom.Count() > i)
            {
                result.Add(polynom[i] + coefficient);
                i++;
            }

            return result;
        }

        public List<int> getCheckBits(List<int> receiveMessagePolynom, List<int> generatingPolynom)
        {
            return ModDiv(receiveMessagePolynom, generatingPolynom);
        }

        public bool syndromIsError(List<int> receiveMessagePolynom, List<int> generatingPolynom)
        {
            List<int> syndrom = new List<int>();
            syndrom = ModDiv(receiveMessagePolynom, generatingPolynom);

            if (syndrom.Count() == 0)
                return false;
            else
                return true;
        }

        public string printInBitsView(List<int> polynom, int length)
        {
            string result = String.Empty;

            if (polynom.Count() > 0)
            {
                for (int i = length - 1; i > -1; i--)
                {
                    if (polynom.Any(p => p == i))
                        result += "1";
                    else
                        result += "0";
                }
            }
            else
            {
                for (int i = 0; i < r; i++)
                    result += "0";
            }

            return result;
        }

        public string printInPolynomView(List<int> polynom)
        {
            string result = String.Empty;

            if (polynom.Count() > 0)
            {
                for (int i = polynom.Max(); i > -1; i--)
                {
                    if (polynom.Any(p => p == i))
                    {
                        if (i == 1)
                            result += "X";
                        else if (i == 0)
                            result += "1";
                        else
                            result += "X" + i;

                        result += " + ";
                    }
                }
            }
            else
                return String.Empty;

            result = result.Substring(0, result.Count() - 3);

            return result;
        }
    }
}