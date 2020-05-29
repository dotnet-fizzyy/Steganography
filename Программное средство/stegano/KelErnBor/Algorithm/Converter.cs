﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stegano.Algorithm
{
    class Converter
    {
        public static string StringToBinary(string value) 
        {
            //MessageBox.Show(value);

            StringBuilder stringBuilder = new StringBuilder();

            var tmp = Encoding.Unicode.GetBytes(value);
            foreach (byte c in tmp)
            {
                
                stringBuilder.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }

            //MessageBox.Show(stringBuilder.ToString());

            return stringBuilder.ToString();

        }
        
        public static string BinaryToString(string value) 
        {
            List<byte> byteList = new List<byte>();

            try
            {
               
                for (int i = 0; i < value.Length; i += 8) 
                {
                    byteList.Add(Convert.ToByte(value.Substring(i, 8), 2));
                }
            }
            catch
            {
                return string.Empty;
            }
            
            return Encoding.Unicode.GetString(byteList.ToArray());
        }

        public static int[] ToIntArray(string value)
        {
            char[] letters = value.ToCharArray();
            int[] result = new int[letters.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (letters[i] == '1')
                    result[i] = 1;
                else
                    result[i] = 0;
            }

            return result;
        }

        public static string RsaCryptor(string value, string path)
        {
            try
            {

                var csp = new RSACryptoServiceProvider(1024);

                //how to get the private key
                var privKey = csp.ExportParameters(true);

                //and the public key ...
                var pubKey = csp.ExportParameters(false);

                //converting the public key into a string representation
                string pubKeyString;
                {
                    //we need some buffer
                    var sw = new System.IO.StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, pubKey);
                    //get the string from the stream
                    pubKeyString = sw.ToString();
                }

                string privKeyString;
                {
                    //we need some buffer
                    var sw = new System.IO.StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, privKey);

                    privKeyString = sw.ToString();
                }

                File.WriteAllText(path + "pubKey.txt", pubKeyString);
                File.WriteAllText(path + "privateKey.txt", privKeyString);

                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(pubKey);

                var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(value);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                var cypherText = Convert.ToBase64String(bytesCypherText);
                return StringToBinary(cypherText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
        }

        public static Task<string> RsaDecryptor(string value, string pathToPrivateKey)
        {
            try
            {
                var csp = new RSACryptoServiceProvider(1024);
                var privKey = csp.ExportParameters(true);

                {
                    //get a stream from the string
                    var sr = new System.IO.StringReader(File.ReadAllText(pathToPrivateKey)); //TODO: read from file
                    //we need a deserializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //get the object back from the stream
                    privKey = (RSAParameters) xs.Deserialize(sr);
                }
                byte[] bytesCypherText = Convert.FromBase64String(value);


                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privKey);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                // get our original plainText back...
                return Task.FromResult(System.Text.Encoding.Unicode.GetString(bytesPlainTextData));
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public static string ArgbToHex(Color color)
        { 
                     
            StringBuilder hexPart = new StringBuilder();

            hexPart.Append(HexCorrector(color.R.ToString("X")));
            hexPart.Append(HexCorrector(color.G.ToString("X")));
            hexPart.Append(HexCorrector(color.B.ToString("X")));
     
            return hexPart.ToString();
        }

        private static string HexCorrector(string colorPart)
        {
            if (colorPart.Length == 1)
                return colorPart.Insert(0, "0");

            return colorPart;
        }
    }
}
