using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stegano.Algorithm
{
    class ShifrElGamal
    {

        public const int p = 811;
        public static int x = 3;


        public static string CoderElGamal(string value)
        {
            
            string text = Converter.BinaryToString(value);
            string result = "";
            List<BigInteger> cipherASCIIElGamal = Encrypt(Encoding.Unicode.GetBytes(text));
            foreach (long l in cipherASCIIElGamal)
            {
                //MessageBox.Show(Convert.ToString(l));

                result += Converter.StringToBinary(Convert.ToString(l));
                result += Converter.StringToBinary(Convert.ToString(" "));
                //MessageBox.Show(Convert.ToString(result));
                //MessageBox.Show(Convert.ToString(Converter.BinaryToString(result)));


            }
            MessageBox.Show(Convert.ToString("res"+result));


            return result;
        }

        public static string DecoderElGamal(string value)
        {
            string text = Convert.ToString(Converter.BinaryToString(value));


            string[] words = text.Split(new char[] { ' ' });

            List<BigInteger> cipherASCIIElGamal= new List<BigInteger>();// = Encrypt(Encoding.Unicode.GetBytes(text));
            //string stringToParse = String.Empty;
            //string string1, string2;
            //string1 = "12347534159895123";
            //string2 = "987654321357159852";
            //stringToParse = string1;
            //BigInteger number1 = BigInteger.Parse(stringToParse);
            //MessageBox.Show(Convert.ToString(number1));
            //List<BigInteger> cipherASCIIElGamal2 = new List<BigInteger>();// = Encrypt(Encoding.Unicode.GetBytes(text));

            BigInteger helper = 0;
            for (int i = 0; i < words.Length-1; i++)
            {
                helper = BigInteger.Parse(words[i]);
                cipherASCIIElGamal.Add(helper);
            }
          
            return Converter.StringToBinary(Encoding.Unicode.GetString(Decrypt(cipherASCIIElGamal)));
        }
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


        public static List<BigInteger> Encrypt(byte[] m)
        {
            Random random = new Random();
            int k = 0;
            //x = random.Next(2, p - 1);
            int g = FindAtiderivativeRoot(p);
            int y = (int)(BigInteger.ModPow(g, x, p));
            BigInteger a = 0;
            BigInteger b = 0;
            List<BigInteger> cipher = new List<BigInteger>();
            for (int i = 0; i < m.Length; i++)
            {
                k = random.Next(2, p - 2);
                a = BigInteger.ModPow(g, k, p);
                b = (BigInteger.Pow(y, k) * m[i]) % p;
                cipher.Add(a);
                cipher.Add(b);
            }
            return cipher;
        }

        public static byte[] Decrypt(List<BigInteger> c)
        {
            BigInteger axMinusOne = 0;
            BigInteger b = 0;
            List<byte> m = new List<byte>();
            for (int i = 0; i < c.Count; i += 2)
            {
                axMinusOne = EvklidExtend(BigInteger.Pow(c[i], x), p);
                b = c[i + 1];
                m.Add((byte)((axMinusOne * b) % p));
            }
            return m.ToArray();

        }
    }

    //public class ShifrElGamal
    //{
    //    static int g_main;
    //    static BigInteger a;
    //    public static List<BigInteger> array_cipher_text; //= new List<BigInteger>();
    //    public static int x;
    //    public static int p;
    //    public static string CoderElGamal(string value)
    //    {
    //        p = 0;
    //        Random random = new Random();


    //        //p = Search_p();
    //        p = 23;
    //        x = 3;

    //        //x = random.Next(1, p - 1); //Генерирую закрытый ключ
    //        BigInteger y = BigInteger.Pow(g_main, x) % p; //Нахожу открытый ключ

    //        string text = Converter.BinaryToString(value);

    //        string result = "";
    //        array_cipher_text = Cipher(text, p, y);

    //        for (int i = 0; i != text.Length; i++)
    //        {
    //            result += Convert.ToString(Converter.StringToBinary(Convert.ToString(a)));
    //            result += Convert.ToString(Converter.StringToBinary(Convert.ToString(" ")));
    //            result += Convert.ToString(Converter.StringToBinary(Convert.ToString(array_cipher_text[i])));
    //            result += Convert.ToString(Converter.StringToBinary(Convert.ToString(" ")));



    //        }
    //       // MessageBox.Show(Convert.ToString(result));

    //        return result;

    //    }

    //    public static string DecoderElGamal(string value)
    //    {
    //        string text = Converter.BinaryToString(value);
    //        MessageBox.Show(Convert.ToString(text));
    //        //MessageBox.Show(Convert.ToString(a));

    //        //var str = "слово1^слово2^слово3^...^словоN";
    //        // List<BigInteger> array_cipher_text = new List<BigInteger>();;
    //        string[] words = text.Split(new char[] { ' ' });

    //        int a = Convert.ToInt32(text.Substring(0, text.IndexOf(' ')));
    //        //MessageBox.Show(Convert.ToString(a));
    //        //MessageBox.Show(Convert.ToString(((words.Length - 1) / 2)));
    //        int[] array_cipher_text = new int[(words.Length - 1) / 2];

    //        p = 23;
    //        x = 3;

    //        //BigInteger y = BigInteger.Pow(g_main, x) % p;

    //        int j = 0;
    //        for (int i = 0; i < words.Length; i++)
    //        {
    //            //MessageBox.Show(Convert.ToString("i" + i));

    //            if (i % 2 != 0)
    //            {
    //                //MessageBox.Show(Convert.ToString("j" + j));

    //                //MessageBox.Show(Convert.ToString("g" + words[i]));

    //                array_cipher_text[j] = Convert.ToInt32(words[i]);
    //               // MessageBox.Show(Convert.ToString("c" + array_cipher_text[j]));

    //                j++;

    //            }
    //        }

    //        ////MessageBox.Show(Convert.ToString(Cipher_RAZ(a, 2, array_cipher_text, 3, 23)));


    //        ////return Cipher_RAZ(a, 2, array_cipher_text, x, p);
    //        //return "a";


    //        string save_text = "";
    //        BigInteger integer;

    //        for (int i = 0; i != (words.Length - 1) / 2; i++)
    //        {
    //            integer = (array_cipher_text[i] * (BigInteger.Pow(a, p - 1 - x))) % p;
    //            MessageBox.Show("result" + array_cipher_text[i]);
    //            MessageBox.Show("result2" + integer);

    //            save_text += (char)integer;
    //            MessageBox.Show("save_text" + integer);

    //        }

    //        return save_text;
    //    }
    //    public static bool Search_g(int p, int g)
    //    {
    //        bool boolean = false;
    //        List<BigInteger> array_mod_number = new List<BigInteger>();


    //        BigInteger integer = ((BigInteger.Pow(g, 1)) % p);
    //        array_mod_number.Add(integer);


    //        for (int i = 2; i != p; i++)
    //        {
    //            integer = BigInteger.Pow(g, i) % p;
    //            for (int j = 0; j != i - 1; j++)
    //            {
    //                if (array_mod_number[j] == integer)
    //                {
    //                    g--;
    //                    array_mod_number.Clear();
    //                    i = 1;
    //                    integer = BigInteger.Pow(g, 1) % p;
    //                    array_mod_number.Add(integer);
    //                    break;
    //                }

    //                if ((j == i - 2) && (array_mod_number[j] != integer))
    //                {
    //                    array_mod_number.Add(integer);
    //                }
    //            }
    //        }

    //        g_main = g;

    //        boolean = true;

    //        return boolean;
    //    }


    //    public static int Search_p()
    //    {
    //        Random random = new Random();
    //        int p;//= 0;
    //        Boolean boolean = false;


    //        do
    //        {
    //            p = random.Next(2000, 2500);

    //            for (int i = 2; i != p; i++)
    //            {
    //                if (i == p - 1)
    //                {
    //                    boolean = Search_g(p, p - 1);
    //                    break;
    //                }

    //                if (p % i == 0)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //        while (boolean == false);

    //        return p;
    //    }


    //    public static List<BigInteger> Cipher(string text,int p, BigInteger y)
    //    {
    //        //string text = Converter.StringToBinary(text);
    //        List<BigInteger> array = new List<BigInteger>();
    //        Random random = new Random();
    //        int k = random.Next(1, p - 1);


    //        for (int i = 0; i != text.Length; i++)
    //        {
    //            a = BigInteger.Pow(g_main, k) % p;
    //            array.Add((BigInteger.Pow(y, k) * (int)text[i]) % p);
    //        }

    //        return array;
    //    }

    //    public static string Cipher_RAZ(int a, int length_text, List<BigInteger> array_number, int x, int p)
    //    {
    //        string save_text = "";
    //        BigInteger integer;

    //        for (int i = 0; i != length_text; i++)
    //        {
    //            integer = (array_number[i] * (BigInteger.Pow(a, p - 1 - x))) % p;
    //            save_text += (char)integer;
    //        }


    //        return save_text;
    //    }
    //}
}
