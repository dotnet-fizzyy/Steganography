namespace Stegano.Interfaces
{
    public interface ICrypt
    {
        string Encrypt(string text, string pathToFile);

        string Decrypt(string encryptedText, string pathToFile);
    }
}
