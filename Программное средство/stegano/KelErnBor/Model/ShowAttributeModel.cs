using Stegano.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model
{
    public class ShowAttributeModel
    {
        public bool VerifySHA512Hash(string input, string pathToHash)
        {
            string fileHash = File.ReadAllText(pathToHash);
            var isHashSame = Hashing.VerifySHA512Hash(input, fileHash);

            return isHashSame;
        }

        public bool VerifyMD5Hash(string input, string pathToHash)
        {
            string fileHash = File.ReadAllText(pathToHash);
            var isHashSame = Hashing.VerifyMd5Hash(input, fileHash);

            return isHashSame;
        }
    }
}
