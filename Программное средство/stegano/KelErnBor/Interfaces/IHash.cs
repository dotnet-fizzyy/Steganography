namespace Stegano.Interfaces
{
    public interface IHash
    {
        string GetHash(string input);

        bool VerifyHash(string input, string hashPath);
    }
}
