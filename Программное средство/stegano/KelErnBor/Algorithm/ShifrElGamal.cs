using GalaSoft.MvvmLight.Messaging;
using Stegano.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stegano.Algorithm
{
    class ShifrElGamal : ICrypt
    {

        public const int p = 811;
        public static int x;
        private const string FileName = "privateKey.txt";

        //public static string CoderElGamal(string value)
        //{

        //    string text = Converter.BinaryToString(value);
        //    string result = "";
        //    List<BigInteger> cipherASCIIElGamal = Encrypt(Encoding.Unicode.GetBytes(text));
        //    foreach (long l in cipherASCIIElGamal)
        //    {
        //        //MessageBox.Show(Convert.ToString(l));

        //        result += Converter.StringToBinary(Convert.ToString(l));
        //        result += Converter.StringToBinary(Convert.ToString(" "));
        //        //MessageBox.Show(Convert.ToString(result));
        //        //MessageBox.Show(Convert.ToString(Converter.BinaryToString(result)));


        //    }
        //    MessageBox.Show(Convert.ToString("res" + result));


        //    return result;
        //}

        //public static string DecoderElGamal(string value)
        //{
        //    string text = Convert.ToString(Converter.BinaryToString(value));


        //    string[] words = text.Split(new char[] { ' ' });

        //    List<BigInteger> cipherASCIIElGamal = new List<BigInteger>();// = Encrypt(Encoding.Unicode.GetBytes(text));
        //    //string stringToParse = String.Empty;
        //    //string string1, string2;
        //    //string1 = "12347534159895123";
        //    //string2 = "987654321357159852";
        //    //stringToParse = string1;
        //    //BigInteger number1 = BigInteger.Parse(stringToParse);
        //    //MessageBox.Show(Convert.ToString(number1));
        //    //List<BigInteger> cipherASCIIElGamal2 = new List<BigInteger>();// = Encrypt(Encoding.Unicode.GetBytes(text));

        //    BigInteger helper = 0;
        //    for (int i = 0; i < words.Length - 1; i++)
        //    {
        //        helper = BigInteger.Parse(words[i]);
        //        cipherASCIIElGamal.Add(helper);
        //    }

        //    return Converter.StringToBinary(Encoding.Unicode.GetString(Decrypt(cipherASCIIElGamal)));
        //}
        private static BigInteger EvklidExtend(BigInteger firstNum, BigInteger secondNum)
        {
            BigInteger q, r, z, x1 = 0, x2 = 1, a = firstNum, b = secondNum;

            while (b > 0)
            {
                q = a / b;
                r = a - q * b;
                z = x2 - q * x1;
                a = b;
                b = r;
                x2 = x1;
                x1 = z;

            }
            z = x2;
            if (z > 0)
                return z;
            else
                return z + secondNum;
        }

        public static int FindAtiderivativeRoot(int p)
        {
            int Fp = p - 1;
            int possibleRoot = 2;
            int primaryRoot = 0;
            int counterDividers = 0;

            List<int> dividers = new List<int>();

            for (int i = 2; i < Fp / 2 + 1; i++)
            {
                if (Fp % i == 0)
                    dividers.Add(i);
            }

            while (primaryRoot == 0)
            {
                foreach (int div in dividers)
                {
                    if (BigInteger.ModPow(possibleRoot, div, p) != 1)
                        counterDividers++;
                    else
                        break;
                }
                if (counterDividers == dividers.Count)
                    primaryRoot = possibleRoot;
                else
                {
                    counterDividers = 0;
                    possibleRoot++;

                }

            }
            return primaryRoot;
        }


        public static List<BigInteger> EncryptMethod(byte[] m)
        {
            Random random = new Random();
            x = random.Next(2, p - 1);
                       
            int g = FindAtiderivativeRoot(p);
            int y = (int)(BigInteger.ModPow(g, x, p));
            
            List<BigInteger> cipher = new List<BigInteger>();
            for (int i = 0; i < m.Length; i++)
            {
                int k = random.Next(2, p - 2);
                BigInteger a = BigInteger.ModPow(g, k, p);
                BigInteger b = (BigInteger.Pow(y, k) * m[i]) % p;
                cipher.Add(a);
                cipher.Add(b);
            }
            return cipher;
        }

        public static byte[] DecryptMethod(List<BigInteger> c,string pathToFile)
        {
            List<byte> m = new List<byte>();

            int x = Convert.ToInt32(File.ReadAllText(pathToFile));

            for (int i = 0; i < c.Count; i += 2)
            {
                BigInteger axMinusOne = EvklidExtend(BigInteger.Pow(c[i], x), p);
                BigInteger b = c[i + 1];
                m.Add((byte)((axMinusOne * b) % p));
            }
            return m.ToArray();

        }

        public string Encrypt(string value, string pathToFile)
        {
            string result = "";
            List<BigInteger> cipherASCIIElGamal = EncryptMethod(Encoding.Unicode.GetBytes(value));

            File.WriteAllText(pathToFile + FileName, Convert.ToString(x));

            foreach (long l in cipherASCIIElGamal)
            {
                result += Converter.StringToBinary(Convert.ToString(l));
                result += Converter.StringToBinary(Convert.ToString(" "));
            }

            return result;
        }

        public string Decrypt(string value, string pathToFile)
        {            
            string[] words = value.Split(new char[] { ' ' });

            List<BigInteger> cipherASCIIElGamal = new List<BigInteger>();
            
            for (int i = 0; i < words.Length - 1; i++)
            {
                BigInteger helper = BigInteger.Parse(words[i]);
                cipherASCIIElGamal.Add(helper);
            }

            return Encoding.Unicode.GetString(DecryptMethod(cipherASCIIElGamal,pathToFile));
        }

        public override string ToString() => "El-Gamal";
    }

}