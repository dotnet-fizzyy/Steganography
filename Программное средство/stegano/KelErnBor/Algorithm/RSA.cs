using Stegano.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Stegano.Algorithm
{
    public class RSA : ICrypt
    {
        public string Decrypt(string encryptedText, string pathToFile)
        {
            try
            {
                var csp = new RSACryptoServiceProvider(1024);
                var privKey = csp.ExportParameters(true);

                {
                    //get a stream from the string
                    var sr = new System.IO.StringReader(File.ReadAllText(pathToFile)); //TODO: read from file
                    //we need a deserializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //get the object back from the stream
                    privKey = (RSAParameters)xs.Deserialize(sr);
                }
                byte[] bytesCypherText = Convert.FromBase64String(encryptedText);


                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privKey);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                // get our original plainText back...
                return System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            }
            catch
            {
                return null;
            }
        }

        public string Encrypt(string text, string pathToFile)
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

                File.WriteAllText(pathToFile + "pubKey.txt", pubKeyString);
                File.WriteAllText(pathToFile + "privateKey.txt", privKeyString);

                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(pubKey);

                var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(text);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                var cypherText = Convert.ToBase64String(bytesCypherText);
                return Converter.StringToBinary(cypherText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override string ToString() => "RSA";
    }
}
