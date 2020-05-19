using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stegano.Algorithm
{
    public class AES
    {
        private AesCryptoServiceProvider cryptoProvider;

        private const string FileName = "privateKey.txt";

        public AES()
        {
            cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.BlockSize = 128;
            cryptoProvider.KeySize = 256;
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string text, string pathToFile)
        {
            cryptoProvider.GenerateKey();
            cryptoProvider.GenerateIV();

            AesFileModel model = new AesFileModel();
            model.Key = Convert.ToBase64String(cryptoProvider.Key);
            model.IV = Convert.ToBase64String(cryptoProvider.IV);

            ICryptoTransform transform = cryptoProvider.CreateEncryptor();

            File.WriteAllText(pathToFile + FileName, JsonConvert.SerializeObject(model));

            var stringBytes = Encoding.Unicode.GetBytes(text);
            byte[] bytesCypherText = transform.TransformFinalBlock(stringBytes, 0, stringBytes.Length);
            var cypherText = Convert.ToBase64String(bytesCypherText);

            return Converter.StringToBinary(cypherText);
        }

        public string Decrypt(string encryptedText, string pathToFile)
        {
            string jsonString = File.ReadAllText(pathToFile);
            AesFileModel model = JsonConvert.DeserializeObject<AesFileModel>(jsonString);

            cryptoProvider.Key = Convert.FromBase64String(model.Key);
            cryptoProvider.IV = Convert.FromBase64String(model.IV);

            ICryptoTransform transform = cryptoProvider.CreateDecryptor();

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            string decryptedString = Encoding.Unicode.GetString(decryptedBytes);

            return decryptedString;
        }
    }

    class AesFileModel
    {
        public string Key { get; set; }

        public string IV { get; set; }
    }
}
