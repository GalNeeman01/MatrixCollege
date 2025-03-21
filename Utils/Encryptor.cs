using System.Security.Cryptography;
using System.Text;

namespace Matrix;

public static class Encryptor
{
    public static string GetHashed(string originalText)
    {
        string salt = "404: Joke not found.";
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(originalText, saltBytes, 14, HashAlgorithmName.SHA512);
        byte[] hashByte = rfc.GetBytes(64);
        string hashPassword = Convert.ToBase64String(hashByte);

        return hashPassword;
    }
}
